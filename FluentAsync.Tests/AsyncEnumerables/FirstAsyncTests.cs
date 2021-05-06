using System;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAsync.Tests.Utils;
using Xunit;

namespace FluentAsync.Tests.AsyncEnumerables
{
    public class FirstAsyncTests
    {
        [Fact]
        public async Task Get_the_first_element_of_an_async_enumerable()
        {
            var numberGenerator = new NumberGenerator(0, 0);

            var number = await numberGenerator.GenerateNumbers(10).FirstAsync();

            number.Should().Be(0);

            var secondNumber = await numberGenerator.GenerateNumbers(10).FirstOrDefaultAsync();

            secondNumber.Should().Be(0);
        }

        [Fact]
        public void Throw_error_when_getting_the_first_element_but_no_element_found()
        {
            var numberGenerator = new NumberGenerator(0, 0);

            Func<Task> gettingFirstElementOfEmptyEnumerable = async () => await numberGenerator.GenerateNumbers(0).FirstAsync();

            gettingFirstElementOfEmptyEnumerable
                .Should()
                .Throw<InvalidOperationException>()
                .WithMessage("The sequence contains no element");
        }

        [Fact]
        public async Task Get_default_value_when_first_or_default_on_empty_collection()
        {
            var numberGenerator = new NumberGenerator(10, 100);

            var number = await numberGenerator.GenerateNumbers(0).FirstOrDefaultAsync();

            number.Should().Be(0);
        }

        [Fact]
        public async Task Get_first_value_that_match_a_predicate()
        {
            var numberGenerator = new NumberGenerator(10, 13);

            var number = await numberGenerator.GenerateNumbers(100).FirstAsync(x => x == 12);

            number.Should().Be(12);

            var secondNumber = await numberGenerator.GenerateNumbers(100).FirstOrDefaultAsync(x => x == 12);

            secondNumber.Should().Be(12);
        }
    }
}