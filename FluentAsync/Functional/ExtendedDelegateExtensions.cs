using System;

namespace FluentAsync.Functional {
    public static class ExtendedDelegateExtensions
    {
        public static ExtendedDelegate<T1, T2, TResult> Ext<T1, T2, TResult>(this Func<T1, T2, TResult> func1)
            => new ExtendedDelegate<T1, T2, TResult>(func1);

        public static ExtendedDelegate<T1, TResult> Ext<T1, TResult>(this Func<T1, TResult> func1)
            => new ExtendedDelegate<T1, TResult>(func1);

        public static ExtendedDelegate<TResult> Ext<TResult>(this Func<TResult> func1)
            => new ExtendedDelegate<TResult>(func1);
    }
}