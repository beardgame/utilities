﻿using System;
using System.Collections.Generic;
using System.Linq;
using Bearded.Utilities.Input.Actions;
using OpenTK.Input;

namespace Bearded.Utilities.Input
{
    partial class InputManager
    {
        public partial struct ActionConstructor
        {
            public KeyboardActions Keyboard => new KeyboardActions(manager);
        }

        public struct KeyboardActions
        {
            private readonly InputManager manager;

            public KeyboardActions(InputManager inputManager)
            {
                manager = inputManager;
            }
            
            public IEnumerable<IAction> All
            {
                get
                {
                    var m = manager;
                    return ((Key[])Enum.GetValues(typeof(Key))).Select(k => new KeyboardAction(m, k));
                }
            }

            public IAction FromKey(Key key) => new KeyboardAction(manager, key);

            public IAction FromString(string value)
                => TryParse(value, out var action)
                    ? action
                    : throw new FormatException($"Keyboard key '{value}' invalid.");

            public bool TryParse(string value, out IAction action)
                => TryParseLowerTrimmedString(value.ToLowerInvariant().Trim(), out action);

            internal bool TryParseLowerTrimmedString(string value, out IAction action)
            {
                action = null;

                if (!value.StartsWith("keyboard:"))
                    return false;

                var keyName = value.Substring(9).Trim();
                
                if (!Enum.TryParse(keyName, false, out Key key))
                    return false;

                if (key == Key.Unknown)
                    return false;

                action = new KeyboardAction(manager, key);
                return true;
            }
        }
    }
}
