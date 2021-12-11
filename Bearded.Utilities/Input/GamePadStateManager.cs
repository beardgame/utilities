namespace Bearded.Utilities.Input;

public class GamePadStateManager
{
    public void ProcessEventsAsync() {}
    public void Update(bool windowIsActive) {}

    // public int Id { get; }
    //
    // public AsyncAtomicUpdating<GamePadState> State { get; } = new AsyncAtomicUpdating<GamePadState>();
    //
    // public static GamePadStateManager ForId(int id) => new GamePadStateManager(id);
    //
    // private GamePadStateManager(int id)
    // {
    //     Id = id;
    // }
    //
    // public void ProcessEventsAsync() => State.SetLastKnownState(GamePad.GetState(Id));
    //
    // public void Update(bool windowIsActive)
    // {
    //     if (windowIsActive)
    //     {
    //         State.Update();
    //     }
    //     else
    //     {
    //         State.UpdateToDefault();
    //     }
    // }
}
