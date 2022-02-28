using Bearded.Utilities.Tests.Generators;
using Bearded.Utilities.Tilemaps.Rectangular;
using FluentAssertions;
using FsCheck;
using FsCheck.Xunit;
using System;
using System.Linq;
using Xunit;

namespace Bearded.Utilities.Tests.Tilemaps;

public class TileTests
{
    [Fact]
    public void CantCreateWithNullTilemap()
    {
        Action action = () => new Tile<int>(null, 1, 1);
        action.Should()
            .Throw<ArgumentNullException>()
            .WithMessage("*tilemap*");
    }

    [Fact]
    public void GetsValueFromTilemap()
    {
        var tilemap = new Tilemap<int>(1, 1);
        var value = 1;
        tilemap[0, 0] = value;
        var tile = new Tile<int>(tilemap, 0, 0);
        tile.Value.Should().Be(value);
    }

    [Property(Arbitrary = new[] { typeof(IntGenerators.PositiveInt) })]
    public void IsValidIsTheSameAsIsValidTileOnTilemap(int width, int length, int x, int y)
    {
        Tilemap<int> tilemap = new Tilemap<int>(width, length);
        var tile = new Tile<int>(tilemap, x, y);
        tile.IsValid.Should().Be(tilemap.IsValidTile(x, y));
    }

    [Fact]
    public void EmptyTileIsNotValid()
    {
        var tile = new Tile<int>();
        tile.IsValid.Should().BeFalse();
    }

    [Fact]
    public void SingleTileHasNoValidNeighbours()
    {
        var tile = new Tile<int>(new Tilemap<int>(1, 1), 0, 0);
        tile.ValidNeighbours.Should().BeEmpty();
    }

    [Fact]
    public void TilesOnSameCoordinatesAreEquale()
    {
        Tilemap<int> tilemap = new Tilemap<int>(1, 1);
        var tile = new Tile<int>(tilemap, 0, 0);
        var tile2 = new Tile<int>(tilemap, 0, 0);
        (tile == tile2).Should().BeTrue();
    }
    [Fact]
    public void NullTileIsDifferent()
    {
        Tilemap<int> tilemap = new Tilemap<int>(1, 1);
        var tile = new Tile<int>(tilemap, 0, 0);
        Tile<int>? tile2 = null;
        (tile != tile2).Should().BeTrue();
    }

    [Theory]
    [InlineData(0, 0, 1, 3, 4)]
    [InlineData(0, 1, 4, 5, 2, 0, 3)]
    [InlineData(0, 2, 5, 1, 4)]
    [InlineData(1, 0, 6, 7, 4, 1, 0)]
    [InlineData(1, 1, 7, 8, 5, 2, 1, 0, 3, 6)]
    [InlineData(1, 2, 8, 2, 1, 4, 7)]
    [InlineData(2, 0, 7, 4, 3)]
    [InlineData(2, 1, 8, 5, 4, 3, 6)]
    [InlineData(2, 2, 5, 4, 7)]
    public void ValidNeighboursAreAsExpected(int x, int y, params int[] values)
    {
        var tilemap = GetInitializedTilemap();
        var tile = new Tile<int>(tilemap, x, y);
        var list = tile.ValidNeighbours
            .Select(v => v.Value).ToList();
        foreach (var v in values)
        {
            list.Should().Contain(v);
        }
        list
            .Should().HaveCount(values.Count());
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(0, 1)]
    [InlineData(0, 2)]
    [InlineData(1, 0)]
    [InlineData(1, 1)]
    [InlineData(1, 2)]
    [InlineData(2, 0)]
    [InlineData(2, 1)]
    [InlineData(2, 2)]
    public void PossibleNeighboursContainTileAsNeighbour(int x, int y)
    {
        var tilemap = new Tilemap<int>(3, 3);
        var tile = new Tile<int>(tilemap, x, y);
        foreach (var neighbour in tile.PossibleNeighbours)
        {
            neighbour.PossibleNeighbours.Contains(tile);
        }
        tile.PossibleNeighbours.Should().HaveCount(8);
    }

    [Theory]
    [InlineData(0, 2, Direction.UpLeft)]
    [InlineData(1, 2, Direction.Up)]
    [InlineData(2, 2, Direction.UpRight)]
    [InlineData(0, 1, Direction.Left)]
    [InlineData(2, 1, Direction.Right)]
    [InlineData(0, 0, Direction.DownLeft)]
    [InlineData(1, 0, Direction.Down)]
    [InlineData(2, 0, Direction.DownRight)]
    public void NeighbourReturnsExpectedTile(int x, int y, Direction direction)
    {
        var tilemap = new Tilemap<int>(3, 3);
        var tile = new Tile<int>(tilemap, 1, 1);
        var neighbour = tile.Neighbour(direction);
        neighbour.X.Should().Be(x);
        neighbour.Y.Should().Be(y);
    }

    private static Tilemap<int> GetInitializedTilemap()
    {
        var tilemap = new Tilemap<int>(3, 3);
        var i = 0;
        for (int x = 0; x < tilemap.Width; x++)
        for (int y = 0; y < tilemap.Height; y++)
            tilemap[x, y] = i++;
        return tilemap;
    }
}
