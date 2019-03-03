using System;

namespace Bearded.Utilities.Tests.Random
{
    public class BaseTests
    {
        public abstract class RandomMethodWithoutParameters<T>
            where T : struct, IComparable<T>
        {
            protected Func<T> CallingWith(int seed)
                => () => ResultOfCallingWith(seed);

            protected T ResultOfCallingWith(int seed)
            {
                Seed(seed);
                return CallMethod();
            }

            protected abstract T CallMethod();

            protected abstract void Seed(int seed);
        }

        public abstract class RandomMethodWithOneParameter<T>
            : RandomMethodWithOneParameter<T, T>
            where T : struct, IComparable<T>
        {
            
        }
        
        public abstract class RandomMethodWithOneParameter<T, TParameter>
            where T : struct, IComparable<T>
        {
            protected Func<T> CallingWith(int seed, TParameter parameter)
                => () => ResultOfCallingWith(seed, parameter);

            protected T ResultOfCallingWith(int seed, TParameter parameter)
            {
                Seed(seed);
                return CallMethod(parameter);
            }

            protected abstract T CallMethod(TParameter parameter);

            protected abstract void Seed(int seed);
        }

        public abstract class RandomMethodWithTwoParameters<T>
            where T : struct, IComparable<T>
        {
            protected Func<T> CallingWith(int seed, T parameter1, T parameter2)
                => () => ResultOfCallingWith(seed, parameter1, parameter2);

            protected T ResultOfCallingWith(int seed, T parameter1, T parameter2)
            {
                Seed(seed);
                return CallMethod(parameter1, parameter2);
            }

            protected abstract T CallMethod(T parameter1, T parameter2);

            protected abstract void Seed(int seed);
        }
    }
}
