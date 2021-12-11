using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Bearded.Utilities.Collections;
using Bearded.Utilities.Linq;
using FluentAssertions;
using FluentAssertions.Equivalency;
using FsCheck;
using FsCheck.Xunit;
using Xunit;
// ReSharper disable AssignmentIsFullyDiscarded

namespace Bearded.Utilities.Tests.Collections;

public class DeletableObjectListTests
{
    private static readonly Func<EquivalencyAssertionOptions<TestDeletable>, EquivalencyAssertionOptions<TestDeletable>>
        withExactSameItems = o => o.WithStrictOrdering().ComparingByValue<TestDeletable>();

    public static TheoryData<int> PositiveCounts => new TheoryData<int> {1, 2, 3, 4, 5, 7, 10, 13, 37, 42, 1337};

    private static List<TestDeletable> getDeletables(int itemsToAdd)
        => Enumerable.Range(0, itemsToAdd).Select(i => new TestDeletable()).ToList();

    private static (DeletableObjectList<TestDeletable> List, List<TestDeletable> Items)
        createPopulatedList(int itemsToAdd)
    {
        var list = new DeletableObjectList<TestDeletable>();
        var items = getDeletables(itemsToAdd);
        foreach (var item in items)
            list.Add(item);
        return (list, items);
    }

    public class TestDeletable : IDeletable
    {
        public bool Deleted { get; set; }
    }

    public class TheParameterlessConstructor
    {
        [Fact]
        public void CreatesEmptyList()
        {
            var list = new DeletableObjectList<IDeletable>();

            list.Should().BeEmpty();
        }
    }

    public class TheIntParameterConstructor
    {
        [Fact]
        public void CreatesEmptyListForZeroValue()
        {
            var list = new DeletableObjectList<IDeletable>(0);

            list.Should().BeEmpty();
        }

        [Theory]
        [MemberData(nameof(PositiveCounts), MemberType = typeof(DeletableObjectListTests))]
        public void CreatesEmptyListForPositiveValues(int count)
        {
            var list = new DeletableObjectList<IDeletable>(count);

            list.Should().BeEmpty();
        }

        [Theory]
        [MemberData(nameof(PositiveCounts), MemberType = typeof(DeletableObjectListTests))]
        public void ThrowsForNegativeValues(int count)
        {
            Action createListWithNegativeValue = () => _ = new DeletableObjectList<IDeletable>(-count);

            createListWithNegativeValue.Should().Throw<ArgumentOutOfRangeException>();
        }
    }

    public abstract class MethodThatDoesNotThrowsWhenEnumeratingTests
    {
        protected abstract void CallMethod(DeletableObjectList<TestDeletable> list);

        public static TheoryData<int> IndicesFromZeroToNineteen
        {
            get
            {
                var data = new TheoryData<int>();
                foreach (var i in Enumerable.Range(0, 20))
                {
                    data.Add(i);
                }
                return data;
            }
        }

        [Theory]
        [MemberData(nameof(IndicesFromZeroToNineteen))]
        public void DoesNotThrowWhenCallingDuringEnumeration(int index)
        {
            var (list, _) = createPopulatedList(20);

            foreach (var i in list.Select((item, i) => i))
            {
                if (i == index)
                    CallMethod(list);
            }
        }
    }

    public class TheAddMethod : MethodThatDoesNotThrowsWhenEnumeratingTests
    {
        protected override void CallMethod(DeletableObjectList<TestDeletable> list)
            => list.Add(new TestDeletable());

        [Fact]
        public void AddsObjectToList()
        {
            var list = new DeletableObjectList<TestDeletable>();
            var deletable = new TestDeletable();

            list.Add(deletable);

            list.Should().ContainSingle().Which.Should().Be(deletable);
        }

        [Fact]
        public void ThrowsOnNull()
        {
            var list = new DeletableObjectList<TestDeletable>();

            Action addingNull = () => list.Add(null);

            addingNull.Should().Throw<ArgumentNullException>();
        }

        [Theory]
        [MemberData(nameof(PositiveCounts), MemberType = typeof(DeletableObjectListTests))]
        public void CanAddMultipleItems(int itemsToAdd)
        {
            var list = new DeletableObjectList<TestDeletable>();
            var items = getDeletables(itemsToAdd);

            foreach (var item in items)
                list.Add(item);

            list.Should().BeEquivalentTo(items, withExactSameItems);
        }

        [Fact]
        public void EnumeratesElementsAddedDuringEnumeration()
        {
            var (list, items) = createPopulatedList(1);
            var addItems = 3;

            foreach (var _ in list)
            {
                if (addItems > 0)
                {
                    var newItem = new TestDeletable();
                    items.Add(newItem);
                    list.Add(newItem);
                    addItems--;
                }
            }

            list.Should().HaveCount(4).And.BeEquivalentTo(items, withExactSameItems);
        }
    }

    public class TheRemoveMethod : MethodThatDoesNotThrowsWhenEnumeratingTests
    {
        private readonly System.Random random = new System.Random(0);

