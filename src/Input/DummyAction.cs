
namespace Bearded.Utilities.Input
{
    /// <summary>
    /// An implementation of IAction with no functionality.
    /// This is used mainly for unbound actions, and for gamepad actions that were deserialised for a non connected gamepad, to not lose the name of the control.
    /// </summary>
    sealed class DummyAction : IAction
    {
        private readonly string name;

        public DummyAction(string name)
        {
            this.name = name;
        }

        public bool Hit
        {
            get { return false; }
        }

        public bool Active
        {
            get { return false; }
        }

        public bool Released
        {
            get { return false; }
        }

        public bool IsAnalog
        {
            get { return false; }
        }

        public float AnalogAmount
        {
            get { return 0; }
        }

        public override string ToString()
        {
            return this.name;
        }

        public bool Equals(IAction other)
        {
            return this.IsSameAs(other);
        }

        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }
    }
}
