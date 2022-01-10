using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Bearded.Utilities.Input.Actions;

sealed class KeyboardAction : DigitalAction
{
    private readonly InputManager inputManager;
    private readonly Keys key;

    public KeyboardAction(InputManager inputManager, Keys key)
    {
        this.inputManager = inputManager;
        this.key = key;
    }

    public override bool Hit => inputManager.IsKeyHit(key);
    public override bool Active => inputManager.IsKeyPressed(key);
    public override bool Released => inputManager.IsKeyReleased(key);

    public override string ToString() => "keyboard:" + key;
}
