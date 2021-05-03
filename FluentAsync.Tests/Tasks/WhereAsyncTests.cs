using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAsync.CovariantTask;
using FluentAsync.Tests.Utils;
using Xunit;

namespace FluentAsync.Tests.Tasks
{
    public class WhereAsyncTests
    {
        private static readonly IEnumerable<string> Elements = new List<string> {
            "hello world",
            "please",
            "do it now",
            "cuz"
        };

        private readonly ITask<IEnumerable<string>> task = Task.FromResult(Elements).ChainWith();

        [Fact]
        public async Task Filters_elements_asynchronously()
        {
            var composedWords = await task.WhereAsync(x => x.Contains(" "));

            composedWords.Should().BeEquivalentTo("hello world", "do it now");
        }

        [Fact]
        public async Task Filters_elements_asynchronously_with_asynchronous_predicate()
        {
            var composedWords = await task.WhereAsync(x => TaskUtils.WaitAndReturn(x.Contains(" ")));

            composedWords.Should().BeEquivalentTo("hello world", "do it now");
        }

        [Fact]
        public async Task Execute_predicates_only_on_enumeration()
        {
            var whereCallCount = 0;

            var composedWords = await task
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
            var results = await task
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