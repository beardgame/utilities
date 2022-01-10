using Bearded.Utilities.Linq;
using Bearded.Utilities.Testing;
using Bearded.Utilities.Tests.Generators;
using FluentAssertions;
using FsCheck;
using FsCheck.Xunit;
using System;
using System.Collections.Generic;
using Xunit;

namespace Bearded.Utilities.Tests.Linq;

public sealed class ExtensionsTests
{
    [Fact]
    public void Yield()
    {
        var obj = new object();
        obj.Yield().Should().HaveCount(1)
            .And.Contain(obj);
    }

    [Fact]
    public void AppendOnEmptyReturnsObject()
    {
        var obj = new object();
        Array.Empty<object>().Append(obj).Should().HaveCount(1)
            .And.Contain(obj);
    }

    [Fact]
    public void AppendOnNotEmptyReturnsObjectAsLastElement()
    {
        var obj = new object();
        var array = new[] { new object() };
        array.Append(obj).Should().HaveCount(2)
            .And.ContainInOrder(array[0], obj);
    }

    [Fact]
    public void PrependOnEmptyReturnsObject()
    {
        var obj = new object();
        Array.Empty<object>().Prepend(obj).Should().HaveCount(1)
            .And.Contain(obj);
    }

    [Fact]
    public void PrependOnNotEmptyReturnsObjectAsFirstElement()
    {
        var obj = new object();
        var array = new[] { new object() };
        array.Prepend(obj).Should().HaveCount(2)
            .And.ContainInOrder(obj, array[0]);
    }

    [Theory]
    [InlineData(1, 2, 3, 3)]
    [InlineData(3, 2, 1, 1)]
    public void MaxByFindsGreaterElementBySelector(int value1, int value2, int value3, int expectedId)
    {
        var array = new[] {
            new Entity(1, value1),
            new Entity(2, value2),
            new Entity(3, value3)
        };
        array.MaxBy(e => e.Value).Id.Should().Be(expectedId);
    }

    [Theory]
    [InlineData(1, 2, 3, 1)]
    [InlineData(3, 2, 1, 3)]
    public void MinByFindsSmallerElementBySelector(int value1, int value2, int value3, int expectedId)
    {
        var array = new[] {
            new Entity(1, value1),
            new Entity(2, value2),
            new Entity(3, value3)
        };
        array.MinBy(e => e.Value).Id.Should().Be(expectedId);
    }

    [Fact]
    public void TryGetTransformedValueTransformPresentValue()
    {
        var entity = new Entity(2, 0);
        var dictionary = new Dictionary<int, Entity>()
        {
            {1, entity }
        };
        var result = dictionary.TryGetTransformedValue(1, out var value, e => e.Id * 2);
        result.Should().BeTrue();
        value.Should().Be(4);
    }

    [Fact]
    public void TryGetTransformedValueReturnsDefaultOnMissingValue()
    {
        var entity = new Entity(2, 0);
        var dictionary = new Dictionary<int, Entity>()
        {
            {1, entity }
        };
        var result = dictionary.TryGetTransformedValue(2, out var value, e => e.Id * 2);
        result.Should().BeFalse();
        value.Should().Be(default(int));
    }

    [Fact]
    public void AddRangeAddsPairsToDictionary()
    {
        var entity = new Entity(2, 0);
        var firstDictionary = new Dictionary<int, Entity>()
        {
            {1, entity }
        };
        var secondDictionary = new Dictionary<int, Entity>()
        {
            {2, entity },
            {3, entity }
        };
        firstDictionary.AddRange(secondDictionary);
        firstDictionary.Keys
            .Should().Contain(1)
            .And.Contain(2)
            .And.Contain(3);
    }

    public class AddSorted
    {
        [Theory]
        [InlineData(0, 0)]
        [InlineData(2, 1)]
        [InlineData(4, 2)]
        [InlineData(6, 3)]
        public void AddsInExpectedPosition(int id, int index)
        {
            var entity = new Entity(id, 0);
            var list = new List<Entity>() {
                new Entity(1, 0),
                new Entity(3, 0),
                new Entity(5, 0)
            };
            list.AddSorted(entity, Entity.ById);
            list[index].Should().Be(entity);
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(2, 1)]
        [InlineData(4, 2)]
        [InlineData(6, 3)]
        public void AddsInExpectedPositionUsingDefaultComparer(int id, int index)
        {
            var list = new List<int>() {
                1,3,5
            };
            list.AddSorted(id);
            list[index].Should().Be(id);
        }

