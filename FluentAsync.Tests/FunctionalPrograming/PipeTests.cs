using System;
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
    }
}