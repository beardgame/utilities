using System;
using System.Collections.Generic;
using System.Linq;

namespace Bearded.Utilities.Input
{
    /// <summary>
    /// Contains functionality to create actions that dynamically accesses an underlying action for every check.
    /// This is useful to instantly and automatically updates rebindable controls without having to change references inside game objects.
    /// </summary>
    public static class DeferredAction
    {
        /// <summary>
        /// Creates a new deferred action with the given action selector.
        /// </summary>
        /// <param name="actionSelector">The selector to use to get the real action every time the returned action is used.</param>
        public static IAction From(Func<IAction> actionSelector)
        {
            return new LambdaAction(actionSelector);
        }

        /// <summary>
        /// Creates an action that combines several deferred actions.
        /// </summary>
        /// <param name="actionSelectors">The selectors to use to get the real actions eveyr time the returned action is used.</param>
        public static IAction Any(IEnumerable<Func<IAction>> actionSelectors)
        {
            return InputAction.AnyOf(actionSelectors.Select(From));
        }

        /// <summary>
        /// Creates an action that combines several deferred actions.
        /// </summary>
        /// <param name="actionSelectors">The selectors to use to get the real actions eveyr time the returned action is used.</param>
        public static IAction Any(params Func<IAction>[] actionSelectors)
        {
            return Any((IEnumerable<Func<IAction>>)actionSelectors);
        }

        private sealed class LambdaAction : IAction
        {
            private readonly Func<IAction> action;

            public LambdaAction(Func<IAction> actionSelector)
            {
                this.action = actionSelector;
            }

            public bool Hit
            {
                get { return this.action().Hit; }
            }

            public bool Active
            {
                get { return this.action().Active; }
            }

            public bool Released
            {
                get { return this.action().Released; }
            }

            public bool IsAnalog
            {
                get { return this.action().IsAnalog; }
            }

            public float AnalogAmount
            {
                get { return this.action().AnalogAmount; }
            }

            public bool Equals(IAction other)
            {
                return this.IsSameAs(other);
            }
        }
    }
}
