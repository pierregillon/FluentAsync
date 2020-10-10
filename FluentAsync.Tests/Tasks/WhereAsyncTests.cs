using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace FluentAsync.Tests.Tasks
{
    public class WhereAsyncTests
    {
        private readonly IEnumerable<string> _elements = new List<string> {
            "hello world",
            "please",
            "do it now",
            "cuz"
        };

        protected Task<IEnumerable<string>> Task => System.Threading.Tasks.Task.FromResult(_elements);

        [Fact]
        public async Task Filters_elements_asynchronously()
        {
            var composedWords = await Task.WhereAsync(x => x.Contains(" "));

            composedWords.Should().BeEquivalentTo("hello world", "do it now");
        }

        [Fact]
        public async Task Execute_predicates_only_on_enumeration()
        {
            var whereCallCount = 0;

            var composedWords = await Task
                .WhereAsync(x => {
                    whereCallCount++;
                    return x.Contains(" ");
                });

            whereCallCount.Should().Be(0);

            _ = composedWords.ToArray();

            whereCallCount.Should().Be(4);
        }

        [Fact]
        public async Task Can_be_chained()
        {
            var results = await Task
                .WhereAsync(_ => true)
                .WhereAsync(x => x.Contains(" "))
                .WhereAsync(x => x.EndsWith('w'))
                .EnumerateAsync();

            results
                .Should()
                .BeEquivalentTo("do it now");
        }
    }
}