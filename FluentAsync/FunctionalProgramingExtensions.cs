using System;
using System.Diagnostics;
using System.Threading.Tasks;

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
    }
}