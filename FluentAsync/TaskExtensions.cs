using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace FluentAsync
{
    [DebuggerStepThrough]
    public static class TaskExtensions
    {
        /// <summary>
        /// Asynchronously filters a sequence of value based on a predicate.
        /// </summary>
        /// <returns>A <see cref="Task"/> that will contains elements from the input sequence that satisfy the condition.</returns>
        public static async Task<IEnumerable<T>> WhereAsync<T>(this Task<IEnumerable<T>> enumerable, Func<T, bool> predicate)
            => (await enumerable).Where(predicate);

        /// <summary>
        /// Asynchronously project each element on a sequence in a new form.
        /// </summary>
        /// <returns>A <see cref="Task"/> that will contains elements whose are the result of invoking the transform function on each element of the source.</returns>
        public static async Task<IEnumerable<TResult>> SelectAsync<T, TResult>(this Task<IEnumerable<T>> enumerable, Func<T, TResult> projection)
            => (await enumerable).Select(projection);

        /// <summary>
        /// Enumerable the task and retrieve readonly result.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <returns></returns>
        public static async Task<IReadOnlyCollection<T>> EnumerateAsync<T>(this Task<IEnumerable<T>> enumerable)
            => (await enumerable).ToArray();

        /// <summary>
        /// Asynchronously groups elements of a sequence according to a specified key selector function.
        /// </summary>
        /// <returns>An enumerable of groups and for each the key and elements matching the key selector function.</returns>
        public static async Task<IEnumerable<IGrouping<TKey, T>>> GroupByAsync<T, TKey>(this Task<IEnumerable<T>> enumerable, Func<T, TKey> keySelector)
            => (await enumerable).GroupBy(keySelector);

        /// <summary>
        /// Asynchronously projects each element of a sequence to an IEnumerable, and flatten the resulting sequence into one sequence.
        /// </summary>
        /// <returns>A Task that will contains elements that the result of invoking the one-to-many transform function on each element on the input sequence.</returns>
        public static async Task<IEnumerable<TCollection>> SelectManyAsync<T, TCollection>(this Task<IEnumerable<T>> enumerable, Func<T, IEnumerable<TCollection>> collectionSelector)
            => (await enumerable).SelectMany(collectionSelector);


        #region ToEnumerableTask

        public static async Task<IEnumerable<T>> ToEnumerableTask<T>(this Task<List<T>> enumerable) => await enumerable;
        public static async Task<IEnumerable<T>> ToEnumerableTask<T>(this Task<T[]> enumerable) => await enumerable;
        public static async Task<IEnumerable<T>> ToEnumerableTask<T>(this Task<IReadOnlyCollection<T>> enumerable) => await enumerable;
        public static async Task<IEnumerable<T>> ToEnumerableTask<T>(this Task<IReadOnlyList<T>> enumerable) => await enumerable;
        public static async Task<IEnumerable<T>> ToEnumerableTask<T>(this Task<HashSet<T>> enumerable) => await enumerable;
        public static async Task<IEnumerable<T>> ToEnumerableTask<T>(this Task<ICollection<T>> enumerable) => await enumerable;
        public static async Task<IEnumerable<T>> ToEnumerableTask<T>(this Task<Collection<T>> enumerable) => await enumerable;

        #endregion
    }
}