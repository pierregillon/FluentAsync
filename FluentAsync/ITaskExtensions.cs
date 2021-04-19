﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace FluentAsync
{
    [DebuggerStepThrough]
    public static class ITaskExtensions
    {
        /// <summary>
        /// Asynchronously filters a sequence of value based on a predicate.
        /// </summary>
        /// <returns>A <see cref="Task"/> that will contains elements from the input sequence that satisfy the condition.</returns>
        public static ITask<IEnumerable<T>> WhereAsync<T>(this ITask<IEnumerable<T>> enumerable, Func<T, bool> predicate)
            => enumerable.Pipe(async x => (await x).Where(predicate)).ToCovariantTask();

        /// <summary>
        /// Asynchronously project each element on a sequence in a new form.
        /// </summary>
        /// <returns>A <see cref="Task"/> that will contains elements whose are the result of invoking the transform function on each element of the source.</returns>
        public static ITask<IEnumerable<TResult>> SelectAsync<T, TResult>(this ITask<IEnumerable<T>> enumerable, Func<T, TResult> projection)
            => enumerable.Pipe(async x => (await x).Select(projection)).ToCovariantTask();

        /// <summary>
        /// Enumerable the task enumerable and retrieve a readonly result.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <returns></returns>
        public static ITask<IReadOnlyCollection<T>> EnumerateAsync<T>(this ITask<IEnumerable<T>> enumerable)
            => enumerable.Pipe(async x => (await x).ToArray()).ToCovariantTask();

        /// <summary>
        /// Asynchronously groups elements of a sequence according to a specified key selector function.
        /// </summary>
        /// <returns>An enumerable of groups and for each the key and elements matching the key selector function.</returns>
        public static ITask<IEnumerable<IGrouping<TKey, T>>> GroupByAsync<T, TKey>(this ITask<IEnumerable<T>> enumerable, Func<T, TKey> keySelector)
            => enumerable.Pipe(async x => (await x).GroupBy(keySelector)).ToCovariantTask();

        /// <summary>
        /// Asynchronously projects each element of a sequence to an IEnumerable, and flatten the resulting sequence into one sequence.
        /// </summary>
        /// <returns>A Task that will contains elements that the result of invoking the one-to-many transform function on each element on the input sequence.</returns>
        public static ITask<IEnumerable<TCollection>> SelectManyAsync<T, TCollection>(this ITask<IEnumerable<T>> enumerable, Func<T, IEnumerable<TCollection>> collectionSelector)
            => enumerable.Pipe(async x => (await x).SelectMany(collectionSelector)).ToCovariantTask();

        /// <summary>
        /// Asynchronously returns the first element of a sequence.
        /// </summary>
        /// <returns>The first element on the specified sequence.</returns>
        public static ITask<T> FirstAsync<T>(this ITask<IEnumerable<T>> enumerable)
            => enumerable.Pipe(async x => (await x).First()).ToCovariantTask();

        /// <summary>
        /// Asynchronously returns the first element of a sequence that satisfies the specified condition.
        /// </summary>
        /// <returns>The first element on the specified sequence.</returns>
        public static ITask<T> FirstAsync<T>(this ITask<IEnumerable<T>> enumerable, Func<T, bool> predicate)
            => enumerable.Pipe(async x => (await x).First(predicate)).ToCovariantTask();

        /// <summary>
        /// Returns the first element of a sequence, or a default value if the sequence contains no elements.
        /// </summary>
        /// <returns></returns>
        public static ITask<T> FirstOrDefaultAsync<T>(this ITask<IEnumerable<T>> enumerable)
            => enumerable.Pipe(async x => (await x).FirstOrDefault()).ToCovariantTask();

        /// <summary>
        /// Returns the first element of a sequence, that satisfies a condition, or a default value if the sequence contains no elements.
        /// </summary>
        /// <returns></returns>
        public static ITask<T> FirstOrDefaultAsync<T>(this ITask<IEnumerable<T>> enumerable, Func<T, bool> predicate)
            => enumerable.Pipe(async x => (await x).FirstOrDefault(predicate)).ToCovariantTask();

        /// <summary>
        /// Asynchronously sort the elements of a sequence in ascending order according to a key.
        /// </summary>
        /// <returns>An <see cref="IOrderedEnumerable{TElement}"/> whose elements are sorted according to a key.</returns>
        public static ITask<IEnumerable<T>> OrderByAsync<T, TSelector>(this ITask<IEnumerable<T>> enumerable, Func<T, TSelector> keySelector)
            => enumerable.Pipe(async x => (await x).OrderBy(keySelector)).ToCovariantTask();

        /// <summary>
        /// Asynchronously sort the elements of a sequence in descending order according to a key.
        /// </summary>
        /// <returns>An <see cref="IOrderedEnumerable{TElement}"/> whose elements are sorted according to a key.</returns>
        public static ITask<IEnumerable<T>> OrderByDescendingAsync<T, TSelector>(this ITask<IEnumerable<T>> enumerable, Func<T, TSelector> keySelector)
            => enumerable.Pipe(async x => (await x).OrderByDescending(keySelector)).ToCovariantTask();

        /// <summary>
        /// Asynchronously sort the elements of a sequence in ascending order according to the default comparer.
        /// </summary>
        /// <returns>An <see cref="IOrderedEnumerable{TElement}"/> whose elements are sorted.</returns>
        public static ITask<IEnumerable<T>> OrderByAsync<T>(this ITask<IEnumerable<T>> enumerable)
            => enumerable.Pipe(async x => (await x).OrderBy(x => x)).ToCovariantTask();

        /// <summary>
        /// Asynchronously sort the elements of a sequence in descending order according to the default comparer.
        /// </summary>
        /// <returns>An <see cref="IOrderedEnumerable{TElement}"/> whose elements are sorted.</returns>
        public static ITask<IEnumerable<T>> OrderByDescendingAsync<T>(this ITask<IEnumerable<T>> enumerable)
            => enumerable.Pipe(async x => (await x).OrderByDescending(x => x)).ToCovariantTask();

        /// <summary>
        /// Asynchronously apply an accumulator function over a sequence.
        /// </summary>
        /// <returns>The final accumulator value</returns>
        public static ITask<T> AggregateAsync<T>(this ITask<IEnumerable<T>> enumerable, Func<T, T, T> func)
            => enumerable.Pipe(async x => (await x).Aggregate(func)).ToCovariantTask();

        /// <summary>
        /// Asynchronously apply an accumulator function over a sequence. The specified seed value is used as the initial accumulator value.
        /// </summary>
        /// <returns>The final accumulator value</returns>
        public static ITask<T> AggregateAsync<T>(this ITask<IEnumerable<T>> enumerable, T seed, Func<T, T, T> func)
            => enumerable.Pipe(async x => (await x).Aggregate(seed, func)).ToCovariantTask();

        /// <summary>
        /// Asynchronously apply an accumulator function over a sequence. The specified seed value is used as the initial accumulator value, and the specified function is used to select the result value.
        /// </summary>
        /// <returns>The final accumulator value</returns>
        public static ITask<TResult> AggregateAsync<T, TResult>(this ITask<IEnumerable<T>> enumerable, T seed, Func<T, T, T> func, Func<T, TResult> resultSelector)
            => enumerable.Pipe(async x => (await x).Aggregate(seed, func, resultSelector)).ToCovariantTask();

        public static ITask<TResult> PipeAsync<T, TResult>(this ITask<T> element, Func<T, TResult> projection)
            => element.Pipe(async x => projection(await element)).ToCovariantTask();
    }
}