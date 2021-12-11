using System;
using System.Collections.Generic;
using System.Linq;
using Bearded.Utilities.Input.Actions;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Bearded.Utilities.Input;

partial class InputManager
{
    public readonly partial struct ActionConstructor
    {
        public MouseActions Mouse => new MouseActions(manager);
    }

    public readonly struct MouseActions
    {
        private readonly InputManager manager;

        public MouseActions(InputManager inputManager)
        {
            manager = inputManager;
        }

        public IAction LeftButton => FromButton(MouseButton.Left);
        public IAction RightButton => FromButton(MouseButton.Right);
        public IAction MiddleButton => FromButton(MouseButton.Middle);

        public IEnumerable<IAction> All
        {
            get
            {
                var inputManager = manager;
                return ((MouseButton[])Enum.GetValues(typeof(MouseButton)))
                    .Select(k => new MouseButtonAction(inputManager, k));
            }
        }

        public IAction FromButton(MouseButton button) => new MouseButtonAction(manager, button);

    }
}
