using System;
using System.Collections.Generic;

namespace Bearded.Utilities.Graphs;

public interface IDirectedGraph<T> where T : IEquatable<T>
{
    IEnumerable<T> Elements { get; }
    int Count { get; }
    IEnumerable<T> GetDirectSuccessorsOf(T element);
    IEnumerable<T> GetDirectPredecessorsOf(T element);
}
