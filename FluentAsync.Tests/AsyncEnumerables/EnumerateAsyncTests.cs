using System.Threading.Tasks;
using FluentAssertions;
using FluentAsync.Tests.Utils;
using Xunit;

namespace FluentAsync.Tests.AsyncEnumerables
{
    public class EnumerateAsyncTests
    {
        [Theory]
        [InlineData(100)]
        [InlineData(4)]
        public async Task Enumerate_all_elements_of_an_async_enumerable(int count)
        {
            var numberGenerator = new NumberGenerator();

            var numbers = await numberGenerator.GenerateNumbers(count).EnumerateAsync();

            numbers.Should().HaveCount(count);
        }
    }
}