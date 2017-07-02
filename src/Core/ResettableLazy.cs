﻿using System;

namespace Bearded.Utilities
{
    public static class ResettableLazy
    {
        public static ResettableLazy<T> From<T>(Func<T> factory)
        {
            return new ResettableLazy<T>(factory);
        }
    }

    /// <summary>
    /// This class represents a lazily initialised value. It can be reset to call the initialisation again afterwards.
    /// It is not thread safe.
    /// </summary>
    public sealed class ResettableLazy<T>
    {
        private readonly Func<T> factory;

        private bool hasValue;
        private T value;
        
        public T Value => ensureValue();
        
        public ResettableLazy(Func<T> factory)
        {
            this.factory = factory;
        }

        public void Reset()
        {
            hasValue = false;
        }

        private T ensureValue()
        {
            if (!hasValue)
            {
                value = factory();
                hasValue = true;
            }
            return value;
        }
    }
}
