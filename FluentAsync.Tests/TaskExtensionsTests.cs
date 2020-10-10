using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace FluentAsync.Tests
{
    public class TaskExtensionsTests
    {
        public class WhereAsync
        {
            private readonly List<string> _elements = new List<string> {
                "hello world",
                "please",
                "do it now",
                "cuz"
            };

            protected Task<IEnumerable<string>> Task => System.Threading.Tasks.Task.FromResult(_elements).ToEnumerableTask();

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
            public async Task Can_be_composed()
            {
                var whereCallCount = 0;

                var results = await Task
                    .WhereAsync(_ => true)
                    .WhereAsync(x => x.Contains(" "))
                    .WhereAsync(x => {
                        whereCallCount++;
                        return x.EndsWith('w');
                    });

                _ = results.ToArray();

                whereCallCount.Should().Be(2);
            }
        }
    }
}