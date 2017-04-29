using System.Collections.ObjectModel;
using System.Linq;
using OpenTK;
using OpenTK.Input;

namespace Bearded.Utilities.Input
{
    /// <summary>
    /// The class for managing input.
    /// </summary>
    public static class InputManager
    {
        #region Fields
        private static KeyboardState currentKbState, prevKbState;
        private static MouseState currentMouseState, prevMouseState;
        private static MouseDevice mouse;
        private static ReadOnlyCollection<GamePadStateContainer> gamepads;
        #endregion

        #region Methods
        /// <summary>
        /// Sets the mouse device to use for determining the current mouse position.
        /// </summary>
        /// <param name="mouse"></param>
        public static void SetMouse(MouseDevice mouse)
        {
            InputManager.mouse = mouse;
        }

        /// <summary>
        /// Initializes the input manager.
        /// </summary>
        /// <param name="mouse">The mouse device to use for determining the current mouse position.</param>
        public static void Initialize(MouseDevice mouse)
        {
            InputManager.mouse = mouse;

            InputManager.gamepads =
                Enumerable.Range(0, int.MaxValue - 1)
                    .TakeWhile(i => GamePad.GetState(i).IsConnected)
                    .Select(GamePadStateContainer.ForId)
                    .ToList().AsReadOnly();
        }

        /// <summary>
        /// Moves the input states one frame forward.
        /// </summary>
        public static void Update()
        {
            // Move the keyboard state forward
            InputManager.prevKbState = InputManager.currentKbState;
            InputManager.currentKbState = Keyboard.GetState();
            // Move the mouse state forward
            InputManager.prevMouseState = InputManager.currentMouseState;
            InputManager.currentMouseState = mouse.GetCursorState();
            // Move the gamepad states forward
            foreach (var gamepad in InputManager.gamepads)
            {
                gamepad.Update();
            }
        }
        #endregion

        #region Keyboard
        /// <summary>
        /// Determines whether the specified key is held down.
        /// </summary>
        /// <param name="k"></param>
        /// <returns>True if the specified key is held down.</returns>
        public static bool IsKeyPressed(Key k)
        {
            return InputManager.currentKbState.IsKeyDown(k);
        }

        /// <summary>
        /// Determines whether the specified key was hit in the last frame.
        /// </summary>
        /// <param name="k"></param>
        /// <returns>True if the specified key was hit in the last frame.</returns>
        public static bool IsKeyHit(Key k)
        {
            return InputManager.IsKeyPressed(k) && InputManager.prevKbState.IsKeyUp(k);
        }

        /// <summary>
        /// Determines whether the specified key was released in the last frame.
        /// </summary>
        /// <param name="k"></param>
        /// <returns>True if the specified key was released in the last frame.</returns>
        public static bool IsKeyReleased(Key k)
        {
            return !InputManager.IsKeyPressed(k) && InputManager.prevKbState.IsKeyDown(k);
        }
        #endregion

        #region Mouse
        /// <summary>
        /// Current mouse position.
        /// </summary>
        public static Vector2 MousePosition => new Vector2(mouse.X, mouse.Y);

        /// <summary>
        /// Whether the mouse was moved in the last frame.
        /// </summary>
        public static bool MouseMoved => currentMouseState.X != prevMouseState.X
                                         || currentMouseState.Y != prevMouseState.Y;

        /// <summary>
        /// The difference in scroll wheel values in the last frame.
        /// </summary>
        public static int DeltaScroll => currentMouseState.ScrollWheelValue - prevMouseState.ScrollWheelValue;

        /// <summary>
        /// Determines whether the specified mouse button is held down.
        /// </summary>
        /// <param name="button"></param>
        /// <returns></returns>
        public static bool IsMouseButtonPressed(MouseButton button)
        {
            return currentMouseState[button];
        }

        /// <summary>
        /// Determines whether the specified mouse button was hit in the last frame.
        /// </summary>
        /// <param name="button"></param>
        /// <returns></returns>
        public static bool IsMouseButtonHit(MouseButton button)
        {
            return currentMouseState[button] && !prevMouseState[button];
        }

