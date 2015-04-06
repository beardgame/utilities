using System;
using System.Collections.Generic;
using System.Linq;
using Bearded.Utilities.Math;
using OpenTK.Input;

namespace Bearded.Utilities.Input
{
    using ButtonSelector = Func<GamePadState, ButtonState>;
    using AxisSelector = Func<GamePadState, float>;

    /// <summary>
    /// Contains functionality to create actions for gamepad controls.
    /// </summary>
    public static class GamePadAction
    {
        /// <summary>
        /// Contains mutable settings for gamepad axes like deadzone and power settings.
        /// </summary>
        public static class Settings
        {
            private static float hitValue = 0.6f;
            private static float releaseValue = 0.4f;
            private static float analogDeadZone = 0.05f;
            private static float analogMaxValue = 0.95f;
            private static float analogPower = 1;

            /// <summary>
            /// The value at which gamepad axes register a hit.
            /// Default is 0.6f.
            /// </summary>
            public static float HitValue
            {
                get { return hitValue; }
                set { hitValue = value.Clamped(0.1f, 0.9f); }
            }

            /// <summary>
            /// The value to which gamepad axes have to return after a hit to reset.
            /// Default is 0.4f.
            /// </summary>
            public static float ReleaseValue
            {
                get { return releaseValue; }
                set { releaseValue = value.Clamped(0.1f, 0.9f); }
            }

            /// <summary>
            /// The dead zone of gamepad axes.
            /// Default is 0.05f.
            /// </summary>
            public static float AnalogDeadZone
            {
                get { return analogDeadZone; }
                set { analogDeadZone = value.Clamped(0, 0.9f); }
            }

            /// <summary>
            /// The maximum value to which gamepad axes are set. In effect the opposite of a deadzone.
            /// Default is 0.95f;
            /// </summary>
            public static float AnalogMaxValue
            {
                get { return analogMaxValue; }
                set { analogMaxValue = value.Clamped(0.1f, 1); }
            }

            /// <summary>
            /// The power to which the gamepad axis input should be raised.
            /// Values below 1 cause lower, and values above 1 result in higher sensitivity.
            /// Default is 1.
            /// </summary>
            public static float AnalogPower
            {
                get { return analogPower; }
                set { analogPower = System.Math.Max(0.01f, value); }
            }

            internal static float AnalogDeadToMaxRange
            {
                get { return AnalogMaxValue - AnalogDeadZone; }
            }

            internal static float AdjustAnalogSignal(float input)
            {
                if (input < AnalogDeadZone)
                    return 0;
                if (input > AnalogMaxValue)
                    return 1;
                return (float)System.Math.Pow(
                    (input - AnalogDeadZone) / AnalogDeadToMaxRange,
                    analogPower);
            }
        }

        /// <summary>
        /// Tries creating a gamepad action from a string.
        /// Throws an exception of the string is for a valid gamepad, but not for a valid gamepad control.
        /// </summary>
        /// <param name="name">The name of the action to create.</param>
        /// <returns>A functional IAction if the specified gamepad exists;
        /// a non functional dummy action with the correct name if the specified gamepad does not exist (or is not connected or found);
        /// null if the specified string is no gamepad action.</returns>
        public static IAction FromString(string name)
        {
            var lower = name.ToLowerInvariant().Trim();
            if (!lower.StartsWith("gamepad"))
                return null;
            var split = lower.Substring(7).Split(':');
            if (split.Length != 2)
                throw new ArgumentException("Gamepad button name must have exactly one ':'.", "name");
            int id;
            if (!int.TryParse(split[0].Trim(), out id))
                throw new ArgumentException("Gamepad button name must include gamepad id.", "name");

            var buttonName = split[1].Trim();

            if (id >= InputManager.GamePads.Count)
            {
                // this may not be the best solution
                // but it prevents crashing and things from being overridden, if a gamepad is not connected
                return new DummyAction(name);
            }


            var buttonSelector = GamePadAction.GamePadButtonAction.GetButtonSelector(buttonName);

            if (buttonSelector != null)
                return new GamePadButtonAction(id, buttonName, buttonSelector);

            var axisSelector = GamePadAction.GamePadAxisAction.GetAxisSelector(buttonName);

            if(axisSelector != null)
                return new GamePadAxisAction(id, buttonName, axisSelector);

            throw new ArgumentException("Gamepad button name unknown.", "name");
        }

        /// <summary>
        /// Returns all actions for a specified gamepad.
        /// </summary>
        /// <param name="pad">The gamepad to get actions for.</param>
        public static IEnumerable<IAction> GetAll(InputManager.GamePadStateContainer pad)
        {
            return GamePadAction.GetAll(pad.Id);
        }

        /// <summary>
        /// Returns all actions for a gamepad id.
        /// </summary>
        /// <param name="id">The gamepad id to get actions for.</param>
        public static IEnumerable<IAction> GetAll(int id)
        {
            return GamePadAxisAction.GetAll(id).Cast<IAction>().Concat(GamePadButtonAction.GetAll(id));
        }

