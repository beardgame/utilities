using System;

namespace Bearded.Utilities.Input
{
    /// <summary>
    /// Represents a simple generic input source, like a keyboard key or a gamepad stick axis.
    /// </summary>
    public interface IAction : IEquatable<IAction>
    {
        /// <summary>
        /// Whether the input was hit (or clicked) this frame.
        /// </summary>
        bool Hit { get; }
        /// <summary>
        /// Whether the input is being held down.
        /// </summary>
        bool Active { get; }
        /// <summary>
        /// Whether the input was released this frame.
        /// </summary>
        bool Released { get; }

        /// <summary>
        /// Whether the input can supply an analog value.
        /// </summary>
        bool IsAnalog { get; }
        /// <summary>
        /// The analog value of the input.
        /// If no analog value is available, this is equal to Active ? 1 : 0.
        /// </summary>
        float AnalogAmount { get; }
    }
}
