using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace FluentAsync
{
    [DebuggerStepThrough]
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Create an asynchronous enumerable from an input sequence and a asynchronous projection function.
        /// </summary>
        /// <returns>An <see cref="IAsyncEnumerable{T}"/> that will evaluates sequentially the projection function for each element of the input sequence. </returns>
        public static async IAsyncEnumerable<TResult> SelectAsync<T, TResult>(this IEnumerable<T> enumerable, Func<T, Task<TResult>> asyncProjection)
        {
            foreach (var element in enumerable) {
                yield return await asyncProjection(element);
            }
        }
    }
}