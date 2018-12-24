using System.Collections.Generic;
using System.Linq;
using Bearded.Utilities.Tilemaps.Rectangular;
using Xunit;

namespace Bearded.Utilities.Tests.Tilemaps
{
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

        #endregion
    }
}