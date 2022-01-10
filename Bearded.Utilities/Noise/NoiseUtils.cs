using System;

namespace Bearded.Utilities.Noise;

static class NoiseUtils
{
    internal static T[,] GenerateRandomArray<T>(int width, int height, Func<Random, T> generator, Random? random)
    {
        random ??= new Random();

        var array = new T[width, height];

        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                array[x, y] = generator(random);
            }
        }

        return array;
    }
}
