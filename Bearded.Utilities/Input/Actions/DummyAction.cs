namespace Bearded.Utilities.Input.Actions;

sealed class DummyAction : IAction
{
    private readonly string name;

    public DummyAction(string name)
    {
        this.name = name;
    }

    public bool Hit => false;
    public bool Active => false;
    public bool Released => false;
    public bool IsAnalog => false;
    public float AnalogAmount => 0;

    public override string ToString() => name;

    public override bool Equals(object? obj) => Equals(obj as IAction);
    public bool Equals(IAction other) => other is DummyAction && this.IsSameAs(other);
    public override int GetHashCode() => ToString().GetHashCode();
}
