﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace FluentAsync.Tests
{
    public class GroupByAsyncTests
    {
        private readonly IEnumerable<Person> _persons = new List<Person> {
            new Person { Name = "bob", Age = 5 },
            new Person { Name = "sarah", Age = 16 },
            new Person { Name = "john", Age = 20 },
            new Person { Name = "isaac", Age = 26 },
        };

        private Task<IEnumerable<Person>> Task => System.Threading.Tasks.Task.FromResult(_persons);

        [Fact]
        public async Task Group_elements_asynchronously()
        {
            var groups = await Task.GroupByAsync(x => x.Age / 18);

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

            var composedWords = await Task
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
            var results = await Task
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