        /// <summary>
        /// Determines whether the specified mouse button was released in the last frame.
        /// </summary>
        /// <param name="button"></param>
        /// <returns></returns>
        public static bool IsMouseButtonReleased(MouseButton button)
        {
            return !currentMouseState[button] && prevMouseState[button];
        }

        /// <summary>
        /// Whether the left mouse button is held down.
        /// </summary>
        public static bool LeftMousePressed => IsMouseButtonPressed(MouseButton.Left);

        /// <summary>
        /// Whether the left mouse button was hit in the last frame.
        /// </summary>
        public static bool LeftMouseHit => IsMouseButtonHit(MouseButton.Left);

        /// <summary>
        /// Whether the left mouse button was released in the last frame.
        /// </summary>
        public static bool LeftMouseReleased => IsMouseButtonReleased(MouseButton.Left);

        /// <summary>
        /// Whether the right mouse button is held down.
        /// </summary>
        public static bool RightMousePressed => IsMouseButtonPressed(MouseButton.Right);

        /// <summary>
        /// Whether the right mouse button was hit in the last frame.
        /// </summary>
        public static bool RightMouseHit => IsMouseButtonHit(MouseButton.Right);

        /// <summary>
        /// Whether the right mouse button was released in the last frame.
        /// </summary>
        public static bool RightMouseReleased => IsMouseButtonReleased(MouseButton.Right);

        /// <summary>
        /// Whether the middle mouse button is held down.
        /// </summary>
        public static bool MiddleMousePressed => IsMouseButtonPressed(MouseButton.Middle);

        /// <summary>
        /// Whether the middle mouse button was hit in the last frame.
        /// </summary>
        public static bool MiddleMouseHit => IsMouseButtonHit(MouseButton.Middle);

        /// <summary>
        /// Whether the middle mouse button was released in the last frame.
        /// </summary>
        public static bool MiddleMouseReleased => IsMouseButtonReleased(MouseButton.Middle);

        /// <summary>
        /// Checks if the cursor is currently contained in a specified rectangle in screen coordinates.
        /// </summary>
        /// <param name="rect">The rectangle to check against.</param>
        /// <returns>True if the cursor is contained in the specified rectangle.</returns>
        public static bool IsMouseInRectangle(System.Drawing.Rectangle rect)
        {
            return rect.Contains(mouse.X, mouse.Y);
        }
        #endregion

        #region Gamepad
        /// <summary>
        /// Collection of all connected gamepads.
        /// </summary>
        public static ReadOnlyCollection<GamePadStateContainer> GamePads { get { return InputManager.gamepads; } }

        #region GamepadStateContainer
        /// <summary>
        /// Container for the current and previous state of a gamepad.
        /// </summary>
        public class GamePadStateContainer
        {
            /// <summary>
            /// Unique gamepad id.
            /// </summary>
            public int Id { get; private set; }

            /// <summary>
            /// Current state of the gamepad.
            /// </summary>
            public GamePadState CurrentState { get; private set; }

            /// <summary>
            /// Previous state of the gamepad.
            /// </summary>
            public GamePadState PreviousState { get; private set; }

            /// <summary>
            /// Creates a new gamepad state container for the gamepad with the specified id.
            /// </summary>
            /// <param name="id">The id of the gamepad.</param>
            /// <returns>A new container for the specified gamepad.</returns>
            internal static GamePadStateContainer ForId(int id)
            {
                return new GamePadStateContainer(id);
            }

            private GamePadStateContainer(int id)
            {
                this.Id = id;
                this.CurrentState = GamePad.GetState(id);
                this.PreviousState = this.CurrentState;
            }

            /// <summary>
            /// Moves the gamepad state forward one frame.
            /// </summary>
            // ReSharper disable once MemberHidesStaticFromOuterClass
            internal void Update()
            {
                this.PreviousState = this.CurrentState;
                this.CurrentState = GamePad.GetState(this.Id);
            }
        }
        #endregion
        #endregion
    }
}