        protected override void CallMethod(DeletableObjectList<TestDeletable> list)
            => list.Remove(list.RandomElement(random));

        [Fact]
        public void ReturnsFalseOnANewList()
        {
            var list = new DeletableObjectList<TestDeletable>();

            var returnValue = list.Remove(new TestDeletable());

            returnValue.Should().BeFalse();
        }

        [Fact]
        public void ReturnsFalseForUnknownElement()
        {
            var (list, _) = createPopulatedList(1);

            var returnValue = list.Remove(new TestDeletable());

            returnValue.Should().BeFalse();
        }

        [Fact]
        public void ReturnsFalseForNull()
        {
            var (list, _) = createPopulatedList(1);

            var returnValue = list.Remove(null);

            returnValue.Should().BeFalse();
        }

        [Fact]
        public void ReturnsTrueForKnownItem()
        {
            var (list, items) = createPopulatedList(1);

            var returnValue = list.Remove(items[0]);

            returnValue.Should().BeTrue();
        }

        [Fact]
        public void ReturnsFalseForRepeatedCall()
        {
            var (list, items) = createPopulatedList(1);

            list.Remove(items[0]);
            var returnValue = list.Remove(items[0]);

            returnValue.Should().BeFalse();
        }

        [Fact]
        public void ReturnsFalseForClearedList()
        {
            var (list, items) = createPopulatedList(1);
            list.Clear();

            var returnValue = list.Remove(items[0]);

            returnValue.Should().BeFalse();
        }

        [Theory]
        [MemberData(nameof(PositiveCounts), MemberType = typeof(DeletableObjectListTests))]
        public void CanRemoveMultipleItems(int itemsToAdd)
        {
            var (list, items) = createPopulatedList(itemsToAdd);

            foreach (var item in items)
            {
                var returnValue = list.Remove(item);
                returnValue.Should().BeTrue();
            }

            list.Should().BeEmpty();
        }

        [Fact]
        public void DoesNotInfluenceEnumerationIfRemovedItemsHaveAlreadyBeenEnumerated()
        {
            var (list, items) = createPopulatedList(20);
            var enumeratedItems = new List<TestDeletable>();

            foreach (var item in list)
            {
                enumeratedItems.Add(item);
                list.Remove(item);
            }

            enumeratedItems.Should().BeEquivalentTo(items, withExactSameItems);
        }

        [Fact]
        public void PreventsRemovedItemsFromBeingEnumeratedIfRemovedItemsHaveNotBeenEnumeratedYet()
        {
            var (list, items) = createPopulatedList(20);
            var enumeratedItems = new List<TestDeletable>();

            foreach (var item in list)
            {
                var toRemove = list.Last();
                enumeratedItems.Add(item);
                list.Remove(toRemove);
            }

            enumeratedItems.Should().BeEquivalentTo(items.Take(10), withExactSameItems);
        }
    }

    public class TheClearMethod : MethodThatDoesNotThrowsWhenEnumeratingTests
    {
        protected override void CallMethod(DeletableObjectList<TestDeletable> list)
            => list.Clear();

        [Fact]
        public void DoesNotThrowForEmptyList()
        {
            var list = new DeletableObjectList<TestDeletable>();

            list.Clear();
        }

        [Theory]
        [MemberData(nameof(PositiveCounts), MemberType = typeof(DeletableObjectListTests))]
        public void ClearsAllItems(int itemsToAdd)
        {
            var (list, _) = createPopulatedList(itemsToAdd);

            list.Clear();

            list.Should().BeEmpty();
        }
    }

    public abstract class NonMutatingMethodThatThrowsWhenEnumeratingTests
    {
        protected abstract void CallMethod(DeletableObjectList<TestDeletable> list);

        [Fact]
        public void DoesNotThrowForEmptyList()
        {
            var list = new DeletableObjectList<TestDeletable>();

            CallMethod(list);
        }

        [Theory]
        [MemberData(nameof(PositiveCounts), MemberType = typeof(DeletableObjectListTests))]
        public void DoesNotRemoveItems(int itemsToAdd)
        {
            var (list, items) = createPopulatedList(itemsToAdd);

            CallMethod(list);

            list.Should().BeEquivalentTo(items, withExactSameItems);
        }

