using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace FluentAsync.Tests.Tasks
{
    public class OrderByAsyncTests
    {
        private static readonly IEnumerable<string> Elements = new List<string> {
            "hello world",
            "please",
            "do it now",
            "cuz"
        };

        private readonly ITask<IEnumerable<string>> task = Task.FromResult(Elements).ToCovariantTask();

        [Fact]
        public async Task Asynchronously_order_ascending_elements()
        {
            var expectedOrder = new [] { "cuz", "do it now", "hello world", "please" };

            (await task.OrderByAsync()).Should().ContainInOrder(expectedOrder);
            (await task.OrderByAsync(x => x)).Should().ContainInOrder(expectedOrder);
        }

        [Fact]
        public async Task Asynchronously_order_ascending_elements_by_selector()
        {
            (await task.OrderByAsync(x => x.Length)).Should().ContainInOrder("cuz", "please", "do it now", "hello world");
        }

        [Fact]
        public async Task Asynchronously_order_descending_elements()
        {
            var expectedOrder = new [] { "please", "hello world", "do it now", "cuz" };
            
            (await task.OrderByDescendingAsync()).Should().ContainInOrder(expectedOrder);
            (await task.OrderByDescendingAsync(x => x)).Should().ContainInOrder(expectedOrder);
        }

        [Fact]
        public async Task Asynchronously_order_descending_elements_by_selector()
        {
            (await task.OrderByDescendingAsync(x => x.Length)).Should().ContainInOrder("hello world", "do it now", "please", "cuz");
        }
    }
}