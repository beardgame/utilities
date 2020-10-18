using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Bearded.Utilities.Input
{
    public sealed partial class InputManager
    {
        private readonly KeyboardEvents keyboardEvents;
        private readonly MouseEvents mouseEvents;

        private readonly KeyboardState keyboardState;
        private readonly MouseState mouseState;

        public ReadOnlyCollection<GamePadStateManager> GamePads { get; }

        public InputManager(NativeWindow nativeWindow)
        {
            keyboardState = nativeWindow.KeyboardState;
            mouseState = nativeWindow.MouseState;

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
                MousePosition = mouseState.Position;
            }
            else
            {
                //keyboardState.UpdateToDefault();
            }

            foreach (var gamePad in GamePads)
            {
                gamePad.Update(windowIsActive);
            }
        }

        public Vector2 MousePosition { get; private set; }
        public int DeltaScroll => MoreMath.RoundToInt(DeltaScrollF);
        public float DeltaScrollF => mouseEvents.DeltaScrollF;

        public bool MouseMoved => mouseState.Delta.LengthSquared != 0;

        public bool LeftMousePressed => IsMouseButtonPressed(MouseButton.Left);
        public bool LeftMouseHit => IsMouseButtonHit(MouseButton.Left);
        public bool LeftMouseReleased => IsMouseButtonReleased(MouseButton.Left);

        public bool RightMousePressed => IsMouseButtonPressed(MouseButton.Right);
        public bool RightMouseHit => IsMouseButtonHit(MouseButton.Right);
        public bool RightMouseReleased => IsMouseButtonReleased(MouseButton.Right);

        public bool MiddleMousePressed => IsMouseButtonPressed(MouseButton.Middle);
        public bool MiddleMouseHit => IsMouseButtonHit(MouseButton.Middle);
        public bool MiddleMouseReleased => IsMouseButtonReleased(MouseButton.Middle);

        public bool IsMouseButtonPressed(MouseButton button) => mouseState.IsButtonDown(button);
        public bool IsMouseButtonHit(MouseButton button) =>
            IsMouseButtonPressed(button) && !mouseState.WasButtonDown(button);
        public bool IsMouseButtonReleased(MouseButton button) =>
            !IsMouseButtonPressed(button) && mouseState.WasButtonDown(button);

        public bool IsKeyPressed(Keys k) => keyboardState.IsKeyDown(k);
        public bool IsKeyHit(Keys k) => IsKeyPressed(k) && !keyboardState.WasKeyDown(k);
        public bool IsKeyReleased(Keys k) => !IsKeyPressed(k) && keyboardState.WasKeyDown(k);

        public bool IsMouseInRectangle(System.Drawing.Rectangle rect) =>
            rect.Contains((int) MousePosition.X, (int) MousePosition.Y);

        public IReadOnlyList<(KeyboardKeyEventArgs args, bool isPressed)> KeyEvents => keyboardEvents.KeyEvents;
        public IReadOnlyList<char> PressedCharacters => keyboardEvents.PressedCharacters;
    }
}
