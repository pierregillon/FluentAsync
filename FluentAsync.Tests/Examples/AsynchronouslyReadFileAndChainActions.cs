using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace FluentAsync.Tests.Examples
{
    public class AsynchronouslyReadFileAndChainActions
    {
        [Fact]
        public async Task Select_distinct_errors_in_a_log_file()
        {
            var lines = await File.ReadAllLinesAsync("./Examples/Resources/logs.txt")
                .ToEnumerableTask()
                .WhereAsync(StartWithError)
                .PipeAsync(RemoveDuplicatedLines)
                .EnumerateAsync();

            lines
                .Should()
                .BeEquivalentTo(
                    "[ERROR] Failed to process the command : unable to find a correct handler", 
                    "[ERROR] Unhandled exception");
        }

        [Fact]
        public async Task Select_distinct_errors_in_a_log_file_without_async_extension()
        {
            var lines = await File.ReadAllLinesAsync("./Examples/Resources/logs.txt");

            var errorLines = lines
                .Where(StartWithError)
                .Pipe(RemoveDuplicatedLines)
                .ToArray();

            errorLines
                .Should()
                .BeEquivalentTo(
                    "[ERROR] Failed to process the command : unable to find a correct handler",
                    "[ERROR] Unhandled exception");
        }

        private static bool StartWithError(string x) => Regex.Match(x, @"\[.*\]").Value == "[ERROR]";

        private static IEnumerable<string> RemoveDuplicatedLines(IEnumerable<string> lines)
        {
            return lines
                .GroupBy(x => x)
                .Select(x => x.First());
        }
    }
}