using System.Collections.Concurrent;
using System.Collections.Generic;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace Bearded.Utilities.Input;

sealed class KeyboardEvents
{
    private readonly ConcurrentQueue<(KeyboardKeyEventArgs, bool)> keyEventsQueue
        = new ConcurrentQueue<(KeyboardKeyEventArgs, bool)>();
    private readonly ConcurrentQueue<char> pressedCharactersQueue = new ConcurrentQueue<char>();

    private readonly List<(KeyboardKeyEventArgs, bool)> keyEvents = new List<(KeyboardKeyEventArgs, bool)>();
    private readonly List<char> pressedCharacters = new List<char>();

    internal IReadOnlyList<(KeyboardKeyEventArgs, bool)> KeyEvents { get; }
    internal IReadOnlyList<char> PressedCharacters { get; }

    internal KeyboardEvents(NativeWindow nativeWindow)
    {
        nativeWindow.KeyDown += onKeyDown;
        nativeWindow.KeyUp += onKeyUp;
        nativeWindow.TextInput += onTextInput;

        KeyEvents = keyEvents.AsReadOnly();
        PressedCharacters = pressedCharacters.AsReadOnly();
    }

    private void onKeyDown(KeyboardKeyEventArgs e)
    {
        keyEventsQueue.Enqueue((e, true));
    }

    private void onKeyUp(KeyboardKeyEventArgs e)
    {
        keyEventsQueue.Enqueue((e, false));
    }

    private void onTextInput(TextInputEventArgs e)
    {
        // FIXME: unicode may be represented by a string of more than one character
        pressedCharactersQueue.Enqueue((char) e.Unicode);
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
