using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace FluentAsync.Tests.Tasks
{
    public class CovariantConversionPerformance
    {
        [Fact]
        public async Task Compare_task_execution_performance()
        {
            static async Task<IEnumerable<int>> GenerateIntegers()
            {
                await Task.Delay(1);
                return Enumerable.Range(0, 10000);
            }

            static ITask<string> BuildCovariantTask() => GenerateIntegers()
                .ToCovariantTask()
                .WhereAsync(x => x % 20 == 0)
                .OrderByDescendingAsync(x => x)
                .SelectAsync(x => $"Element is {x}")
                .AggregateAsync((x, y) => x + ", " + y);

            static async Task<string> BuildTask()
            {
                var elements = await GenerateIntegers();
                return elements
                    .Where(x => x % 20 == 0)
                    .OrderByDescending(x => x)
                    .Select(x => $"Element is {x}")
                    .Aggregate((x, y) => x + ", " + y);
            }


            var result = await Process(BuildTask, BuildCovariantTask);

            result.CovariantTaskMedianExecutionMs.Should().BeApproximately(result.StandardTaskMedianExecutionMs, 3);
        }

        [Fact]
        public async Task Compare_task_aggregation_performance()
        {
            const int count = 500;

            static async Task<IEnumerable<int>> GenerateIntegers()
            {
                await Task.Delay(1);
                return Enumerable.Range(0, count);
            }

            static ITask<IEnumerable<int>> BuildCovariantTask()
            {
                var task = GenerateIntegers().ToCovariantTask();
                for (var i = 1; i < count; i++) {
                    var i1 = i;
                    task = task
                        .SelectAsync(x => x + i1)
                        .WhereAsync(x => x % i1 == 0);
                }

                return task;
            }

            static Task<IEnumerable<int>> BuildTask()
            {
                var task = GenerateIntegers();
                for (var i = 1; i < count; i++) {
                    var i1 = i;
                    task = task
                        .PipeAsync(enumerable => enumerable.Select(x => x + i1))
                        .PipeAsync(enumerable => enumerable.Where(x => x % i1 == 0));
                }

                return task;
            }


            var result = await Process(BuildTask, BuildCovariantTask);

            result.CovariantTaskMedianExecutionMs.Should().BeApproximately(result.StandardTaskMedianExecutionMs, 3);
        }

        private static async Task<(decimal StandardTaskMedianExecutionMs, decimal CovariantTaskMedianExecutionMs)> Process<T>(Func<Task<T>> taskFactory, Func<ITask<T>> covariantTaskFactory)
        {
            const int count = 100;

            var standardTaskResult = await CalculateAverage(count, async () => await taskFactory());
            var covariantTaskResult = await CalculateAverage(count, async () => await covariantTaskFactory());

            standardTaskResult.Result.Should().BeEquivalentTo(covariantTaskResult.Result);

            return (
                StandardTaskMedianExecutionMs: standardTaskResult.Median,
                CovariantTaskMedianExecutionMs: covariantTaskResult.Median
            );
        }

        private static async Task<(decimal Median, T Result)> CalculateAverage<T>(int count, Func<Task<T>> build)
        {
            var total = new List<decimal>();
            var watch = new Stopwatch();
            for (var i = 0; i < count; i++) {
                watch.Restart();
                await build();
                total.Add(watch.ElapsedMilliseconds);
            }

            return (total.Median(), await build());
        }
    }

    public static class TestExtensions
    {
        private static int Partition<T>(this IList<T> list, int start, int end, Random rnd = null) where T : IComparable<T>
        {
            if (rnd != null) {
                list.Swap(end, rnd.Next(start, end + 1));
            }

            var pivot = list[end];
            var lastLow = start - 1;
            for (var i = start; i < end; i++) {
                if (list[i].CompareTo(pivot) <= 0) {
                    list.Swap(i, ++lastLow);
                }
            }

            list.Swap(end, ++lastLow);
            return lastLow;
        }

        /// <summary>
        /// Returns Nth smallest element from the list. Here n starts from 0 so that n=0 returns minimum, n=1 returns 2nd smallest element etc.
        /// Note: specified list would be mutated in the process.
        /// Reference: Introduction to Algorithms 3rd Edition, Corman et al, pp 216
        /// </summary>
        public static T NthOrderStatistic<T>(this IList<T> list, int n, Random rnd = null) where T : IComparable<T> => NthOrderStatistic(list, n, 0, list.Count - 1, rnd);

        private static T NthOrderStatistic<T>(this IList<T> list, int n, int start, int end, Random rnd) where T : IComparable<T>
        {
            while (true) {
                var pivotIndex = list.Partition(start, end, rnd);
                if (pivotIndex == n) {
                    return list[pivotIndex];
                }

                if (n < pivotIndex) {
                    end = pivotIndex - 1;
                }
                else {
                    start = pivotIndex + 1;
                }
            }
        }

        public static void Swap<T>(this IList<T> list, int i, int j)
        {
            if (i == j) //This check is not required but Partition function may make many calls so its for perf reason
            {
                return;
            }

            var temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }

        /// <summary>
        /// Note: specified list would be mutated in the process.
        /// </summary>
        public static T Median<T>(this IList<T> list) where T : IComparable<T> => list.NthOrderStatistic((list.Count - 1) / 2);

        public static double Median<T>(this IEnumerable<T> sequence, Func<T, double> getValue)
        {
            var list = sequence.Select(getValue).ToList();
            var mid = (list.Count - 1) / 2;
            return list.NthOrderStatistic(mid);
        }
    }
}