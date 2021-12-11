using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Bearded.Utilities.Tilemaps.Rectangular;
using FluentAssertions;
using Xunit;

namespace Bearded.Utilities.Tests.Tilemaps;

public class RectangularTilemapTests
{
    #region Tilemap

    [Fact]
    public void TestTilemapDimensions()
    {
        var tilemap = new Tilemap<int>(42, 1337);

        Assert.Equal(42, tilemap.Width);
        Assert.Equal(1337, tilemap.Height);
        Assert.Equal(42 * 1337, tilemap.Count);
    }

    [Fact]
    public void TestTilemapEnumeration()
    {
        var tilemap = new Tilemap<int>(42, 1337);

        var tiles = tilemap.ToList();

        Assert.Equal(42 * 1337, tiles.Count);

        var tilesByHash = new HashSet<Tile<int>>();

        foreach (var tile in tiles)
        {
            Assert.True(tilesByHash.Add(tile));
        }
    }
    [Fact]
    public void TestTilemapEnumerationAsEnumerable()
    {
        var tilemap = (IEnumerable)new Tilemap<int>(42, 1337);


        var tilesByHash = new HashSet<object?>();

        foreach (var tile in tilemap)
        {
            Assert.True(tilesByHash.Add(tile));
        }
    }

    [Fact]
    public void TestTilemapTileValidity()
    {
        var tilemap = new Tilemap<int>(42, 1337);

        Assert.True(tilemap.IsValidTile(0, 0));
        Assert.True(tilemap.IsValidTile(13, 42));
        Assert.True(tilemap.IsValidTile(41, 1336));

        Assert.False(tilemap.IsValidTile(0, 1337));
        Assert.False(tilemap.IsValidTile(42, 0));

        foreach (var tile in tilemap)
        {
            Assert.True(tilemap.IsValidTile(tile));
        }
    }

    [Fact]
    public void TestTilemapIndexAccess()
    {
        var tilemap = new Tilemap<int>(10,20);

        for (int y = 0; y < tilemap.Height; y++)
        {
            for (int x = 0; x < tilemap.Width; x++)
            {
                var expected = 1 + x * y;
                tilemap[x, y] = expected;
                tilemap[x, y].Should().Be(expected);
            }
        }
    }

    [Fact]
    public void TestTilemapIndexAccessByTile()
    {
        var tilemap = new Tilemap<int>(10, 20);

        for (int y = 0; y < tilemap.Height; y++)
        {
            for (int x = 0; x < tilemap.Width; x++)
            {
                var expected = 1 + x * y;
                var tile = new Tile<int>(tilemap,x, y);
                tilemap[tile] = expected;
                tilemap[tile].Should().Be(expected);
            }
        }
    }

    #endregion
}
