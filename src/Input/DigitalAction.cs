
namespace Bearded.Utilities.Input
{
    /// <summary>
    /// An abstraction for digital (non analog) actions like keyboard keys and gamepad buttons.
    /// </summary>
    abstract class DigitalAction : IAction
    {
        public abstract bool Hit { get; }
        public abstract bool Active { get; }
        public abstract bool Released { get; }
        public bool IsAnalog { get { return false; } }

        public float AnalogAmount { get { return this.Active ? 1 : 0; } }

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
