using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using OpenToolkit.Mathematics;
using OpenToolkit.Windowing.Common;
using OpenToolkit.Windowing.Common.Input;

namespace Bearded.Utilities.Input
{
    public sealed partial class InputManager
    {
        private readonly INativeWindow nativeWindow;
        private readonly KeyboardEvents keyboardEvents;
        private readonly MouseEvents mouseEvents;

        private readonly AsyncAtomicUpdating<KeyboardState> keyboardState = new AsyncAtomicUpdating<KeyboardState>();
        private readonly AsyncAtomicUpdating<MouseState> mouseState = new AsyncAtomicUpdating<MouseState>();

        public ReadOnlyCollection<GamePadStateManager> GamePads { get; }

        public InputManager(INativeWindow nativeWindow)
        {
            this.nativeWindow = nativeWindow;
            keyboardEvents = new KeyboardEvents(nativeWindow);
            mouseEvents = new MouseEvents(nativeWindow);

            GamePads = Enumerable.Empty<GamePadStateManager>().ToList().AsReadOnly();
            // GamePads = Enumerable.Range(0, int.MaxValue - 1)
            //     .TakeWhile(i => GamePad.GetState(i).IsConnected)
            //     .Select(GamePadStateManager.ForId)
            //     .ToList().AsReadOnly();
        }

        public void ProcessEventsAsync()
        {
            keyboardState.SetLastKnownState(nativeWindow.KeyboardState);
            mouseState.SetLastKnownState(nativeWindow.MouseState);

            foreach (var gamePad in GamePads)
            {
                gamePad.ProcessEventsAsync();
            }
        }

        public void Update(bool windowIsActive)
        {
            if (windowIsActive)
            {
                keyboardEvents.Update();
                mouseEvents.Update();
                keyboardState.Update();
                mouseState.Update();
                MousePosition = mouseState.Current.Position;
            }
            else
            {
                keyboardState.UpdateToDefault();
            }

            foreach (var gamePad in GamePads)
            {
                gamePad.Update(windowIsActive);
            }
        }

        public Vector2 MousePosition { get; private set; }
        public int DeltaScroll => Mathf.RoundToInt(DeltaScrollF);
        public float DeltaScrollF => mouseEvents.DeltaScrollF;

        public bool MouseMoved => mouseState.Current.X != mouseState.Previous.X
                                  || mouseState.Current.Y != mouseState.Previous.Y;

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
    }
}