        sealed private class GamePadButtonAction : DigitalAction
        {
            private readonly static Dictionary<string, ButtonSelector> buttonSelectors = new Dictionary<string, ButtonSelector>
            {
                { "a", b => b.Buttons.A },
                { "b", b => b.Buttons.B },
                { "x", b => b.Buttons.X },
                { "y", b => b.Buttons.Y },

                { "start", b => b.Buttons.Start },
                { "back", b => b.Buttons.Back },
                { "bigbutton", b => b.Buttons.BigButton },

                { "leftshoulder", b => b.Buttons.LeftShoulder },
                { "leftstick", b => b.Buttons.LeftStick },
                { "rightshoulder", b => b.Buttons.RightShoulder },
                { "rightstick", b => b.Buttons.RightStick },

                { "dpadright", b => b.DPad.Right },
                { "dpadleft", b => b.DPad.Left },
                { "dpadup", b => b.DPad.Up },
                { "dpaddown", b => b.DPad.Down }
            };

            private readonly InputManager.GamePadStateContainer pad;
            private readonly string buttonName;
            private readonly ButtonSelector buttonSelector;

            public GamePadButtonAction(int id, string buttonName, ButtonSelector buttonSelector)
            {
                this.pad = InputManager.GamePads[id];
                this.buttonName = buttonName;
                this.buttonSelector = buttonSelector;
            }

// ReSharper disable once MemberHidesStaticFromOuterClass
            public static IEnumerable<GamePadButtonAction> GetAll(int id)
            {
                return GamePadButtonAction.buttonSelectors.Select(
                    pair => new GamePadButtonAction(id, pair.Key, pair.Value)
                    );
            }


            public static ButtonSelector GetButtonSelector(string buttonName)
            {
                ButtonSelector selector;

                GamePadButtonAction.buttonSelectors.TryGetValue(buttonName, out selector);

                return selector;
            }

            private bool downNow
            {
                get { return this.buttonSelector(this.pad.CurrentState) == ButtonState.Pressed; }
            }

            private bool downBefore
            {
                get { return this.buttonSelector(this.pad.PreviousState) == ButtonState.Pressed; }
            }

            public override bool Hit
            {
                get { return this.downNow && !this.downBefore; }
            }

            public override bool Active
            {
                get { return this.downNow; }
            }

            public override bool Released
            {
                get { return this.downBefore && !this.downNow; }
            }

            public override string ToString()
            {
                return "gamepad" + this.pad.Id + ":" + this.buttonName;
            }

        }

        sealed private class GamePadAxisAction : IAction
        {
            private readonly static Dictionary<string, AxisSelector> axisSelectors = new Dictionary<string, AxisSelector>
            {
                { "+x", s => s.ThumbSticks.Left.X },
                { "-x", s => -s.ThumbSticks.Left.X },

                { "+y", s => s.ThumbSticks.Left.Y },
                { "-y", s => -s.ThumbSticks.Left.Y },

                { "+z", s => s.ThumbSticks.Right.X },
                { "-z", s => -s.ThumbSticks.Right.X },

                { "+w", s => s.ThumbSticks.Right.Y },
                { "-w", s => -s.ThumbSticks.Right.Y },

                { "triggerleft", s => s.Triggers.Left },
                { "triggerright", s => s.Triggers.Right }
            };

            private readonly InputManager.GamePadStateContainer pad;
            private readonly string axisName;
            private readonly AxisSelector axisSelector;
            private bool digitalDownBefore;
            private bool digitalDown;

            private float digitalValueBefore;
            private float digitalValueNow;

            public GamePadAxisAction(int id, string axisName, AxisSelector axisSelector)
            {
                this.pad = InputManager.GamePads[id];
                this.axisName = axisName;
                this.axisSelector = axisSelector;
            }

// ReSharper disable once MemberHidesStaticFromOuterClass
            public static IEnumerable<GamePadAxisAction> GetAll(int id)
            {
                return GamePadAxisAction.axisSelectors.Select(
                    pair => new GamePadAxisAction(id, pair.Key, pair.Value)
                    );
            }

            public static AxisSelector GetAxisSelector(string axisName)
            {
                AxisSelector selector;

                GamePadAxisAction.axisSelectors.TryGetValue(axisName, out selector);

                return selector;
            }

            private void updateDigital()
            {
                float v = this.axisSelector(this.pad.CurrentState);
                float vb = this.axisSelector(this.pad.PreviousState);

                if (v == this.digitalValueNow && vb == this.digitalValueBefore)
                    return;

                this.digitalValueNow = v;
                this.digitalValueBefore = vb;

                if (v >= Settings.HitValue)
                {
                    this.digitalDownBefore = this.digitalDown && vb >= Settings.HitValue;
                    this.digitalDown = true;
                }
                else if (v <= Settings.ReleaseValue)
                {
                    this.digitalDownBefore = this.digitalDown && vb > Settings.ReleaseValue;
                    this.digitalDown = false;
                }
            }

            private float adjustedAnalog
            {
                get
                {
                    return Settings.AdjustAnalogSignal(this.axisSelector(this.pad.CurrentState));
                }
            }

            public bool Hit
            {
                get
                {
                    this.updateDigital();
                    return this.digitalDown && !this.digitalDownBefore;
                }
            }

            public bool Active
            {
                get
                {
                    this.updateDigital();
                    return this.digitalDown;
                }
            }

            public bool Released
            {
                get
                {
                    this.updateDigital();
                    return this.digitalDownBefore && !this.digitalDown;
                }
            }

            public bool IsAnalog
            {
                get { return true; }
            }

            public float AnalogAmount
            {
                get { return this.adjustedAnalog; }
            }

            public override string ToString()
            {
                return "gamepad" + this.pad.Id + ":" + this.axisName;
            }


            public bool Equals(IAction other)
            {
                return other is GamePadAxisAction && this.IsSameAs(other);
            }

            public override int GetHashCode()
            {
                return this.ToString().GetHashCode();
            }
        }
    }

}
