using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace FluentAsync
{
    [DebuggerStepThrough]
    public static class AsyncEnumerableExtensions
    {
        /// <summary>
        /// Enumerate all the elements of the async enumerable and returns a collection result.
        /// Warning : be aware that if the async enumerable is an infinite loop enumeration, this method will block the current thread for ever.
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="enumerable"></param>
        /// <returns></returns>
        public static async Task<IReadOnlyCollection<TResult>> EnumerateAsync<TResult>(this IAsyncEnumerable<TResult> enumerable)
        {
            var results = new List<TResult>();
            await foreach (var element in enumerable) {
                results.Add(element);
            }

            return results;
        }

        /// <summary>
        /// Asynchronously filters an asynchronous sequence of value based on a predicate.
        /// </summary>
        /// <returns>A <see cref="IAsyncEnumerable{T}"/> that will contains elements from the input sequence that satisfy the condition.</returns>
        public static async IAsyncEnumerable<T> WhereAsync<T>(this IAsyncEnumerable<T> enumerable, Func<T, bool> predicate)
        {
            await foreach (var element in enumerable) {
                if (predicate(element)) {
                    yield return element;
                }
            }
        }

        /// <summary>
        /// Asynchronously filters an asynchronous sequence of value based on a predicate.
        /// </summary>
        /// <returns>A <see cref="IAsyncEnumerable{T}"/> that will contains elements from the input sequence that satisfy the condition.</returns>
        public static async IAsyncEnumerable<T> WhereAsync<T>(this IAsyncEnumerable<T> enumerable, Func<T, Task<bool>> asynchronousPredicate)
        {
            await foreach (var element in enumerable) {
                if (await asynchronousPredicate(element)) {
                    yield return element;
                }
            }
        }

        /// <summary>
        /// Asynchronously project each element on an asynchronous sequence in a new form.
        /// </summary>
        /// <returns>A <see cref="IAsyncEnumerable{T}"/> that will contains elements whose are the result of invoking the transform function on each element of the source.</returns>
        public static async IAsyncEnumerable<TResult> SelectAsync<T, TResult>(this IAsyncEnumerable<T> enumerable, Func<T, TResult> projection)
        {
            await foreach (var element in enumerable) {
                yield return projection(element);
            }
        }

        /// <summary>
        /// Asynchronously project each element on an asynchronous sequence in a new form.
        /// </summary>
        /// <returns>A <see cref="IAsyncEnumerable{T}"/> that will contains elements whose are the result of invoking the transform function on each element of the source.</returns>
        public static async IAsyncEnumerable<TResult> SelectAsync<T, TResult>(this IAsyncEnumerable<T> enumerable, Func<T, Task<TResult>> asynchronousProjection)
        {
            await foreach (var element in enumerable) {
                yield return await asynchronousProjection(element);
            }
        }
    }
}