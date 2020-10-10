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