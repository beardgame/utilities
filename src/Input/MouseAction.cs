using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK.Input;

namespace Bearded.Utilities.Input
{
    /// <summary>
    /// Contains functionality to create actions for mouse buttons.
    /// </summary>
    public static class MouseAction
    {
        /// <summary>
        /// Returns a mouse action for the specified mouse button.
        /// </summary>
        public static IAction FromButton(MouseButton button)
        {
            return new ButtonAction(button);
        }

        /// <summary>
        /// Returns a mouse action for the left mouse button.
        /// </summary>
        public static IAction ForLeftButton => FromButton(MouseButton.Left);

        /// <summary>
        /// Returns a mouse action for the right mouse button.
        /// </summary>
        public static IAction ForRightButton => FromButton(MouseButton.Right);

        /// <summary>
        /// Returns a mouse action for the middle mouse button.
        /// </summary>
        public static IAction ForMiddleButton => FromButton(MouseButton.Middle);

        /// <summary>
        /// Returns a sequence of all mouse actions.
        /// </summary>
        public static IEnumerable<IAction> GetAll()
        {
            return ((MouseButton[])Enum.GetValues(typeof(MouseButton)))
                .Select(k => new ButtonAction(k));
        }

        private sealed class ButtonAction : DigitalAction
        {
            private readonly MouseButton button;

            public ButtonAction(MouseButton button)
            {
                this.button = button;
            }

            public override bool Hit => InputManager.IsMouseButtonHit(button);
            public override bool Active => InputManager.IsMouseButtonPressed(button);
            public override bool Released => InputManager.IsMouseButtonReleased(button);

            public override string ToString()
            {
                return "mouse:" + button;
            }
        }
    }
}
