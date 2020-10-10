using System;
using System.Collections.Generic;
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
        public static async Task<IEnumerable<T>> WhereAsync<T>(this Task<IEnumerable<T>> enumerable, Func<T, bool> predicate) => (await enumerable).Where(predicate);


        public static async Task<IEnumerable<T>> ToEnumerableTask<T>(this Task<IEnumerable<T>> enumerable) => await enumerable;
        public static async Task<IEnumerable<T>> ToEnumerableTask<T>(this Task<List<T>> enumerable) => await enumerable;
        public static async Task<IEnumerable<T>> ToEnumerableTask<T>(this Task<T[]> enumerable) => await enumerable;
        public static async Task<IEnumerable<T>> ToEnumerableTask<T>(this Task<IReadOnlyCollection<T>> enumerable) => await enumerable;
        public static async Task<IEnumerable<T>> ToEnumerableTask<T>(this Task<IReadOnlyList<T>> enumerable) => await enumerable;
    }
}