namespace Bearded.Utilities.Tilemaps
{
    /// <summary>
    /// Represents a step taken in a Tilemap from one tile to another.
    /// </summary>
    struct Step
    {
        public readonly sbyte X;
        public readonly sbyte Y;

        public Step(sbyte x, sbyte y)
        {
            this.X = x;
            this.Y = y;
        }
    }
}