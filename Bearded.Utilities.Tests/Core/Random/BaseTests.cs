using System;

namespace Bearded.Utilities.Tests.Random;

public class BaseTests
{
    public abstract class RandomMethodBase<TMethod>
    {
        // ReSharper disable once VirtualMemberCallInConstructor
        // it's magic to make actual tests more concise, yet enforce implementing setup
        // (though we can't enforce _correct_ implementation)
        protected RandomMethodBase() => Setup();

        protected abstract void Setup();

        protected void Setup(Action<int> seed, TMethod method)
        {
            Seed = seed;
            CallMethod = method;
        }
            
        public TMethod CallMethod { get; set; }

        public Action<int> Seed { get; set; }
    }
        
    public abstract class RandomMethodWithoutParameters<T> : RandomMethodBase<Func<T>>
        where T : struct, IComparable<T>
    {
        protected Func<T> CallingWith(int seed)
            => () => ResultOfCallingWith(seed);

        protected T ResultOfCallingWith(int seed)
        {
            Seed(seed);
            return CallMethod();
        }
    }

    public abstract class RandomMethodWithOneParameter<T>
        : RandomMethodWithOneParameter<T, T>
        where T : struct, IComparable<T>
    {
            
    }
        
    public abstract class RandomMethodWithOneParameter<T, TParameter> : RandomMethodBase<Func<TParameter, T>>
        where T : struct, IComparable<T>
    {
        protected Func<T> CallingWith(int seed, TParameter parameter)
            => () => ResultOfCallingWith(seed, parameter);

        protected T ResultOfCallingWith(int seed, TParameter parameter)
        {
            Seed(seed);
            return CallMethod(parameter);
        }
    }

    public abstract class RandomMethodWithTwoParameters<T> : RandomMethodBase<Func<T, T, T>>
        where T : struct, IComparable<T>
    {
        protected Func<T> CallingWith(int seed, T parameter1, T parameter2)
            => () => ResultOfCallingWith(seed, parameter1, parameter2);

        protected T ResultOfCallingWith(int seed, T parameter1, T parameter2)
        {
            Seed(seed);
            return CallMethod(parameter1, parameter2);
        }
    }
}