        [Fact]
        public void ThrowsIfEnumerating()
        {
            var list = new DeletableObjectList<TestDeletable>();
            _ = list.GetEnumerator();

            Action callingWhileEnumerating = () => CallMethod(list);

            callingWhileEnumerating.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void DoesNotThrowAfterDisposingEnumerator()
        {
            var list = new DeletableObjectList<TestDeletable>();
            var enumerator = list.GetEnumerator();
            enumerator.Dispose();

            CallMethod(list);
        }

        [Fact]
        public void ThrowsIfAtLeastOneEnumeratorIsNotDisposed()
        {
            var list = new DeletableObjectList<TestDeletable>();
            var enumerators = Enumerable.Range(0, 20).Select(i => list.GetEnumerator()).ToList();
            var arbitraryEnumerator = enumerators[7];

            foreach (var enumerator in enumerators.Except(new [] { arbitraryEnumerator }))
            {
                enumerator.Dispose();
            }

            Action callingWhileEnumerating = () => CallMethod(list);

            callingWhileEnumerating.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void DoesNotThrowIfAllEnumeratorsAreDisposed()
        {
            var list = new DeletableObjectList<TestDeletable>();
            var enumerators = Enumerable.Range(0, 20).Select(i => list.GetEnumerator()).ToList();

            foreach (var enumerator in enumerators)
            {
                enumerator.Dispose();
            }

            CallMethod(list);
        }
    }

    public class TheTrimExcessMethod : NonMutatingMethodThatThrowsWhenEnumeratingTests
    {
        protected override void CallMethod(DeletableObjectList<TestDeletable> list) => list.TrimExcess();
    }

    public class TheForceCompactMethod : NonMutatingMethodThatThrowsWhenEnumeratingTests
    {
        protected override void CallMethod(DeletableObjectList<TestDeletable> list) => list.ForceCompact();
    }

    public class Enumeration
    {
        [Property]
        public void SkipsDeletedItems(NonEmptyArray<bool> someBools)
        {
            var boolQueue = someBools.Get.Looping();
            var (list, items) = createPopulatedList(20);

            foreach (var item in list)
            {
                if (boolQueue.Next())
                    item.Deleted = true;
            }

            list.Should().BeEquivalentTo(items.Where(i => !i.Deleted), withExactSameItems);
        }

        [Fact]
        public void SkipsItemsDeletedAheadOfEnumeration()
        {
            var (list, items) = createPopulatedList(20);
            var enumeratedItems = new List<TestDeletable>();

            foreach (var item in list)
            {
                enumeratedItems.Add(item);
                list.Last().Deleted = true;
            }

            enumeratedItems.Should().BeEquivalentTo(items.Take(10), withExactSameItems);
            list.Should().BeEquivalentTo(items.Take(10), withExactSameItems);
        }

        [Fact]
        public void IsNotAffectedByItemsBeingDeletedAfterEnumeratingThem()
        {
            var (list, items) = createPopulatedList(20);
            var enumeratedItems = new List<TestDeletable>();

            foreach (var item in list)
            {
                enumeratedItems.Add(item);
                list.First().Deleted = true;
            }

            enumeratedItems.Should().BeEquivalentTo(items, withExactSameItems);
            list.Should().BeEmpty();
        }

        [Fact]
        public void CanBeResetTakingIntoAccountDeletedAndRemovedItems()
        {
            var (list, items) = createPopulatedList(20);
            var enumeratedItems = new List<TestDeletable>();

            // ReSharper disable once GenericEnumeratorNotDisposed
            var enumerator = list.GetEnumerator();
            enumerator.MoveNext();
            enumerator.MoveNext();
            enumerator.MoveNext();

            list.Remove(items[0]);
            items[1].Deleted = true;

            enumerator.Reset();

            while (enumerator.MoveNext())
            {
                enumeratedItems.Add(enumerator.Current);
            }

            enumeratedItems.Should().BeEquivalentTo(items.Skip(2), withExactSameItems);
        }

        [Fact]
        public void WorksNonGenerically()
        {
            var (list, items) = createPopulatedList(20);
            var nonGenericList = (IEnumerable) list;

            var enumeratedItems = nonGenericList.Cast<TestDeletable>().ToList();

            enumeratedItems.Should().BeEquivalentTo(items, withExactSameItems);
        }
    }

    public class TheApproximateCountProperty
    {
        [Fact]
        public void IsZeroForEmptyList()
        {
            var list = new DeletableObjectList<TestDeletable>();

            list.ApproximateCount.Should().Be(0);
        }

        [Fact]
        public void IsZeroForClearedList()
        {
            var (list, _) = createPopulatedList(10);

            list.Clear();

            list.ApproximateCount.Should().Be(0);
        }

        [Property]
        public void IsAccurateWhenAddingAndRemoving(PositiveInt itemsToAdd, int randomSeed)
        {
            var itemCount = itemsToAdd.Get;
            var random = new System.Random(randomSeed);
            var list = new DeletableObjectList<TestDeletable>();

            foreach (var i in Enumerable.Range(1, itemCount))
            {
                list.Add(new TestDeletable());
                list.ApproximateCount.Should().Be(i);
            }

            foreach (var i in Enumerable.Range(1, itemCount))
            {
                list.Remove(list.RandomElement(random));
                list.ApproximateCount.Should().Be(itemCount - i);
            }
        }

        [Property]
        public void IsAccurateAfterEnumerating(PositiveInt itemsToAdd, int randomSeed)
        {
            var itemCount = itemsToAdd.Get;
            var random = new System.Random(randomSeed);
            var (list, items) = createPopulatedList(itemCount);
            items.RandomElement(random).Deleted = true;

            _ = list.Count();

            list.ApproximateCount.Should().Be(itemCount - 1);
        }
    }
}
