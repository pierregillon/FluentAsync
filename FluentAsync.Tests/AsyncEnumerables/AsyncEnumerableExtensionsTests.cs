using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAsync.Tests.Utils;
using Xunit;

namespace FluentAsync.Tests.AsyncEnumerables
{
    public class AsyncEnumerableExtensionsTests
    {
        [Theory]
        [InlineData(100)]
        [InlineData(4)]
        public async Task Enumerate_all_elements_of_an_async_enumerable(int count)
        {
            var numberGenerator = new NumberGenerator();

            var numbers = await numberGenerator.GenerateNumbers(count).EnumerateAsync();

            numbers.Should().HaveCount(count);
        }

        [Theory]
        [InlineData(100)]
        [InlineData(4)]
        public async Task Can_chain_enumeration_with_synchronous_filter(int maxValue)
        {
            const int count = 10;
            var numberGenerator = new NumberGenerator(1, maxValue);

            var numbers = await numberGenerator
                .GenerateNumbers(count)
                .WhereAsync(x => x < maxValue)
                .EnumerateAsync();

            numbers.Should().HaveCount(count);
        }

        [Theory]
        [InlineData(100)]
        [InlineData(4)]
        public async Task Can_chain_enumeration_with_asynchronous_filter(int maxValue)
        {
            const int count = 10;
            var numberGenerator = new NumberGenerator(1, maxValue);

            var numbers = await numberGenerator
                .GenerateNumbers(count)
                .WhereAsync(x => Task.FromResult(x < maxValue))
                .EnumerateAsync();

            numbers.Should().HaveCount(count);
        }

        [Theory]
        [InlineData(100)]
        [InlineData(4)]
        public async Task Can_chain_enumeration_with_projection(int maxValue)
        {
            const int count = 10;
            var numberGenerator = new NumberGenerator(1, maxValue);

            var numbers = await numberGenerator
                .GenerateNumbers(count)
                .SelectAsync(x => x * 10)
                .EnumerateAsync();

            numbers.Should().Match(x => x.All(i => i < maxValue * 10));
        }

        [Theory]
        [InlineData(100)]
        [InlineData(4)]
        public async Task Can_chain_enumeration_with_asynchronous_projection(int maxValue)
        {
            const int count = 10;
            var numberGenerator = new NumberGenerator(1, maxValue);

            var numbers = await numberGenerator
                .GenerateNumbers(count)
                .SelectAsync(x => TaskUtils.WaitAndReturn(x * 20))
                .EnumerateAsync();

            numbers.Should().Match(x => x.All(i => i < maxValue * 20));
        }

        private class NumberGenerator
        {
            private readonly Random _random = new Random((int) DateTime.Now.Ticks);
            private readonly int max;
            private readonly int min;

            public NumberGenerator(int min = 0, int max = int.MaxValue)
            {
                this.min = min;
                this.max = max;
            }

            public async IAsyncEnumerable<int> GenerateNumbers(int count)
            {
                for (var i = 0; i < count; i++) {
                    yield return await Random();
                }
            }

            private Task<int> Random() => Task.FromResult(_random.Next(min, max));
        }
    }
}