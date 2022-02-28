namespace Bearded.Utilities.Input;

partial class InputManager
{
    public ActionConstructor Actions => new ActionConstructor(this);

    public readonly partial struct ActionConstructor
    {
        private readonly InputManager manager;

        public ActionConstructor(InputManager inputManager)
        {
            manager = inputManager;
        }

        public IAction None => InputAction.None;

        public IAction FromString(string value)
            => fromLowerCaseTrimmedString(value.ToLowerInvariant().Trim());

        private IAction fromLowerCaseTrimmedString(string value)
        {
            switch (value)
            {
                case "none":
                case "unbound":
                    return None;
                default:
                {
                    // if (Keyboard.TryParseLowerTrimmedString(value, out var action)
                    //     || Gamepad.TryParseLowerTrimmedString(value, out action))
                    //     return action;
                    if (Keyboard.TryParseLowerTrimmedString(value, out var action))
                        return action;

                    return None;
                }
            }
        }
    }
}
