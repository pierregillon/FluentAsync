using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace FluentAsync.Tests.Tasks
{
    public class AsyncEnumerableExtensionsTests
    {
        [Theory]
        [InlineData(100)]
        [InlineData(4)]
        public async Task EnumerableAllElementsOfAnAsyncEnumerable(int count)
        {
            var numberGenerator = new NumberGenerator();

            var numbers = await numberGenerator.GenerateNumbers(count).EnumerateAll();

            numbers.Should().HaveCount(count);
        }


        private class NumberGenerator
        {
            private readonly Random _random = new Random((int) DateTime.Now.Ticks);

            public async IAsyncEnumerable<int> GenerateNumbers(int count)
            {
                for (var i = 0; i < count; i++) {
                    yield return await Random();
                }
            }

            private Task<int> Random()
            {
                return Task.FromResult(_random.Next());
            }
        }
    }
}