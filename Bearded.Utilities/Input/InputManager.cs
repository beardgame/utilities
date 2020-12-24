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
        private readonly AsyncAtomicUpdating<KeyboardStateSnapshot> keyboardStateSnapshot;
        private readonly AsyncAtomicUpdating<MouseStateSnapshot> mouseStateSnapshot;

        public ReadOnlyCollection<GamePadStateManager> GamePads { get; }

        public InputManager(NativeWindow nativeWindow)
        {
            keyboardState = nativeWindow.KeyboardState;
            mouseState = nativeWindow.MouseState;

            keyboardStateSnapshot = new AsyncAtomicUpdating<KeyboardStateSnapshot>();
            mouseStateSnapshot = new AsyncAtomicUpdating<MouseStateSnapshot>();

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
            keyboardStateSnapshot.SetLastKnownState(new KeyboardStateSnapshot(keyboardState));
            mouseStateSnapshot.SetLastKnownState(new MouseStateSnapshot(mouseState));

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
                keyboardStateSnapshot.Update();
                mouseStateSnapshot.Update();

                MousePosition = mouseStateSnapshot.Current.State?.Position ?? MousePosition;
            }
            else
            {
                keyboardStateSnapshot.UpdateToDefault();
            }

            foreach (var gamePad in GamePads)
            {
                gamePad.Update(windowIsActive);
            }
        }

        public Vector2 MousePosition { get; private set; }
        public int DeltaScroll => MoreMath.RoundToInt(DeltaScrollF);
        public float DeltaScrollF => mouseEvents.DeltaScrollF;

        public bool MouseMoved =>
            mouseStateSnapshot.Current.State?.Position != mouseStateSnapshot.Previous.State?.Position;

        public bool LeftMousePressed => IsMouseButtonPressed(MouseButton.Left);
        public bool LeftMouseHit => IsMouseButtonHit(MouseButton.Left);
        public bool LeftMouseReleased => IsMouseButtonReleased(MouseButton.Left);

        public bool RightMousePressed => IsMouseButtonPressed(MouseButton.Right);
        public bool RightMouseHit => IsMouseButtonHit(MouseButton.Right);
        public bool RightMouseReleased => IsMouseButtonReleased(MouseButton.Right);

        public bool MiddleMousePressed => IsMouseButtonPressed(MouseButton.Middle);
        public bool MiddleMouseHit => IsMouseButtonHit(MouseButton.Middle);
        public bool MiddleMouseReleased => IsMouseButtonReleased(MouseButton.Middle);

        public bool IsMouseButtonPressed(MouseButton button) =>
            mouseStateSnapshot.Current.State?.IsButtonDown(button) ?? false;
        private bool wasMouseButtonPressed(MouseButton button) =>
            mouseStateSnapshot.Previous.State?.IsButtonDown(button) ?? false;
        public bool IsMouseButtonHit(MouseButton button) =>
            IsMouseButtonPressed(button) && !wasMouseButtonPressed(button);
        public bool IsMouseButtonReleased(MouseButton button) =>
            !IsMouseButtonPressed(button) && wasMouseButtonPressed(button);

        public bool IsKeyPressed(Keys k) => keyboardStateSnapshot.Current.State?.IsKeyDown(k) ?? false;
        private bool wasKeyPressed(Keys k) => keyboardStateSnapshot.Previous.State?.IsKeyDown(k) ?? false;
        public bool IsKeyHit(Keys k) => IsKeyPressed(k) && !wasKeyPressed(k);
        public bool IsKeyReleased(Keys k) => !IsKeyPressed(k) && wasKeyPressed(k);

        public bool IsMouseInRectangle(System.Drawing.Rectangle rect) =>
            rect.Contains((int) MousePosition.X, (int) MousePosition.Y);

        public IReadOnlyList<(KeyboardKeyEventArgs args, bool isPressed)> KeyEvents => keyboardEvents.KeyEvents;
        public IReadOnlyList<char> PressedCharacters => keyboardEvents.PressedCharacters;

        private readonly struct MouseStateSnapshot
        {
            public MouseState? State { get; }

            public MouseStateSnapshot(MouseState original)
            {
                State = original.GetSnapshot();
            }
        }

        private readonly struct KeyboardStateSnapshot
        {
            public KeyboardState? State { get; }

            public KeyboardStateSnapshot(KeyboardState original)
            {
                State = original.GetSnapshot();
            }
        }
    }
}
