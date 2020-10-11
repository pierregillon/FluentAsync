using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace FluentAsync.Tests.Tasks
{
    public class AggregateAsyncTests
    {
        private readonly IEnumerable<int> _elements = Enumerable.Range(0, 20);

        protected Task<IEnumerable<int>> Task => System.Threading.Tasks.Task.FromResult(_elements);

        [Fact]
        public async Task Aggregate_elements_asynchronously()
        {
            var composedWords = await Task.AggregateAsync((x, y) => x + y);

            composedWords.Should().Be(190);
        }

        [Fact]
        public async Task Aggregate_elements_asynchronously_from_a_seed()
        {
            var composedWords = await Task.AggregateAsync(100, (x, y) => x + y);

            composedWords.Should().Be(290);
        }

        [Fact]
        public async Task Aggregate_elements_asynchronously_from_a_seed_and_select_the_result_value()
        {
            var composedWords = await Task.AggregateAsync(100, (x, y) => x + y, x => x / 2);

            composedWords.Should().Be(145);
        }
    }
}