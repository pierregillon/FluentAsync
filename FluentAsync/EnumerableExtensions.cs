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

        /// <summary>
        /// Create an asynchronous enumerable from an input sequence and a asynchronous predicate function.
        /// </summary>
        /// <returns>An <see cref="IAsyncEnumerable{T}"/> that will contains elements from the input sequence that satisfy the condition </returns>
        public static IAsyncEnumerable<T> WhereAsync<T>(this IEnumerable<T> enumerable, Func<T, Task<bool>> predicate) 
            => enumerable.ToAsyncEnumerable().WhereAsync(predicate);

        /// <summary>
        /// Create an asynchronous enumerable from an input sequence.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <returns></returns>
#pragma warning disable 1998
        public static async IAsyncEnumerable<T> ToAsyncEnumerable<T>(this IEnumerable<T> enumerable)
#pragma warning restore 1998
        {
            foreach (var element in enumerable) {
                yield return element;
            }
        }
    }
}