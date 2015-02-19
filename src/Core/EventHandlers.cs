namespace Bearded.Utilities
{
    /// <summary>
    /// Event handler for events without parameters.
    /// </summary>
    public delegate void VoidEventHandler();

    /// <summary>
    /// Generic event handler for events with one parameter.
    /// </summary>
    /// <param name="t">The first event parameter.</param>
    /// <typeparam name="T">Type of the first event parameter.</typeparam>
    public delegate void GenericEventHandler<in T>(T t);

    /// <summary>
    /// Generic event handler for events with two parameters.
    /// </summary>
    /// <param name="t1">The first event parameter.</param>
    /// <param name="t2">The second event parameter.</param>
    /// <typeparam name="T1">Type of the first event parameter.</typeparam>
    /// <typeparam name="T2">Type of the second event parameter.</typeparam>
    public delegate void GenericEventHandler<in T1, in T2>(T1 t1, T2 t2);

    /// <summary>
    /// Generic event handler for events with three parameters.
    /// </summary>
    /// <param name="t1">The first event parameter.</param>
    /// <param name="t2">The second event parameter.</param>
    /// <param name="t3">The third event parameter.</param>
    /// <typeparam name="T1">Type of the first event parameter.</typeparam>
    /// <typeparam name="T2">Type of the second event parameter.</typeparam>
    /// <typeparam name="T3">Type of the third event parameter.</typeparam>
    public delegate void GenericEventHandler<in T1, in T2, in T3>(T1 t1, T2 t2, T3 t3);

    /// <summary>
    /// Generic event handler for events with four parameters.
    /// </summary>
    /// <param name="t1">The first event parameter.</param>
    /// <param name="t2">The second event parameter.</param>
    /// <param name="t3">The third event parameter.</param>
    /// <param name="t4">The fourth event parameter.</param>
    /// <typeparam name="T1">Type of the first event parameter.</typeparam>
    /// <typeparam name="T2">Type of the second event parameter.</typeparam>
    /// <typeparam name="T3">Type of the third event parameter.</typeparam>
    /// <typeparam name="T4">Type of the fourth event parameter.</typeparam>
    public delegate void GenericEventHandler<in T1, in T2, in T3, in T4>(T1 t1, T2 t2, T3 t3, T4 t4);
}