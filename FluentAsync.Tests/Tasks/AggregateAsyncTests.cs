using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAsync.CovariantTask;
using Xunit;

namespace FluentAsync.Tests.Tasks
{
    public class AggregateAsyncTests
    {
        private static readonly IEnumerable<int> Elements = Enumerable.Range(0, 20);

        private readonly ITask<IEnumerable<int>> task = Elements
            .Pipe(Task.FromResult)
            .ChainWith();

        [Fact]
        public async Task Aggregate_elements_asynchronously()
        {
            var composedWords = await task.AggregateAsync((x, y) => x + y);

            composedWords.Should().Be(190);
        }

        [Fact]
        public async Task Aggregate_elements_asynchronously_from_a_seed()
        {
            var composedWords = await task.AggregateAsync(100, (x, y) => x + y);

            composedWords.Should().Be(290);
        }

        [Fact]
        public async Task Aggregate_elements_asynchronously_from_a_seed_and_select_the_result_value()
        {
            var composedWords = await task.AggregateAsync(100, (x, y) => x + y, x => x / 2);

            composedWords.Should().Be(145);
        }
    }
}