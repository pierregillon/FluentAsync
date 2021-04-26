using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAsync.Tests.Utils;
using Xunit;

namespace FluentAsync.Tests.Tasks
{
    public class FirstAsyncTests
    {
        private static readonly IEnumerable<string> Elements = new List<string> {
            "hello world",
            "please",
            "do it now",
            "cuz"
        };

        private readonly ITask<IEnumerable<string>> task = Task.FromResult(Elements).ToCovariantTask();

        [Fact]
        public async Task Get_the_first_element_of_an_enumerable_task()
        {
            (await task.FirstAsync()).Should().BeEquivalentTo("hello world");
            (await task.FirstOrDefaultAsync()).Should().BeEquivalentTo("hello world");
        }

        [Fact]
        public void Throw_error_when_getting_the_first_element_of_an_empty_enumerable_task()
        {
            var emptyTask = TaskUtils.WaitAndReturn(new List<string>()).ToCovariantTask();

            Func<Task> gettingFirst = async () => await emptyTask.FirstAsync();

            gettingFirst.Should().Throw<InvalidOperationException>().WithMessage("Sequence contains no elements");
        }

        [Fact]
        public async Task Get_the_first_element_of_an_enumerable_task_matching_a_predicate()
        {
            static bool Predicate(string x) => !x.Contains(" ");

            (await task.FirstAsync(Predicate)).Should().BeEquivalentTo("please");
            (await task.FirstOrDefaultAsync(Predicate)).Should().BeEquivalentTo("please");
        }

        [Fact]
        public async Task Returns_default_element_on_empty_enumerable()
        {
            var emptyTask = TaskUtils.WaitAndReturn(new List<string>()).ToCovariantTask();

            var result = await emptyTask.FirstOrDefaultAsync();

            result.Should().Be(default);
        }
    }
}