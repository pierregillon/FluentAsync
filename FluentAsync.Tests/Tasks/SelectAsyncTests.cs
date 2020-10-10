using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace FluentAsync.Tests.Tasks
{
    public class SelectAsyncTests
    {
        private readonly IEnumerable<string> _elements = new List<string> {
            "hello world",
            "please",
            "do it now",
            "cuz"
        };

        protected Task<IEnumerable<string>> Task => System.Threading.Tasks.Task.FromResult(_elements);

        [Fact]
        public async Task Project_each_element()
        {
            var results = await Task.SelectAsync(x => x.Length);

            results
                .Should()
                .BeEquivalentTo(11, 6, 9, 3);
        }

        [Fact]
        public async Task Execute_projection_only_on_enumeration()
        {
            var selectCallCount = 0;

            var composedWords = await Task
                .SelectAsync(x => {
                    selectCallCount++;
                    return x.Length;
                });

            selectCallCount.Should().Be(0);

            _ = composedWords.ToArray();

            selectCallCount.Should().Be(4);
        }

        [Fact]
        public async Task Can_be_chained()
        {
            var results = await Task
                .SelectAsync(x => x.Length)
                .SelectAsync(x => x % 2)
                .EnumerateAsync();

            results.Should().BeEquivalentTo(1, 0, 1, 1);
        }
    }
}