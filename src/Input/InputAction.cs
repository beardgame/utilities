using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Bearded.Utilities.Linq;

namespace Bearded.Utilities.Input
{
    /// <summary>
    /// Contains methods to easily create and modify actions.
    /// </summary>
    public static class InputAction
    {
        /// <summary>
        /// Creates an action from a given string identifier.
        /// Throws an exception if the given string specifies a key or gamepad control that does not exist.
        /// </summary>
        /// <param name="value">The action name.</param>
        /// <returns>A valid and functional action if the name is valid;
        /// null if neither a keyboard nor a gamepad action is specified;
        /// a non functional dummy action with correct name, if a gamepad action for a not connected gamepad is specified.</returns>
        public static IAction FromString(string value)
        {
            return value.ToLowerInvariant().Trim() == "unbound"
                ? InputAction.Unbound
                : KeyboardAction.FromString(value) ?? GamePadAction.FromString(value);
        }

        /// <summary>
        /// Returns a sequence of all currently valid actions, including all keyboard keys, as well as all controls for all connected gamepads.
        /// </summary>
        public static IEnumerable<IAction> GetAllAvailable()
        {
            return KeyboardAction.GetAll().Concat(InputManager.GamePads.SelectMany(GamePadAction.GetAll));
        }

        /// <summary>
        /// Compares two actions by their string names for equality.
        /// </summary>
        /// <returns>True if the actions are or represent the same control; false otherwise.</returns>
        public static bool IsSameAs(this IAction me, IAction other)
        {
            return me == other || me.ToString() == other.ToString();
        }

        private static readonly IAction unbound = new DummyAction("unbound");
        /// <summary>
        /// A non-functional dummy action to use as placeholder or default/unbound value.
        /// </summary>
        public static IAction Unbound { get { return InputAction.unbound; } }

        /// <summary>
        /// Returns a new action that is activated by any of the given actions.
        /// </summary>
        /// <param name="actions">A sequence of actions to combine into one.</param>
        public static IAction AnyOf(params IAction[] actions)
        {
            return InputAction.AnyOf((IEnumerable<IAction>)actions);
        }

        /// <summary>
        /// Returns a new action that is activated by any of the given actions.
        /// </summary>
        /// <param name="actions">A sequence of actions to combine into one.</param>
        public static IAction AnyOf(IEnumerable<IAction> actions)
        {
            var actionList = new List<IAction>();
            foreach (var action in actions)
            {
                if (action == null)
                    continue;

                var asOr = action as OrAction;
                if (asOr != null)
                {
                    actionList.Add(asOr.Child1);
                    actionList.Add(asOr.Child2);
                    continue;
                }
                var asAny = action as AnyAction;
                if (asAny != null)
                {
                    actionList.AddRange(asAny.Actions);
                    continue;
                }
                actionList.Add(action);
            }

            if (actionList.Count == 0)
                return new DummyAction("unbound");

            if (actionList.Count == 1)
                return actionList[0];

            if (actionList.Count == 2)
                return new OrAction(actionList[0], actionList[1]);

            return new AnyAction(actionList);
        }

        /// <summary>
        /// Returns an action that combines and is activated by both given actions.
        /// </summary>
        /// <param name="me">First action.</param>
        /// <param name="others">Second action.</param>
        public static IAction Or(this IAction me, IEnumerable<IAction> others)
        {
            if (me == null)
                throw new ArgumentNullException("me");
            if (others == null)
                throw new ArgumentNullException("others");

            return InputAction.AnyOf(others.Append(me));
        }

        /// <summary>
        /// Returns an action that combines and is activated by all given actions.
        /// </summary>
        /// <param name="me">One action.</param>
        /// <param name="others">Other actions.</param>
        public static IAction Or(this IAction me, params IAction[] others)
        {
            return me.Or((IEnumerable<IAction>)others);
        }

        private abstract class BinaryAction : IAction
        {
            private readonly IAction child1;
            private readonly IAction child2;

            public IAction Child1 { get { return this.child1; } }
            public IAction Child2 { get { return this.child2; } }

            protected BinaryAction(IAction child1, IAction child2)
            {
                this.child1 = child1;
                this.child2 = child2;
            }

            protected abstract bool boolOp(bool one, bool other);
            protected abstract float floatOp(float one, float other);


            public bool Hit
            {
                get { return this.boolOp(this.child1.Hit, this.child2.Hit); }
            }

            public bool Active
            {
                get { return this.boolOp(this.child1.Active, this.child2.Active); }
            }

            public bool Released
            {
                get { return this.boolOp(this.child1.Released, this.child2.Released); }
            }

            public bool IsAnalog
            {
                get { return this.child1.IsAnalog || this.child2.IsAnalog; }
            }

            public float AnalogAmount
            {
                get { return this.floatOp(this.child1.AnalogAmount, this.child2.AnalogAmount); }
            }

            public bool Equals(IAction other)
            {
                return this.IsSameAs(other);
            }
        }

        private class OrAction : BinaryAction
        {
            public OrAction(IAction child1, IAction child2)
                : base(child1, child2)
            {
            }

            protected override bool boolOp(bool one, bool other)
            {
                return one || other;
            }

            protected override float floatOp(float one, float other)
            {
                return System.Math.Max(one, other);
            }
        }

        private class AnyAction : IAction
        {
            private readonly ReadOnlyCollection<IAction> actions;

            public IEnumerable<IAction> Actions { get { return this.actions; } } 

            public AnyAction(IEnumerable<IAction> actions)
            {
                this.actions = actions.ToList().AsReadOnly();
            }

            public bool Hit
            {
                get { return this.actions.Any(a => a.Hit); }
            }

            public bool Active
            {
                get { return this.actions.Any(a => a.Active); }
            }

            public bool Released
            {
                get { return this.actions.Any(a => a.Released); }
            }

            public bool IsAnalog
            {
                get { return this.actions.Any(a => a.IsAnalog); }
            }

            public float AnalogAmount
            {
                get { return this.actions.Max(a => a.AnalogAmount); }
            }

            public bool Equals(IAction other)
            {
                return this.IsSameAs(other);
            }
        }
    }
}
