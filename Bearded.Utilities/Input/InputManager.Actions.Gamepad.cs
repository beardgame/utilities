// using System;
// using System.Collections.Generic;
// using System.Linq;
// using Bearded.Utilities.Input.Actions;
//
// namespace Bearded.Utilities.Input
// {
//     using ButtonSelector = Func<GamePadState, ButtonState>;
//     using AxisSelector = Func<GamePadState, float>;
//
//     partial class InputManager
//     {
//         public readonly partial struct ActionConstructor
//         {
//             public GamepadsActions Gamepad => new GamepadsActions(manager);
//         }
//
//         public readonly struct GamepadsActions
//         {
//             private readonly InputManager manager;
//
//             public GamepadsActions(InputManager inputManager)
//             {
//                 manager = inputManager;
//             }
//
//             public GamepadActions WithId(int id) => new GamepadActions(manager, id);
//
//             public IAction FromString(string value)
//                 => TryParse(value, out var action)
//                     ? action
//                     : throw new FormatException($"Gamepad button '{value}' invalid.");
//
//             public bool TryParse(string value, out IAction action)
//                 => TryParseLowerTrimmedString(value.ToLowerInvariant().Trim(), out action);
//
//             internal bool TryParseLowerTrimmedString(string value, out IAction action)
//             {
//                 action = null;
//
//                 if (!value.StartsWith("gamepad"))
//                     return false;
//
//                 var split = value.Substring(7).Split(':');
//
//                 if (split.Length != 2)
//                     return false;
//
//                 var idString = split[0].Trim();
//
//                 if (!int.TryParse(idString, out var id))
//                     return false;
//                 if (id < 0)
//                     return false;
//
//                 var buttonName = split[1].Trim();
//
//                 return WithId(id).TryParseButtonName(buttonName, out action);
//             }
//         }
//
//         public readonly struct GamepadActions
//         {
//             private readonly InputManager manager;
//             private readonly int padId;
//
//             public GamepadActions(InputManager inputManager, int id)
//             {
//                 manager = inputManager;
//                 padId = id;
//             }
//
//             public GamepadButtonActions Buttons => new GamepadButtonActions(manager, padId);
//             public GamepadAxisActions Axes => new GamepadAxisActions(manager, padId);
//
//             public bool IsConnected
//                 => padId >= 0 && padId < manager.GamePads.Count
//                    && manager.GamePads[padId].State.Current.IsConnected;
//
//             public IEnumerable<IAction> All => Buttons.All.Concat(Axes.All);
//
//             public IAction FromButtonName(string value)
//                 => TryParseButtonName(value, out var action)
//                     ? action
//                     : throw new FormatException($"Gamepad button '{value}' unknown.");
//
//             public bool TryParseButtonName(string value, out IAction action)
//             {
//                 if (!IsConnected)
//                 {
//                     // this may not be the best solution
//                     // but it prevents crashing and things from being overridden, if a gamepad is not connected
//                     action = new DummyAction(value);
//                     return true;
//                 }
//
//                 return Buttons.TryParseLowerTrimmedString(value, out action)
//                     || Axes.TryParseLowerTrimmedString(value, out action);
//             }
//         }
//
//         public readonly struct GamepadButtonActions
//         {
//             private readonly InputManager manager;
//             private readonly int padId;
//
//             public GamepadButtonActions(InputManager inputManager, int id)
//             {
//                 manager = inputManager;
//                 padId = id;
//             }
//
//             public IAction A => button("a");
//             public IAction B => button("b");
//             public IAction X => button("x");
//             public IAction Y => button("y");
//
//             public IAction Start => button("start");
//             public IAction Back => button("back");
//             public IAction Bigbutton => button("bigbutton");
//
//             public IAction LeftShoulder => button("leftshoulder");
//             public IAction LeftStick => button("leftstick");
//             public IAction RightShoulder => button("rightshoulder");
//             public IAction RightStick => button("rightstick");
//
//             public IAction DpadRight => button("dpadright");
//             public IAction DpadLeft => button("dpadleft");
//             public IAction DpadUp => button("dpadup");
//             public IAction DpadDown => button("dpaddown");
//
//             public IEnumerable<IAction> All
//             {
//                 get
//                 {
//                     var inputManager = manager;
//                     var id = padId;
//                     return selectors.Select(
//                         pair => new GamePadButtonAction(inputManager, id, pair.Key, pair.Value)
//                     );
//                 }
//             }
//
//             public IAction FromName(string buttonName)
//                 => TryParse(buttonName, out var action)
//                     ? action
//                     : throw new FormatException($"Gamepad button '{buttonName}' unknown.");
//
//             public bool TryParse(string buttonName, out IAction action)
//                 => TryParseLowerTrimmedString(buttonName.ToLowerInvariant().Trim(), out action);
//
//             internal bool TryParseLowerTrimmedString(string buttonName, out IAction action)
//             {
//                 action = null;
//
//                 if (!selectors.TryGetValue(buttonName, out var selector))
//                     return false;
//
//                 action = new GamePadButtonAction(manager, padId, buttonName, selector);
//                 return true;
//             }
//
//             private IAction button(string name)
//                 => new GamePadButtonAction(manager, padId, name, selectors[name]);
//
//             private static readonly Dictionary<string, ButtonSelector> selectors =
//                 new Dictionary<string, ButtonSelector>
//                 {
//                     {"a", b => b.Buttons.A},
//                     {"b", b => b.Buttons.B},
//                     {"x", b => b.Buttons.X},
//                     {"y", b => b.Buttons.Y},
//
//                     {"start", b => b.Buttons.Start},
//                     {"back", b => b.Buttons.Back},
//                     {"bigbutton", b => b.Buttons.BigButton},
//
//                     {"leftshoulder", b => b.Buttons.LeftShoulder},
//                     {"leftstick", b => b.Buttons.LeftStick},
//                     {"rightshoulder", b => b.Buttons.RightShoulder},
//                     {"rightstick", b => b.Buttons.RightStick},
//
//                     {"dpadright", b => b.DPad.Right},
//                     {"dpadleft", b => b.DPad.Left},
//                     {"dpadup", b => b.DPad.Up},
//                     {"dpaddown", b => b.DPad.Down}
//                 };
//         }
//
//         public readonly struct GamepadAxisActions
//         {
//             private readonly InputManager manager;
//             private readonly int padId;
//
//             public GamepadAxisActions(InputManager inputManager, int id)
//             {
//                 manager = inputManager;
//                 padId = id;
//             }
//
//             public IAction XPositive => axis("+x");
//             public IAction XNegative => axis("-x");
//
//             public IAction YPositive => axis("+y");
//             public IAction YNegative => axis("-y");
//
//             public IAction ZPositive => axis("+z");
//             public IAction ZNegative => axis("-z");
//
//             public IAction WPositive => axis("+w");
//             public IAction WNegative => axis("-w");
//
//             public IAction TriggerLeft => axis("triggerleft");
//             public IAction TriggerRight => axis("triggerright");
//
//             public IEnumerable<IAction> All
//             {
//                 get
//                 {
//                     var inputManager = manager;
//                     var id = padId;
//                     return selectors.Select(
//                         pair => new GamePadAxisAction(inputManager, id, pair.Key, pair.Value)
//                     );
//                 }
//             }
//
//             public IAction FromName(string axisName)
//                 => TryParse(axisName, out var action)
//                     ? action
//                     : throw new FormatException($"Gamepad axis '{axisName}' unknown.");
//
//             public bool TryParse(string axisName, out IAction action)
//                 => TryParseLowerTrimmedString(axisName.ToLowerInvariant().Trim(), out action);
//
//             internal bool TryParseLowerTrimmedString(string axisName, out IAction action)
//             {
//                 action = null;
//
//                 if (!selectors.TryGetValue(axisName, out var selector))
//                     return false;
//
//                 action = new GamePadAxisAction(manager, padId, axisName, selector);
//                 return true;
//             }
//
//             private IAction axis(string name)
//                 => new GamePadAxisAction(manager, padId, name, selectors[name]);
//
//             private static readonly Dictionary<string, AxisSelector> selectors =
//                 new Dictionary<string, AxisSelector>
//                 {
//                     {"+x", s => s.ThumbSticks.Left.X},
//                     {"-x", s => -s.ThumbSticks.Left.X},
//
//                     {"+y", s => s.ThumbSticks.Left.Y},
//                     {"-y", s => -s.ThumbSticks.Left.Y},
//
//                     {"+z", s => s.ThumbSticks.Right.X},
//                     {"-z", s => -s.ThumbSticks.Right.X},
//
//                     {"+w", s => s.ThumbSticks.Right.Y},
//                     {"-w", s => -s.ThumbSticks.Right.Y},
//
//                     {"triggerleft", s => s.Triggers.Left},
//                     {"triggerright", s => s.Triggers.Right}
//                 };
//         }
//     }
// }

