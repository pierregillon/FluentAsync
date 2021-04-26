using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAsync.CovariantTask;
using Xunit;

namespace FluentAsync.Tests.Examples
{
    public class ChainingMethods
    {
        [Fact]
        public async Task Test()
        {
            var asyncNumbers = Task.FromResult(Enumerable.Range(0, 100));

            var result = await asyncNumbers
                .ToCovariantTask()
                .WhereAsync(x => x % 20 == 0)
                .OrderByDescendingAsync(x => x)
                .SelectAsync(x => $"Element is {x}")
                .AggregateAsync((x, y) => x + ", " + y);

            result
                .Should()
                .Be("Element is 80, Element is 60, Element is 40, Element is 20, Element is 0");
        }
    }
}