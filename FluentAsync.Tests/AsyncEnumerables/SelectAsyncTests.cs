using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAsync.Tests.Utils;
using Xunit;

namespace FluentAsync.Tests.AsyncEnumerables {
    public class SelectAsyncTests
    {

        [Theory]
        [InlineData(100)]
        [InlineData(4)]
        public async Task Can_chain_enumeration_with_projection(int maxValue)
        {
            const int count = 10;
            var numberGenerator = new NumberGenerator(1, maxValue);

            var numbers = await numberGenerator
                .GenerateNumbers(count)
                .SelectAsync(x => x * 10)
                .EnumerateAsync();

            numbers.Should().Match(x => x.All(i => i < maxValue * 10));
        }

        [Theory]
        [InlineData(100)]
        [InlineData(4)]
        public async Task Can_chain_enumeration_with_asynchronous_projection(int maxValue)
        {
            const int count = 10;
            var numberGenerator = new NumberGenerator(1, maxValue);

            var numbers = await numberGenerator
                .GenerateNumbers(count)
                .SelectAsync(x => TaskUtils.WaitAndReturn(x * 20))
                .EnumerateAsync();

            numbers.Should().Match(x => x.All(i => i < maxValue * 20));
        }

        
    }
}