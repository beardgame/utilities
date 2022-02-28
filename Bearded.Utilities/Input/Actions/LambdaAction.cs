using System;
using System.Collections.Generic;
using System.Linq;

namespace Bearded.Utilities.Input.Actions;

public static class DeferredAction
{
    public static IAction From(Func<IAction> actionSelector) => new LambdaAction(actionSelector);

    public static IAction Any(IEnumerable<Func<IAction>> actionSelectors) => InputAction.AnyOf(actionSelectors.Select(From));

    public static IAction Any(params Func<IAction>[] actionSelectors) => Any((IEnumerable<Func<IAction>>)actionSelectors);

    private sealed class LambdaAction : IAction
    {
        private readonly Func<IAction> actionSelector;
        private IAction action => actionSelector();

        public LambdaAction(Func<IAction> actionSelector)
        {
            this.actionSelector = actionSelector;
        }

        public bool Hit => action.Hit;
        public bool Active => action.Active;
        public bool Released => action.Released;
        public bool IsAnalog => action.IsAnalog;
        public float AnalogAmount => action.AnalogAmount;

        public bool Equals(IAction other) => this.IsSameAs(other);
    }
}
