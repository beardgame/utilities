namespace Bearded.Utilities;

public delegate void VoidEventHandler();

public delegate void GenericEventHandler<in T>(T t);
public delegate void GenericEventHandler<in T1, in T2>(T1 t1, T2 t2);
public delegate void GenericEventHandler<in T1, in T2, in T3>(T1 t1, T2 t2, T3 t3);
public delegate void GenericEventHandler<in T1, in T2, in T3, in T4>(T1 t1, T2 t2, T3 t3, T4 t4);