using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace FluentAsync.Tests.Tasks
{
    public class SelectManyAsyncTests
    {
        private static readonly IEnumerable<TodoList> TodoLists = new List<TodoList> {
            new TodoList {
                Items = new[] {
                    new TodoListItem { Name = "Clean the house", IsDone = false },
                    new TodoListItem { Name = "Walk out the dog", IsDone = true }
                }
            },
            new TodoList {
                Items = new[] {
                    new TodoListItem { Name = "Dance in the living room", IsDone = true },
                    new TodoListItem { Name = "Prepare dinner", IsDone = false }
                }
            }
        };

        private readonly ITask<IEnumerable<TodoList>> task = System.Threading.Tasks.Task.FromResult(TodoLists).ToCovariantTask();

        [Fact]
        public async Task SelectMany_elements_asynchronously()
        {
            var composedWords = await task.SelectManyAsync(x => x.Items);

            composedWords.Should().BeEquivalentTo(
                new TodoListItem { Name = "Clean the house", IsDone = false },
                new TodoListItem { Name = "Walk out the dog", IsDone = true },
                new TodoListItem { Name = "Dance in the living room", IsDone = true },
                new TodoListItem { Name = "Prepare dinner", IsDone = false }
            );
        }

        [Fact]
        public async Task Execute_collection_selector_only_on_enumeration()
        {
            var selectManyCount = 0;

            var composedWords = await task.SelectManyAsync(x => {
                selectManyCount++;
                return x.Items;
            });

            selectManyCount.Should().Be(0);

            _ = composedWords.ToArray();

            selectManyCount.Should().Be(2);
        }

        [Fact]
        public async Task Can_be_chained()
        {
            var results = await task
                .SelectManyAsync(x => x.Items)
                .SelectManyAsync(x => x.Name.Take(3))
                .EnumerateAsync();

            results
                .Should()
                .BeEquivalentTo('C', 'l', 'e', 'W', 'a', 'l', 'D', 'a', 'n', 'P', 'r', 'e');
        }

        private class TodoList
        {
            public IEnumerable<TodoListItem> Items { get; set; }
        }

        private class TodoListItem
        {
            public string Name { get; set; }
            public bool IsDone { get; set; }
        }
    }
}