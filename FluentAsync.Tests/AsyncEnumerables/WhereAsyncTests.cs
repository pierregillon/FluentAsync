using System.Threading.Tasks;
using FluentAssertions;
using FluentAsync.Tests.Utils;
using Xunit;

namespace FluentAsync.Tests.AsyncEnumerables {
    public class WhereAsyncTests
    {
        [Theory]
        [InlineData(100)]
        [InlineData(4)]
        public async Task Can_chain_enumeration_with_synchronous_filter(int maxValue)
        {
            const int count = 10;
            var numberGenerator = new NumberGenerator(1, maxValue);

            var numbers = await numberGenerator
                .GenerateNumbers(count)
                .WhereAsync(x => x < maxValue)
                .EnumerateAsync();

            numbers.Should().HaveCount(count);
        }

        [Theory]
        [InlineData(100)]
        [InlineData(4)]
        public async Task Can_chain_enumeration_with_asynchronous_filter(int maxValue)
        {
            const int count = 10;
            var numberGenerator = new NumberGenerator(1, maxValue);

            var numbers = await numberGenerator
                .GenerateNumbers(count)
                .WhereAsync(x => Task.FromResult(x < maxValue))
                .EnumerateAsync();

            numbers.Should().HaveCount(count);
        }
    }
}