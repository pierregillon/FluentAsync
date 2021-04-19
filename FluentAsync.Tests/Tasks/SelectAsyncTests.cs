using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace FluentAsync.Tests.Tasks
{
    public class SelectAsyncTests
    {
        private static readonly IEnumerable<string> Elements = new List<string> {
            "hello world",
            "please",
            "do it now",
            "cuz"
        };

        private readonly ITask<IEnumerable<string>> task = Task.FromResult(Elements).ToCovariantTask();

        [Fact]
        public async Task Project_each_element()
        {
            var results = await task.SelectAsync(x => x.Length);

            results
                .Should()
                .BeEquivalentTo(11, 6, 9, 3);
        }

        [Fact]
        public async Task Execute_projection_only_on_enumeration()
        {
            var selectCallCount = 0;

            var composedWords = await task
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
            var results = await task
                .SelectAsync(x => x.Length)
                .SelectAsync(x => x % 2)
                .EnumerateAsync();

            results.Should().BeEquivalentTo(1, 0, 1, 1);
        }
    }
}