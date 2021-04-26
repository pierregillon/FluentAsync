using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace FluentAsync.Tests.Collections
{
    public class WhereAsyncTests
    {
        [Fact]
        public async Task Asynchronously_project_each_elements()
        {
            var results = await new[] { 1, 2, 3, 4 }
                .WhereAsync(IsOddAsync)
                .EnumerateAsync();

            results
                .Should()
                .BeEquivalentTo(1, 3);
        }

        private static Task<bool> IsOddAsync(int element)
            => Task.FromResult(element % 2 == 1);
    }
}