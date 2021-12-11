// using System;
//
// namespace Bearded.Utilities.Input.Actions
// {
//     using ButtonSelector = Func<GamePadState, ButtonState>;
//     using AxisSelector = Func<GamePadState, float>;
//
//     sealed class GamePadButtonAction : DigitalAction
//     {
//         private readonly GamePadStateManager pad;
//         private readonly string buttonName;
//         private readonly ButtonSelector buttonSelector;
//
//         public GamePadButtonAction(InputManager inputManager, int id, string buttonName, ButtonSelector buttonSelector)
//         {
//             pad = inputManager.GamePads[id];
//             this.buttonName = buttonName;
//             this.buttonSelector = buttonSelector;
//         }
//
//         private bool downNow => buttonSelector(pad.State.Current) == ButtonState.Pressed;
//         private bool downBefore => buttonSelector(pad.State.Previous) == ButtonState.Pressed;
//         public override bool Hit => downNow && !downBefore;
//         public override bool Active => downNow;
//         public override bool Released => downBefore && !downNow;
//
//         public override string ToString() => "gamepad" + pad.Id + ":" + buttonName;
//     }
//
//     sealed class GamePadAxisAction : IAction
//     {
//         private readonly GamePadStateManager pad;
//         private readonly string axisName;
//         private readonly AxisSelector axisSelector;
//         private bool digitalDownBefore;
//         private bool digitalDown;
//
//         private float digitalValueBefore;
//         private float digitalValueNow;
//
//         private float currentValue => axisSelector(pad.State.Current);
//         private float previousValue => axisSelector(pad.State.Previous);
//         private float adjustedAnalog => GamePadAction.Settings.AdjustAnalogSignal(currentValue);
//
//         public GamePadAxisAction(InputManager inputManager, int id, string axisName, AxisSelector axisSelector)
//         {
//             pad = inputManager.GamePads[id];
//             this.axisName = axisName;
//             this.axisSelector = axisSelector;
//         }
//
//         private void updateDigital()
//         {
//             var v = currentValue;
//             var vb = previousValue;
//
//             // ReSharper disable CompareOfFloatsByEqualityOperator
//             if (v == digitalValueNow && vb == digitalValueBefore)
//                 return;
//             // ReSharper enable CompareOfFloatsByEqualityOperator
//
//             digitalValueNow = v;
//             digitalValueBefore = vb;
//
//             if (v >= GamePadAction.Settings.HitValue)
//             {
//                 digitalDownBefore = digitalDown && vb >= GamePadAction.Settings.HitValue;
//                 digitalDown = true;
//             }
//             else if (v <= GamePadAction.Settings.ReleaseValue)
//             {
//                 digitalDownBefore = digitalDown && vb > GamePadAction.Settings.ReleaseValue;
//                 digitalDown = false;
//             }
//         }
//
//         public bool Hit
//         {
//             get
//             {
//                 updateDigital();
//                 return digitalDown && !digitalDownBefore;
//             }
//         }
//
//         public bool Active
//         {
//             get
//             {
//                 updateDigital();
//                 return digitalDown;
//             }
//         }
//
//         public bool Released
//         {
//             get
//             {
//                 updateDigital();
//                 return digitalDownBefore && !digitalDown;
//             }
//         }
//
//         public bool IsAnalog => true;
//         public float AnalogAmount => adjustedAnalog;
//
//         public override string ToString() => "gamepad" + pad.Id + ":" + axisName;
//
//         public override bool Equals(object obj) => Equals(obj as IAction);
//         public bool Equals(IAction other) => other is GamePadAxisAction && this.IsSameAs(other);
//         public override int GetHashCode() => ToString().GetHashCode();
//     }
//
//     static class GamePadAction
//     {
//         public static class Settings
//         {
//             private static float hitValue = 0.6f;
//             private static float releaseValue = 0.4f;
//             private static float analogDeadZone = 0.05f;
//             private static float analogMaxValue = 0.95f;
//             private static float analogPower = 1;
//
//             public static float HitValue
//             {
//                 get => hitValue;
//                 set => hitValue = value.Clamped(0.1f, 0.9f);
//             }
//
//             public static float ReleaseValue
//             {
//                 get => releaseValue;
//                 set => releaseValue = value.Clamped(0.1f, 0.9f);
//             }
//
//             public static float AnalogDeadZone
//             {
//                 get => analogDeadZone;
//                 set => analogDeadZone = value.Clamped(0, 0.9f);
//             }
//
//             public static float AnalogMaxValue
//             {
//                 get => analogMaxValue;
//                 set => analogMaxValue = value.Clamped(0.1f, 1);
//             }
//
//             public static float AnalogPower
//             {
//                 get => analogPower;
//                 set => analogPower = Math.Max(0.01f, value);
//             }
//
//             internal static float AnalogDeadToMaxRange => AnalogMaxValue - AnalogDeadZone;
//
//             internal static float AdjustAnalogSignal(float input)
//             {
//                 if (input < AnalogDeadZone)
//                     return 0;
//                 if (input > AnalogMaxValue)
//                     return 1;
//                 return (float)Math.Pow(
//                     (input - AnalogDeadZone) / AnalogDeadToMaxRange,
//                     analogPower);
//             }
//         }
//     }
// }

