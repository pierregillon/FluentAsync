using System;

namespace FluentAsync.Functional 
{
    public static class Extensions
    {
        public static Func<T1, TResult> Compose<T1, T2, TResult>(this Func<T2, TResult> func1, Func<T1, T2> func2) 
            => x => func1(func2(x));

        public static Func<T1, TResult> Then<T1, T2, TResult>(this Func<T1, T2> func1, Func<T2, TResult> func2) 
            => func2.Compose(func1);

        public static Func<T1, TResult> Curry<T1, T2, TResult>(this Func<T1, T2, TResult> func1, T2 element) 
            => x => func1(x, element);

        public static Func<TResult> Curry<T1, TResult>(this Func<T1, TResult> func1, T1 element) 
            => () => func1(element);
    }
}