using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAsync.CovariantTask;
using FluentAsync.Tests.Utils;
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

            result.CovariantTaskMedianExecutionMs
                .Should()
                .BeApproximately(result.StandardTaskMedianExecutionMs, 3);
        }

        [Fact]
        public async Task Compare_task_composition_performance()
        {
            const int count = 100;

            static async Task<IEnumerable<int>> GenerateIntegers()
            {
                await Task.Delay(1);
                return Enumerable.Range(0, 20);
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

            result.CovariantTaskMedianExecutionMs
                .Should()
                .BeApproximately(result.StandardTaskMedianExecutionMs, 3);
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
}