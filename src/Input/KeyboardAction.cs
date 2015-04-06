using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK.Input;

namespace Bearded.Utilities.Input
{
    /// <summary>
    /// Contains functionality to create actions for keyboard keys.
    /// </summary>
    public static class KeyboardAction
    {
        /// <summary>
        /// Returns a keyboard action for the specified key.
        /// </summary>
        public static IAction FromKey(Key key)
        {
            return new KeyAction(key);
        }

        /// <summary>
        /// Returns a keyboard actions for the specified string.
        /// Throws an exception if the given string specifies a keyboard action with an unknown or invalid key.
        /// </summary>
        /// <param name="name">Name of the keyboard action.</param>
        /// <returns>A functional action if the given name is valid;
        /// null if no keyboard action is specified.</returns>
        public static IAction FromString(string name)
        {
            var lower = name.ToLowerInvariant().Trim();
            if (!lower.StartsWith("keyboard:"))
                return null;

            var keyName = name.Substring(9).Trim();

            var key = (Key)Enum.Parse(typeof(Key), keyName, true);

            if (key == Key.Unknown)
                throw new ArgumentException("Keyboard key name unknown.", "name");

            return new KeyAction(key);
        }

        /// <summary>
        /// Returns a sequence of all keyboard actions.
        /// </summary>
        public static IEnumerable<IAction> GetAll()
        {
            return ((Key[])Enum.GetValues(typeof(Key))).Select(k => new KeyAction(k));
        }



        private sealed class KeyAction : DigitalAction
        {
            private readonly Key key;

            public KeyAction(Key key)
            {
                this.key = key;
            }

            public override bool Hit
            {
                get { return InputManager.IsKeyHit(this.key); }
            }

            public override bool Active
            {
                get { return InputManager.IsKeyPressed(this.key); }
            }

            public override bool Released
            {
                get { return InputManager.IsKeyReleased(this.key); }
            }

            public override string ToString()
            {
                return "keyboard:" + this.key;
            }
        }
    }
}
