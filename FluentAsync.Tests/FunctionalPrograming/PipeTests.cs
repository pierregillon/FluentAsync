using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace FluentAsync.Tests.FunctionalPrograming
{
    public class PipeTests
    {
        private static double Pow2(double x) => Math.Pow(x, 2);
        private static double Add1(double x) => x + 1;
        private static Task<string> DownloadFile(string url) => Task.FromResult("some content");

        [Fact]
        public void Pipe_functions()
        {
            var result = 10d
                .Pipe(Pow2)
                .Pipe(Add1);

            result
                .Should()
                .Be(101);
        }

        [Fact]
        public void If_default_piping()
        {
            DateTime date = default;
            var firstJanuary2021 = new DateTime(2021, 01, 01);

            date
                .IfDefault(firstJanuary2021)
                .Should()
                .Be(firstJanuary2021);
        }

        [Fact]
        public void If_null_piping()
        {
            IEnumerable<int> collection = null;

            var result = collection
                .IfNull(Array.Empty<int>())
                .Select(x => x % 2);

            result
                .Should()
                .BeEmpty();
        }

        [Fact]
        public void If_match_piping()
        {
            var value = 2;

            var result = value.IfMatch(2, x => x * 2);

            result
                .Should()
                .Be(4);
        }

        [Fact]
        public void If_match_predicate_piping()
        {
            static bool IsEven(int x) => x % 2 == 0;
            var value = 2;

            var result = value.IfMatch(IsEven, x => x * 2);

            result
                .Should()
                .Be(4);
        }

        [Fact]
        public void Match_piping()
        {
            var value = 123;
            static bool IsEven(int x) => x % 2 == 0;

            var result = value
                .Match(IsEven, x => x * 2, x => x * 3);

            result
                .Should()
                .Be(369);
        }

        [Fact]
        public async Task Asynchronously_pipe_functions()
        {
            var result = await Task.FromResult(10d)
                .PipeAsync(Pow2)
                .PipeAsync(Add1);

            result
                .Should()
                .Be(101);
        }

        [Fact]
        public async Task Asynchronously_pipe_asynchronous_functions()
        {
            var result = await Task.FromResult("https://somewebsite.com").PipeAsync(DownloadFile);

            result
                .Should()
                .Be("some content");
        }

        [Fact]
        public async Task Asynchronously_pipe_asynchronous_functions_3()
        {
            var result = await Task.FromResult("https://somewebsite.com")
                .PipeAsync(DownloadFile);

            result
                .Should()
                .Be("some content");
        }

        [Fact]
        public async Task Asynchronously_pipe_covariant_task()
        {
            var result = await Task.FromResult("https://somewebsite.com")
                .ChainWith()
                .PipeAsync(DownloadFile);

            result
                .Should()
                .Be("some content");
        }
    }

}