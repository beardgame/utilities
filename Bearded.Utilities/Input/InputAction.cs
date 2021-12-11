using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Bearded.Utilities.Input.Actions;
using Bearded.Utilities.Linq;

namespace Bearded.Utilities.Input;

public static class InputAction
{
    public static bool IsSameAs(this IAction me, IAction other)
        => other != null && (ReferenceEquals(me, other) || me.ToString() == other.ToString());

    public static IAction None { get; } = new DummyAction("unbound");

    public static IAction AnyOf(params IAction[] actions) => AnyOf((IEnumerable<IAction>)actions);

    public static IAction AnyOf(IEnumerable<IAction> actions)
    {
        if (actions == null)
            throw new ArgumentNullException(nameof(actions));

        var actionList = new List<IAction>();
        foreach (var action in actions)
        {
            switch (action)
            {
                case null:
                    break;
                case OrAction asOr:
                    actionList.Add(asOr.Child1);
                    actionList.Add(asOr.Child2);
                    break;
                case AnyAction asAny:
                    actionList.AddRange(asAny.Actions);
                    break;
                default:
                    actionList.Add(action);
                    break;
            }
        }

        switch (actionList.Count)
        {
            case 0:
                return None;
            case 1:
                return actionList[0];
            case 2:
                return new OrAction(actionList[0], actionList[1]);
            default:
                return new AnyAction(actionList);
        }
    }

    public static IAction Or(this IAction me, IAction other)
    {
        if (me == null)
            throw new ArgumentNullException(nameof(me));
        if (other == null)
            throw new ArgumentNullException(nameof(other));

        return new OrAction(me, other);
    }

    public static IAction Or(this IAction me, params IAction[] others) => me.Or((IEnumerable<IAction>)others);

    public static IAction Or(this IAction me, IEnumerable<IAction> others)
    {
        if (me == null)
            throw new ArgumentNullException(nameof(me));
        if (others == null)
            throw new ArgumentNullException(nameof(others));

        // Call extension method explicitly since there is ambiguity between System.Linq.Enumerable.Prepend and
        // Bearded.Utilities.Linq.Extensions.Prepend. (Introduced in .NET Framework 4.8 and .NET Standard 2.0.)
        // ReSharper disable once InvokeAsExtensionMethod
        return AnyOf(Extensions.Prepend(others, me));
    }

    private abstract class BinaryAction : IAction
    {
        public IAction Child1 { get; }
        public IAction Child2 { get; }

        protected BinaryAction(IAction child1, IAction child2)
        {
            Child1 = child1;
            Child2 = child2;
        }

        protected abstract bool BoolOp(bool one, bool other);
        protected abstract float FloatOp(float one, float other);

        public bool Hit => BoolOp(Child1.Hit, Child2.Hit);
        public bool Active => BoolOp(Child1.Active, Child2.Active);
        public bool Released => BoolOp(Child1.Released, Child2.Released);
        public bool IsAnalog => Child1.IsAnalog || Child2.IsAnalog;
        public float AnalogAmount => FloatOp(Child1.AnalogAmount, Child2.AnalogAmount);

        public bool Equals(IAction other) => this.IsSameAs(other);
    }

    private class OrAction : BinaryAction
    {
        public OrAction(IAction child1, IAction child2)
            : base(child1, child2)
        {
        }

        protected override bool BoolOp(bool one, bool other) => one || other;
        protected override float FloatOp(float one, float other) => Math.Max(one, other);
    }

    private class AnyAction : IAction
    {
        private readonly ReadOnlyCollection<IAction> actions;

        public IEnumerable<IAction> Actions => actions;

        public AnyAction(IEnumerable<IAction> actions)
        {
            this.actions = actions.ToList().AsReadOnly();
        }

        public bool Hit => actions.Any(a => a.Hit);
        public bool Active => actions.Any(a => a.Active);
        public bool Released => actions.Any(a => a.Released);
        public bool IsAnalog => actions.Any(a => a.IsAnalog);
        public float AnalogAmount => actions.Max(a => a.AnalogAmount);

        public bool Equals(IAction other) => this.IsSameAs(other);
    }
}
