using System;
using System.Diagnostics;
using System.Threading.Tasks;
using FluentAsync.CovariantTask;

namespace FluentAsync
{
    [DebuggerStepThrough]
    public static class FunctionalProgramingExtensions
    {
        /// <summary>
        /// Apply a projection function on the current element.
        /// </summary>
        /// <returns>The result of the function called with the caller.</returns>
        public static TResult Pipe<T, TResult>(this T element, Func<T, TResult> projection) => projection(element);

        /// <summary>
        /// Apply a projection function on a task result.
        /// </summary>
        /// <returns>The result of the function called with the caller.</returns>
        public static async Task<TResult> PipeAsync<T, TResult>(this Task<T> element, Func<T, TResult> projection) => projection(await element);

        /// <summary>
        /// Apply an asynchronous function on a task result.
        /// </summary>
        /// <returns>The result of the function called with the caller.</returns>
        public static async Task<TResult> PipeAsync<T, TResult>(this Task<T> element, Func<T, Task<TResult>> action) => await action(await element);

        /// <summary>
        /// Apply a projection function on a task result.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="element"></param>
        /// <param name="projection"></param>
        /// <returns></returns>
        public static ITask<TResult> PipeAsync<T, TResult>(this ITask<T> element, Func<T, TResult> projection)
            => element.Pipe(async x => projection(await element)).ChainWith();

        /// <summary>
        /// Apply an asynchronous function on a task result.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="element"></param>
        /// <param name="projection"></param>
        /// <returns></returns>
        public static ITask<TResult> PipeAsync<T, TResult>(this ITask<T> element, Func<T, Task<TResult>> projection)
            => element.Pipe(async x => await projection(await element)).ChainWith();

        public static T IfDefault<T>(this T element, T defaultValue) 
            => !Equals(element, default(T)) ? element : defaultValue;

        public static T IfNull<T>(this T element, T defaultValue) where T : class
            => element ?? defaultValue;

        public static T IfMatch<T>(this T element, Func<T, bool> isMatching, Func<T, T> execute)
            => Match(element, isMatching, execute, x => x);

        public static T IfMatch<T>(this T element, T matchValue, Func<T, T> execute)
            => IfMatch(element, x => Equals(x, matchValue), execute);

        public static TResult Match<T, TResult>(this T element, Func<T, bool> isMatching, Func<T, TResult> whenMatch, Func<T, TResult> whenNotMatch)
            => isMatching(element) ? whenMatch(element) : whenNotMatch(element);

    }
}