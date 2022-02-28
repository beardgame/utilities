using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Bearded.Utilities.Algorithms;

/// <summary>
/// This class contains the functionality to efficiently pack a list of rectangles into one larger rectangle.
/// It tries to pack them tightly and minimizes wasted space in the containing rectangle.
/// The result is not guaranteed to be optimal.
///
/// The algorithm implemented is an almost 1:1 translation of the Binary Tree Bin Packing Algorithm by
/// Jake Gordon: http://codeincomplete.com/posts/2011/5/7/bin_packing/
/// </summary>
public static partial class BinPacking
{
    /// <summary>
    /// Packs a list of rectangles together trying to minimize the size of the containing rectangle.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="items"/> is null.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="sizeSelector"/> is null.</exception>
    public static Result<T> Pack<T>(IEnumerable<T> items, Func<T, Size> sizeSelector)
    {
        return validateArgumentsAndPack(items, sizeSelector, defaultHeuristic);
    }

    /// <summary>
    /// Packs a list of rectangles together trying to minimize the size of the containing rectangle.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="rectangles"/> is null.</exception>
    public static Result<T> Pack<T>(IEnumerable<Rectangle<T>> rectangles)
    {
        return validateArgumentsAndPack(rectangles, defaultHeuristic);
    }

    /// <summary>
    /// Packs a list of rectangles together trying to minimize the size of the containing rectangle.
    /// Unlike Pack(), this method tries several heuristics and returns the result with the densest packing.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="items"/> is null.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="sizeSelector"/> is null.</exception>
    public static Result<T> PackWithMultipleHeuristics<T>(IEnumerable<T> items, Func<T, Size> sizeSelector)
    {
        return validateArgumentsAndPack(items, sizeSelector, defaultHeuristic);
    }

    /// <summary>
    /// Packs a list of rectangles together trying to minimize the size of the containing rectangle.
    /// Unlike Pack(), this method tries several heuristics and returns the result with the densest packing.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="rectangles"/> is null.</exception>
    public static Result<T> PackWithMultipleHeuristics<T>(IEnumerable<Rectangle<T>> rectangles)
    {
        return validateArgumentsAndPack(rectangles, allHeuristics);
    }

    private static ICollection<Comparison<IRectangle>> defaultHeuristic { get; }
        = new List<Comparison<IRectangle>>
        {
            (r1, r2) => (r2.Width * r2.Height).CompareTo(r1.Width * r1.Height)
        }.AsReadOnly();
    private static ICollection<Comparison<IRectangle>> allHeuristics { get; }
        = new List<Comparison<IRectangle>>
        {
            (r1, r2) => (r2.Width * r2.Height).CompareTo(r1.Width * r1.Height),
            (r1, r2) => r2.Width.CompareTo(r1.Width),
            (r1, r2) => r2.Height.CompareTo(r1.Height),
        }.AsReadOnly();

    private static Result<T> validateArgumentsAndPack<T>(
        IEnumerable<T> items, Func<T, Size> sizeSelector, ICollection<Comparison<IRectangle>> heuristics)
    {
        if (items == null)
            throw new ArgumentNullException(nameof(items));
        if (sizeSelector == null)
            throw new ArgumentNullException(nameof(sizeSelector));
        if (heuristics == null)
            throw new ArgumentNullException(nameof(heuristics));
        if (heuristics.Count == 0)
            throw new ArgumentException("Must provide at least one heuristic.", nameof(heuristics));

        var rectangleList = items.Select(i =>
        {
            var size = sizeSelector(i);
            return new Rectangle<T>(i, size.Width, size.Height);
        }).ToList();

        return packImplementation(rectangleList, heuristics);
    }

    private static Result<T> validateArgumentsAndPack<T>(
        IEnumerable<Rectangle<T>> rectangles, ICollection<Comparison<IRectangle>> heuristics)
    {
        if (rectangles == null)
            throw new ArgumentNullException(nameof(rectangles));
        if (heuristics == null)
            throw new ArgumentNullException(nameof(heuristics));
        if (heuristics.Count == 0)
            throw new ArgumentException("Must provide at least one heuristic.", nameof(heuristics));

        var rectangleList = rectangles.ToList();

        return packImplementation(rectangleList, heuristics);
    }

    private static Result<T> packImplementation<T>(
        List<Rectangle<T>> rectangles, IEnumerable<Comparison<IRectangle>> heuristics)
    {
        if (rectangles.Count == 0)
            return Result<T>.Empty;

        Result<T> bestResult = default!;

        foreach (var heuristic in heuristics)
        {
            rectangles.Sort(heuristic);
            var result = new Tree<T>(rectangles).Fit();

            if (bestResult == null || bestResult.Filled < result.Filled)
                bestResult = result;
        }

        return bestResult;
    }
}
