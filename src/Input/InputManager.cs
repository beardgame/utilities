using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using OpenTK;
using OpenTK.Input;

namespace Bearded.Utilities.Input
{
    public sealed partial class InputManager
    {
        private readonly INativeWindow nativeWindow;
        private readonly KeyboardEvents keyboardEvents;

        private readonly AsyncAtomicUpdating<KeyboardState> keyboardState = new AsyncAtomicUpdating<KeyboardState>();
        private readonly AsyncAtomicUpdating<MouseState> mouseState = new AsyncAtomicUpdating<MouseState>();

        private bool windowWasActiveLastUpdate;

        public ReadOnlyCollection<GamePadStateManager> GamePads { get; }

        public InputManager(INativeWindow nativeWindow)
        {
            this.nativeWindow = nativeWindow;
            keyboardEvents = new KeyboardEvents(nativeWindow);

            GamePads = Enumerable.Range(0, int.MaxValue - 1)
                .TakeWhile(i => GamePad.GetState(i).IsConnected)
                .Select(GamePadStateManager.ForId)
                .ToList().AsReadOnly();
        }

        public void ProcessEventsAsync()
        {
            keyboardState.SetLastKnownState(Keyboard.GetState());
            mouseState.SetLastKnownState(Mouse.GetCursorState());

            foreach (var gamepad in GamePads)
            {
                gamepad.ProcessEventsAsync();
            }
        }

        public void Update(bool windowIsActive)
        {
            if (windowIsActive)
            {
                keyboardEvents.Update();
                keyboardState.Update();
                mouseState.Update();
                MousePosition = toVector(nativeWindow.PointToClient(
                    new System.Drawing.Point(mouseState.Current.X, mouseState.Current.Y)));
                if (!windowWasActiveLastUpdate)
                {
                    // mouse state is updated in a special way so that scroll delta doesnt jump
                    mouseState.UpdateTo(mouseState.Current);
                }
            }
            else
            {
                keyboardState.UpdateToDefault();
            }

            foreach (var gamepad in GamePads)
            {
                gamepad.Update(windowIsActive);
            }

            windowWasActiveLastUpdate = windowIsActive;
        }

        public Vector2 MousePosition { get; private set; }

        public bool MouseMoved => mouseState.Current.X != mouseState.Previous.X
                                  || mouseState.Current.Y != mouseState.Previous.Y;
        public int DeltaScroll => mouseState.Current.ScrollWheelValue - mouseState.Previous.ScrollWheelValue;
        public float DeltaScrollF => mouseState.Current.WheelPrecise - mouseState.Previous.WheelPrecise;

        public bool LeftMousePressed => IsMouseButtonPressed(MouseButton.Left);
        public bool LeftMouseHit => IsMouseButtonHit(MouseButton.Left);
        public bool LeftMouseReleased => IsMouseButtonReleased(MouseButton.Left);

        public bool RightMousePressed => IsMouseButtonPressed(MouseButton.Right);
        public bool RightMouseHit => IsMouseButtonHit(MouseButton.Right);
        public bool RightMouseReleased => IsMouseButtonReleased(MouseButton.Right);

        public bool MiddleMousePressed => IsMouseButtonPressed(MouseButton.Middle);
        public bool MiddleMouseHit => IsMouseButtonHit(MouseButton.Middle);
        public bool MiddleMouseReleased => IsMouseButtonReleased(MouseButton.Middle);

        public bool IsMouseButtonPressed(MouseButton button) => mouseState.Current[button];
        public bool IsMouseButtonHit(MouseButton button) => mouseState.Current[button] && !mouseState.Previous[button];
        public bool IsMouseButtonReleased(MouseButton button) => !mouseState.Current[button] && mouseState.Previous[button];

        public bool IsKeyPressed(Key k) => keyboardState.Current.IsKeyDown(k);
        public bool IsKeyHit(Key k) => IsKeyPressed(k) && keyboardState.Previous.IsKeyUp(k);
        public bool IsKeyReleased(Key k) => !IsKeyPressed(k) && keyboardState.Previous.IsKeyDown(k);
        
        public bool IsMouseInRectangle(System.Drawing.Rectangle rect) => rect.Contains((int) MousePosition.X, (int) MousePosition.Y);

        public IReadOnlyList<(KeyboardKeyEventArgs args, bool isPressed)> KeyEvents => keyboardEvents.KeyEvents;
        public IReadOnlyList<char> PressedCharacters => keyboardEvents.PressedCharacters;
        
        private static Vector2 toVector(System.Drawing.Point point)
        {
            return new Vector2(point.X, point.Y);
        }
    }
}