        [Fact]
        public void ThrowsOnNullList()
        {
            List<Entity>? list = null;
            Action action = () => list.AddSorted(new Entity(1, 1), Entity.ById);
            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void ThrowsOnNullComparer()
        {
            var list = new List<Entity>();
            Action action = () => list.AddSorted(new Entity(1, 1), null);
            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void OnEmptyListAddsAtStart()
        {
            var entity = new Entity(1, 0);
            var list = new List<Entity>();
            list.AddSorted(entity, Entity.ById);
            list[0].Should().Be(entity);
        }
    }

    public sealed class ValueOrNull
    {
        [Fact]
        public void ReturnsNullOnMissingValue()
        {
            var entity = new Entity(2, 0);
            var dictionary = new Dictionary<int, Entity>()
            {
                {1, entity }
            };
            var value = dictionary.ValueOrNull(2);
            value.Should().BeNull();
        }

        [Fact]
        public void ReturnsValueIfPresent()
        {
            var entity = new Entity(2, 0);
            var dictionary = new Dictionary<int, Entity>()
            {
                {1, entity }
            };
            var value = dictionary.ValueOrNull(1);
            value.Should().Be(entity);
        }
    }

    public sealed class ValueOrDefault
    {
        [Fact]
        public void ReturnsValueIfPresent()
        {
            var dictionary = new Dictionary<int, int>()
            {
                {1, 1 }
            };
            var value = dictionary.ValueOrDefault(1);
            value.Should().Be(1);
        }

        [Fact]
        public void ReturnsDefaultOnMissingValue()
        {
            var dictionary = new Dictionary<int, int>()
            {
                {1, 1 }
            };
            var value = dictionary.ValueOrDefault(2);
            value.Should().Be(0);
        }
    }

    public sealed class RandomElement
    {
        [Property]
        public void ReturnsAnElementFromTheCollection(int seed)
        {
            var list = new List<int> { 1, 2, 3 };
            var result = list.RandomElement(new System.Random(seed));
            list.Should().Contain(result);
        }

        [Fact]
        public void ReturnsAnElementFromTheCollectionWithDefaultRandom()
        {
            var list = new List<int> { 1, 2, 3 };
            var result = list.RandomElement();
            list.Should().Contain(result);
        }

        [Fact]
        public void ThrowsOnSourceNull()
        {
            List<int>? list = null;
            Action action = () => list.RandomElement(new System.Random());
            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void ThrowsOnRandomNull()
        {
            var list = new List<int> { 1, 2, 3 };
            Action action = () => list.RandomElement(null);
            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void ThrowsOnEmptyList()
        {
            var list = new List<int>();
            Action action = () => list.RandomElement(new System.Random());
            action.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void ThrowsOnEmptyEnumerable()
        {
            var list = System.Linq.Enumerable.Select(new List<int>(), a => a);
            Action action = () => list.RandomElement(new System.Random());
            action.Should().Throw<InvalidOperationException>();
        }
    }

    public sealed class RandomSubset
    {

        [Property(Arbitrary = new [] { typeof(IntGenerators.PositiveInt) })]
        public void ReturnsAnElementFromTheCollection(int seed, int length)
        {
            var list = new List<int> { 1, 2, 3 };
            var result = list.RandomSubset(length, new System.Random(seed));
            foreach (var r in result)
                list.Should().Contain(r);
            result.Should().HaveCount(Math.Min(length, list.Count));
        }

        [Fact]
        public void ReturnsAnElementFromTheCollectionWithDefaultRandom()
        {
            var list = new List<int> { 1, 2, 3 };
            var result = list.RandomSubset(1);
            list.Should().Contain(result);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void ReturnsAnEmptyListIfCountIsLessThanOrEqualToZero(int count)
        {
            var list = new List<int> { 1, 2, 3 };
            var result = list.RandomSubset(count);
            result.Should().BeEmpty();
        }

        [Fact]
        public void ThrowsOnSourceNull()
        {
            List<int>? list = null;
            Action action = () => list.RandomSubset(1, new System.Random());
            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void ThrowsOnRandomNull()
        {
            var list = new List<int> { 1, 2, 3 };
            Action action = () => list.RandomSubset(1, null);
            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void ThrowsOnEmptyList()
        {
            var list = new List<int>();
            var result = list.RandomSubset(1, new System.Random());
            result.Should().BeEmpty();
        }

        [Fact]
        public void ThrowsOnEmptyEnumerable()
        {
            var list = System.Linq.Enumerable.Select(new List<int>(), a => a);
            var result = list.RandomSubset(1, new System.Random());
            result.Should().BeEmpty();
        }
    }

    public sealed class Shuffle
    {
        [Fact]
        public void ThrowsOnSourceNull()
        {
            List<int>? list = null;
            Action action = () => list.Shuffle(new System.Random());
            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void ThrowsOnRandomNull()
        {
            var list = new List<int> { 1, 2, 3 };
            Action action = () => list.Shuffle(null);
            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void ShufflesInPlaceList()
        {
            var list = new List<int> { 1, 2, 3 };
            list.Shuffle();
            list.Should()
                .Contain(1)
                .And.Contain(2)
                .And.Contain(3)
                .And.HaveCount(3);
        }


        [Property]
        public void ShufflesInPlaceListWithGivenSeed(int seed)
        {
            var list = new List<int> { 1, 2, 3 };
            list.Shuffle(new System.Random(seed));
            list.Should()
                .HaveCount(3)
                .And.Contain(1)
                .And.Contain(2)
                .And.Contain(3);
        }
    }

    public sealed class Shuffled
    {
        [Fact]
        public void ThrowsOnSourceNull()
        {
            List<int>? list = null;
            Action action = () => list.Shuffled(new System.Random());
            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void ThrowsOnRandomNull()
        {
            var list = new List<int> { 1, 2, 3 };
            Action action = () => list.Shuffled(null);
            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void ShufflesInPlaceList()
        {
            var list = new List<int> { 1, 2, 3 };
            var result = list.Shuffled();
            foreach (var e in list)
            {
                result.Should().Contain(e);
            }
            result.Should().HaveCount(list.Count);
        }
    }

    private class Entity
    {
        public static IComparer<Entity> ById = new IdComparer();

        public Entity(int id, int value)
        {
            Id = id;
            Value = value;
        }

        public int Id { get; set; }
        public int Value { get; set; }

        public class IdComparer : IComparer<Entity>
        {
            public int Compare(Entity? x, Entity? y)
            {
                if (x == null)
                    return -1;
                if (y == null)
                    return 1;
                return x.Id.CompareTo(y.Id);
            }
        }
    }
}
