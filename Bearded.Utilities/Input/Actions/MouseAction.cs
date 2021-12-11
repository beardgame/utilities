using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Bearded.Utilities.Input.Actions;

sealed class MouseButtonAction : DigitalAction
{
    private readonly InputManager inputManager;
    private readonly MouseButton button;

    public MouseButtonAction(InputManager inputManager, MouseButton button)
    {
        this.inputManager = inputManager;
        this.button = button;
    }

    public override bool Hit => inputManager.IsMouseButtonHit(button);
    public override bool Active => inputManager.IsMouseButtonPressed(button);
    public override bool Released => inputManager.IsMouseButtonReleased(button);

    public override string ToString() => "mouse:" + button;
}
