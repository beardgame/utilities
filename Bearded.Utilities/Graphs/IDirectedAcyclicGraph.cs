using System;

namespace Bearded.Utilities.Graphs;

public interface IDirectedAcyclicGraph<T> : IDirectedGraph<T> where T : IEquatable<T> { }
