using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace FluentAsync.Tests.Tasks
{
    public class GroupByAsyncTests
    {
        private static readonly IEnumerable<Person> Persons = new List<Person> {
            new Person { Name = "bob", Age = 5 },
            new Person { Name = "sarah", Age = 16 },
            new Person { Name = "john", Age = 20 },
            new Person { Name = "isaac", Age = 26 },
        };

        private readonly ITask<IEnumerable<Person>> task = Task.FromResult(Persons).ToCovariantTask();

        [Fact]
        public async Task Group_elements_asynchronously()
        {
            var groups = await task.GroupByAsync(x => x.Age / 18);

            groups.Should().BeEquivalentTo(new[] {
                new[] {
                    new Person { Name = "bob", Age = 5 },
                    new Person { Name = "sarah", Age = 16 },
                },
                new[] {
                    new Person { Name = "john", Age = 20 },
                    new Person { Name = "isaac", Age = 26 },
                }
            });
        }

        [Fact]
        public async Task Execute_grouping_only_on_enumeration()
        {
            var groupByCallCount = 0;

            var composedWords = await task
                .GroupByAsync(x => {
                    groupByCallCount++;
                    return x.Age / 18;
                });

            groupByCallCount.Should().Be(0);

            _ = composedWords.ToArray();

            groupByCallCount.Should().Be(4);
        }

        [Fact]
        public async Task Can_be_chained()
        {
            var results = await task
                .GroupByAsync(x => x.Age / 18)
                .GroupByAsync(x => x.Key % 2)
                .EnumerateAsync();

            results
                .Should()
                .BeEquivalentTo(new[] {
                    new[] {
                        new[] {
                            new Person { Name = "bob", Age = 5 },
                            new Person { Name = "sarah", Age = 16 },
                        },
                    },
                    new[] {
                        new[] {
                            new Person { Name = "john", Age = 20 },
                            new Person { Name = "isaac", Age = 26 },
                        },
                    }
                });
        }


        private class Person
        {
            public string Name { get; set; }
            public int Age { get; set; }
        }
    }
}