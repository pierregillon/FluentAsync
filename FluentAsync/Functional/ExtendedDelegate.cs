using System;

namespace FluentAsync.Functional
{
    public class ExtendedDelegate<T1, T2, TResult>
    {
        private readonly Func<T1, T2, TResult> wrappedDelegate;

        public ExtendedDelegate(Func<T1, T2, TResult> del) => wrappedDelegate = del;

        public TResult Exec(T1 value1, T2 value2) => wrappedDelegate(value1, value2);

        public static ExtendedDelegate<T1, TResult> operator >(ExtendedDelegate<T1, T2, TResult> del, T2 value)
            => del.wrappedDelegate.Curry(value).Ext();

        public static ExtendedDelegate<T1, TResult> operator <(ExtendedDelegate<T1, T2, TResult> del, T2 value) => throw new NotImplementedException();


        public static implicit operator Func<T1, T2, TResult>(ExtendedDelegate<T1, T2, TResult> del) => del.wrappedDelegate;
        public static implicit operator ExtendedDelegate<T1, T2, TResult>(Func<T1, T2, TResult> del) => del.Ext();
    }


    public class ExtendedDelegate<T1, TResult>
    {
        private readonly Func<T1, TResult> wrappedDelegate;

        public ExtendedDelegate(Func<T1, TResult> del) => wrappedDelegate = del;

        public TResult Exec(T1 value1) => wrappedDelegate(value1);


        public static ExtendedDelegate<TResult> operator >(ExtendedDelegate<T1, TResult> del, T1 value) => del.wrappedDelegate.Curry(value).Ext();

        public static ExtendedDelegate<TResult> operator <(ExtendedDelegate<T1, TResult> del, T1 value) => throw new NotImplementedException();

        public static implicit operator Func<T1, TResult>(ExtendedDelegate<T1, TResult> del) => del.wrappedDelegate;
        public static implicit operator ExtendedDelegate<T1, TResult>(Func<T1, TResult> del) => del.Ext();
    }

    public class ExtendedDelegate<TResult>
    {
        private readonly Func<TResult> wrappedDelegate;

        public ExtendedDelegate(Func<TResult> del) => wrappedDelegate = del;

        public TResult Exec() => wrappedDelegate();
    }
}