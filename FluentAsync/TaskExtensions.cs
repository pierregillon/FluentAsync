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
        /// Enumerable the task enumerable and retrieve a readonly result.
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

        /// <summary>
        /// Asynchronously returns the first element of a sequence.
        /// </summary>
        /// <returns>The first element on the specified sequence.</returns>
        public static async Task<T> FirstAsync<T>(this Task<IEnumerable<T>> enumerable)
            => (await enumerable).First();

        /// <summary>
        /// Asynchronously returns the first element of a sequence that satisfies the specified condition.
        /// </summary>
        /// <returns>The first element on the specified sequence.</returns>
        public static async Task<T> FirstAsync<T>(this Task<IEnumerable<T>> enumerable, Func<T, bool> predicate)
            => (await enumerable).First(predicate);

        /// <summary>
        /// Returns the first element of a sequence, or a default value if the sequence contains no elements.
        /// </summary>
        /// <returns></returns>
        public static async Task<T> FirstOrDefaultAsync<T>(this Task<IEnumerable<T>> enumerable)
            => (await enumerable).FirstOrDefault();

        /// <summary>
        /// Returns the first element of a sequence, that satisfies a condition, or a default value if the sequence contains no elements.
        /// </summary>
        /// <returns></returns>
        public static async Task<T> FirstOrDefaultAsync<T>(this Task<IEnumerable<T>> enumerable, Func<T, bool> predicate)
            => (await enumerable).FirstOrDefault(predicate);

        /// <summary>
        /// Asynchronously sort the elements of a sequence in ascending order according to a key.
        /// </summary>
        /// <returns>An <see cref="IOrderedEnumerable{TElement}"/> whose elements are sorted according to a key.</returns>
        public static async Task<IEnumerable<T>> OrderByAsync<T, TSelector>(this Task<IEnumerable<T>> enumerable, Func<T, TSelector> keySelector)
            => (await enumerable).OrderBy(keySelector);

        /// <summary>
        /// Asynchronously sort the elements of a sequence in descending order according to a key.
        /// </summary>
        /// <returns>An <see cref="IOrderedEnumerable{TElement}"/> whose elements are sorted according to a key.</returns>
        public static async Task<IEnumerable<T>> OrderByDescendingAsync<T, TSelector>(this Task<IEnumerable<T>> enumerable, Func<T, TSelector> keySelector)
            => (await enumerable).OrderByDescending(keySelector);

        /// <summary>
        /// Asynchronously sort the elements of a sequence in ascending order according to the default comparer.
        /// </summary>
        /// <returns>An <see cref="IOrderedEnumerable{TElement}"/> whose elements are sorted.</returns>
        public static async Task<IEnumerable<T>> OrderByAsync<T>(this Task<IEnumerable<T>> enumerable)
            => (await enumerable).OrderBy(x => x);

        /// <summary>
        /// Asynchronously sort the elements of a sequence in descending order according to the default comparer.
        /// </summary>
        /// <returns>An <see cref="IOrderedEnumerable{TElement}"/> whose elements are sorted.</returns>
        public static async Task<IEnumerable<T>> OrderByDescendingAsync<T>(this Task<IEnumerable<T>> enumerable)
            => (await enumerable).OrderByDescending(x => x);

        /// <summary>
        /// Asynchronously apply an accumulator function over a sequence.
        /// </summary>
        /// <returns>The final accumulator value</returns>
        public static async Task<T> AggregateAsync<T>(this Task<IEnumerable<T>> enumerable, Func<T, T, T> func)
            => (await enumerable).Aggregate(func);

        /// <summary>
        /// Asynchronously apply an accumulator function over a sequence. The specified seed value is used as the initial accumulator value.
        /// </summary>
        /// <returns>The final accumulator value</returns>
        public static async Task<T> AggregateAsync<T>(this Task<IEnumerable<T>> enumerable, T seed, Func<T, T, T> func)
            => (await enumerable).Aggregate(seed, func);

        /// <summary>
        /// Asynchronously apply an accumulator function over a sequence. The specified seed value is used as the initial accumulator value, and the specified function is used to select the result value.
        /// </summary>
        /// <returns>The final accumulator value</returns>
        public static async Task<TResult> AggregateAsync<T, TResult>(this Task<IEnumerable<T>> enumerable, T seed, Func<T, T, T> func, Func<T, TResult> resultSelector)
            => (await enumerable).Aggregate(seed, func, resultSelector);

        #region ToEnumerableTask

        public static async Task<IEnumerable<T>> ToEnumerableTask<T>(this Task<List<T>> enumerable) => await enumerable;
        public static async Task<IEnumerable<T>> AsEnumerable<T>(this Task<T[]> enumerable) => await enumerable;
        public static async Task<IEnumerable<T>> ToEnumerableTask<T>(this Task<IReadOnlyCollection<T>> enumerable) => await enumerable;
        public static async Task<IEnumerable<T>> ToEnumerableTask<T>(this Task<IReadOnlyList<T>> enumerable) => await enumerable;
        public static async Task<IEnumerable<T>> ToEnumerableTask<T>(this Task<HashSet<T>> enumerable) => await enumerable;
        public static async Task<IEnumerable<T>> ToEnumerableTask<T>(this Task<ICollection<T>> enumerable) => await enumerable;
        public static async Task<IEnumerable<T>> ToEnumerableTask<T>(this Task<Collection<T>> enumerable) => await enumerable;
        public static async Task<IEnumerable<T>> ToEnumerableTask<T, TKey>(this Task<IGrouping<TKey, T>> enumerable) => await enumerable;

        #endregion
    }
}