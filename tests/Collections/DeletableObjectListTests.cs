using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Bearded.Utilities.Collections;
using Bearded.Utilities.Linq;
using FsCheck;
using FsCheck.Xunit;
using Xunit;
using Random = System.Random;

namespace Bearded.Utilities.Tests.Collections
{
    public class DeletableObjectListTests
    {
        public static IEnumerable<object[]> PositiveCounts =>
            new [] {1, 2, 3, 4, 5, 7, 10, 13, 37, 42, 1337}
                .Select(i => new object[] { i });
        
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
                
                Assert.Empty(list);
            }
        }
        
        public class TheIntParameterConstructor
        {
            [Fact]
            public void CreatesEmptyListForZeroValue()
            {
                var list = new DeletableObjectList<IDeletable>(0);
                
                Assert.Empty(list);
            }
            
            [Theory]
            [MemberData(nameof(PositiveCounts), MemberType = typeof(DeletableObjectListTests))]
            public void CreatesEmptyListForPositiveValues(int count)
            {
                var list = new DeletableObjectList<IDeletable>(count);
                
                Assert.Empty(list);
            }
            
            [Theory]
            [MemberData(nameof(PositiveCounts), MemberType = typeof(DeletableObjectListTests))]
            public void ThrowsForNegativeValues(int count)
            {
                Action createListWithNegativeValue = () => new DeletableObjectList<IDeletable>(-count);

                Assert.Throws<ArgumentOutOfRangeException>(createListWithNegativeValue);
            }
        }
        
        public abstract class MethodThatDoesNotThrowsWhenEnumeratingTests
        {
            protected abstract void CallMethod(DeletableObjectList<TestDeletable> list);

            public static IEnumerable<object[]> IndicesFromZeroToNineteen =
                Enumerable.Range(0, 20).Select(i => new object[] {i});

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
                
                Assert.Equal(deletable, list.Single());
            }

            [Fact]
            public void ThrowsOnNull()
            {
                var list = new DeletableObjectList<TestDeletable>();

                Action addingNull = () => list.Add(null);

                Assert.Throws<ArgumentNullException>(addingNull);
            }

            [Theory]
            [MemberData(nameof(PositiveCounts), MemberType = typeof(DeletableObjectListTests))]
            public void CanAddMultipleItems(int itemsToAdd)
            {
                var list = new DeletableObjectList<TestDeletable>();
                var items = getDeletables(itemsToAdd);

                foreach (var item in items)
                    list.Add(item);
                
                Assert.True(items.SequenceEqual(list));
            }

            [Fact]
            public void EnumeratesElementsAddedDuringEnumeration()
            {
                var (list, items) = createPopulatedList(1);
                var addItems = 3;

                foreach (var item in list)
                {
                    if (addItems > 0)
                    {
                        var newItem = new TestDeletable();
                        items.Add(newItem);
                        list.Add(newItem);
                        addItems--;
                    }
                }
                
                Assert.Equal(4, list.Count());
                Assert.True(items.SequenceEqual(list));
            }
        }

        public class TheRemoveMethod : MethodThatDoesNotThrowsWhenEnumeratingTests
        {
            private readonly Random random = new Random(0);

            protected override void CallMethod(DeletableObjectList<TestDeletable> list)
                => list.Remove(list.RandomElement(random));
            
            [Fact]
            public void ReturnsFalseOnANewList()
            {
                var list = new DeletableObjectList<TestDeletable>();

                var returnValue = list.Remove(new TestDeletable());
                
                Assert.False(returnValue);
            }
            
            [Fact]
            public void ReturnsFalseForUnknownElement()
            {
                var (list, _) = createPopulatedList(1);

                var returnValue = list.Remove(new TestDeletable());
                
                Assert.False(returnValue);
            }
            
            [Fact]
            public void ReturnsFalseForNull()
            {
                var (list, _) = createPopulatedList(1);

                var returnValue = list.Remove(null);
                
                Assert.False(returnValue);
            }
            
            [Fact]
            public void ReturnsTrueForKnownItem()
            {
                var (list, items) = createPopulatedList(1);

                var returnValue = list.Remove(items[0]);
                
                Assert.True(returnValue);
            }
            
            [Fact]
            public void ReturnsFalseForRepeatedCall()
            {
                var (list, items) = createPopulatedList(1);
               
                list.Remove(items[0]);
                var returnValue = list.Remove(items[0]);
                
                Assert.False(returnValue);
            }
            
            [Fact]
            public void ReturnsFalseForClearedList()
            {
                var (list, items) = createPopulatedList(1);
                list.Clear();
               
                var returnValue = list.Remove(items[0]);
                
                Assert.False(returnValue);
            }
            
            [Theory]
            [MemberData(nameof(PositiveCounts), MemberType = typeof(DeletableObjectListTests))]
            public void CanRemoveMultipleItems(int itemsToAdd)
            {
                var (list, items) = createPopulatedList(itemsToAdd);

                foreach (var item in items)
                {
                    var returnValue = list.Remove(item);
                    Assert.True(returnValue);
                }
                
                Assert.Empty(list);
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
                
                Assert.True(items.SequenceEqual(enumeratedItems));
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
                
                Assert.True(items.Take(10).SequenceEqual(enumeratedItems));
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
                
                Assert.Empty(list);
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
                
                Assert.True(items.SequenceEqual(list));
            }

            [Fact]
            public void ThrowsIfEnumerating()
            {
                var list = new DeletableObjectList<TestDeletable>();
                list.GetEnumerator();
                
                Action callingWhileEnumerating = () => CallMethod(list);

                Assert.Throws<InvalidOperationException>(callingWhileEnumerating);
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

                Assert.Throws<InvalidOperationException>(callingWhileEnumerating);
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
                
                Assert.True(items.Where(i => !i.Deleted).SequenceEqual(list));
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
                
                Assert.True(items.Take(10).SequenceEqual(enumeratedItems));
                Assert.True(items.Take(10).SequenceEqual(list));
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
                
                Assert.True(items.SequenceEqual(enumeratedItems));
                Assert.Empty(list);
            }

            [Fact]
            public void CanBeResetTakingIntoAccountDeletedAndRemovedItems()
            {
                var (list, items) = createPopulatedList(20);
                var enumeratedItems = new List<TestDeletable>();

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
                
                Assert.True(items.Skip(2).SequenceEqual(enumeratedItems));
            }

            [Fact]
            public void WorksNonGenerically()
            {
                var (list, items) = createPopulatedList(20);
                var enumeratedItems = new List<TestDeletable>();
                var nonGenericList = (IEnumerable) list;

                foreach (var item in nonGenericList)
                {
                    enumeratedItems.Add((TestDeletable) item);
                }
                
                Assert.True(items.SequenceEqual(enumeratedItems));
            }
        }

        public class TheApproximateCountProperty
        {
            [Fact]
            public void IsZeroForEmptyList()
            {
                var list = new DeletableObjectList<TestDeletable>();
                
                Assert.Equal(0, list.ApproximateCount);
            }
            
            [Fact]
            public void IsZeroForClearedList()
            {
                var (list, _) = createPopulatedList(10);
                
                list.Clear();
                
                Assert.Equal(0, list.ApproximateCount);
            }
            
            [Property]
            public void IsAccurateWhenAddingAndRemoving(PositiveInt itemsToAdd, int randomSeed)
            {
                var itemCount = itemsToAdd.Get;
                var random = new Random(randomSeed);
                var list = new DeletableObjectList<TestDeletable>();

                foreach (var i in Enumerable.Range(1, itemCount))
                {
                    list.Add(new TestDeletable());
                    Assert.Equal(i, list.ApproximateCount);
                }
                
                foreach (var i in Enumerable.Range(1, itemCount))
                {
                    list.Remove(list.RandomElement(random));
                    Assert.Equal(itemCount - i, list.ApproximateCount);
                }
            }
            
            [Property]
            public void IsAccurateAfterEnumerating(PositiveInt itemsToAdd, int randomSeed)
            {
                var itemCount = itemsToAdd.Get;
                var random = new Random(randomSeed);
                var (list, items) = createPopulatedList(itemCount);
                items.RandomElement(random).Deleted = true;

                _ = list.Count();
                
                Assert.Equal(itemCount - 1, list.ApproximateCount);
            }
        }
    }
}
