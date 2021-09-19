using System;
using FluentAssertions;
using FluentAsync.Functional;
using Xunit;

namespace FluentAsync.Tests.FunctionalPrograming
{
    public class FunctionExtensionsTests
    {
        [Fact]
        public void Currying_function()
        {
            Func<int, int, int> multiplyBy = (x, n) => x * n;

            var multiplyBy3 = multiplyBy.Curry(3);

            multiplyBy3(3)
                .Should()
                .Be(9);
        }

        [Fact]
        public void Currying_function_using_custom_operator()
        {
            ExtendedDelegate<int, int, int> multiplyBy = new ExtendedDelegate<int, int, int>((x, n) => x * n);

            var multiplyBy3 = multiplyBy > 3;

            multiplyBy3
                .Exec(3)
                .Should()
                .Be(9);
        }

        [Fact]
        public void Double_currying()
        {
            Func<int, int, int> multiplyBy = (x, n) => x * n;

            var multiply3By2 = multiplyBy.Curry(3).Curry(2);

            multiply3By2()
                .Should()
                .Be(6);
        }


        [Fact]
        public void Double_currying_with_operator()
        {
            ExtendedDelegate<int, int, int> multiplyBy = new ExtendedDelegate<int, int, int>((x, n) => x * n);

            var multiply3By2 = multiplyBy > 3 > 2;

            multiply3By2
                .Exec()
                .Should()
                .Be(6);
        }

        [Fact]
        public void Composing_functions()
        {
            Func<int, int, int> add = (x, y) => x + y;
            var add1 = add.Curry(1);

            Func<int, int, int> multiply = (x, y) => x * y;
            var multiplyBy2 = multiply.Curry(2);

            var add1AndMultiplyBy2 = add1.Then(multiplyBy2);

            5.Pipe(add1AndMultiplyBy2).Should().Be(12);
        }

        [Fact]
        public void Composing_functions_with_operator()
        {
            Func<int, int, int> add = (x, y) => x + y;
            ExtendedDelegate<int, int> add1 = add.Curry(1).Ext();

            Func<int, int, int> multiply = (x, y) => x * y;
            ExtendedDelegate<int, int> multiplyBy2 = multiply.Curry(2).Ext();

            var add1AndMultiplyBy2 = add1 & multiplyBy2;

            add1AndMultiplyBy2
                .Exec(5)
                .Should()
                .Be(12);
        }
    }
}