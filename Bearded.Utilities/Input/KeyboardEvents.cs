using System.Collections.Concurrent;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Input;

namespace Bearded.Utilities.Input
{
    sealed class KeyboardEvents
    {
        private readonly ConcurrentQueue<(KeyboardKeyEventArgs, bool)> keyEventsQueue
            = new ConcurrentQueue<(KeyboardKeyEventArgs, bool)>();
        private readonly ConcurrentQueue<char> pressedCharactersQueue = new ConcurrentQueue<char>();

        private readonly List<(KeyboardKeyEventArgs, bool)> keyEvents = new List<(KeyboardKeyEventArgs, bool)>();
        private readonly List<char> pressedCharacters = new List<char>();
        
        internal IReadOnlyList<(KeyboardKeyEventArgs, bool)> KeyEvents { get; }
        internal IReadOnlyList<char> PressedCharacters { get; }

        internal KeyboardEvents(INativeWindow nativeWindow)
        {
            nativeWindow.KeyDown += onKeyDown;
            nativeWindow.KeyUp += onKeyUp;
            nativeWindow.KeyPress += onKeyPress;

            KeyEvents = keyEvents.AsReadOnly();
            PressedCharacters = pressedCharacters.AsReadOnly();
        }

        private void onKeyDown(object sender, KeyboardKeyEventArgs e)
        {
            keyEventsQueue.Enqueue((new KeyboardKeyEventArgs(e), true));
        }

        private void onKeyUp(object sender, KeyboardKeyEventArgs e)
        {
            keyEventsQueue.Enqueue((new KeyboardKeyEventArgs(e), false));
        }

        private void onKeyPress(object sender, KeyPressEventArgs e)
        {
            pressedCharactersQueue.Enqueue(e.KeyChar);
        }

        internal void Update()
        {
            updateCollectionFromConcurrentQueue(keyEvents, keyEventsQueue);
            updateCollectionFromConcurrentQueue(pressedCharacters, pressedCharactersQueue);
        }

        private static void updateCollectionFromConcurrentQueue<T>(ICollection<T> list, ConcurrentQueue<T> queue)
        {
            list.Clear();
            while (queue.TryDequeue(out var result))
                list.Add(result);
        }
    }
}
