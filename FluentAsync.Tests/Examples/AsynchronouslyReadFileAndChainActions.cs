using System.Collections.Generic;
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
        private const string HEADER_PATTERN = @"\[.*\]";

        [Fact]
        public async Task Select_distinct_errors_in_a_log_file()
        {
            var lines = await ReadAllLinesOfLogFileAsync()
                .ToCovariantTask()
                .SelectAsync(x => new {
                    Header = Regex.Match(x, HEADER_PATTERN).Value,
                    Description = Regex.Replace(x, HEADER_PATTERN, string.Empty).Trim()
                })
                .WhereAsync(x => x.Header == "[ERROR]")
                .WhereAsync(x => !x.Description.ToLower().Contains("unhandled"))
                .SelectAsync(x => x.Description)
                .PipeAsync(RemoveDuplicatedLines)
                .OrderByAsync()
                .EnumerateAsync();

            lines
                .Should()
                .BeEquivalentTo(
                    "Failed to process the command : unable to find a correct handler",
                    "Unable to process the command : invalid cast exception."
                );
        }

        [Fact]
        public async Task Select_distinct_errors_in_a_log_file_without_async_extension()
        {
            var lines = await ReadAllLinesOfLogFileAsync();

            var filteredLines = lines
                .Select(x => new {
                    Header = Regex.Match(x, HEADER_PATTERN).Value,
                    Description = Regex.Replace(x, HEADER_PATTERN, string.Empty).Trim()
                })
                .Where(x => x.Header == "[ERROR]")
                .Where(x => !x.Description.ToLower().Contains("unhandled"))
                .Select(x => x.Description);

            var errorLines = RemoveDuplicatedLines(filteredLines).ToArray();

            errorLines
                .Should()
                .BeEquivalentTo(
                    "Failed to process the command : unable to find a correct handler",
                    "Unable to process the command : invalid cast exception."
                );
        }

        private static async Task<IEnumerable<string>> ReadAllLinesOfLogFileAsync() => await File.ReadAllLinesAsync("./Examples/Resources/logs.txt");

        private static IEnumerable<string> RemoveDuplicatedLines(IEnumerable<string> lines)
        {
            return lines
                .GroupBy(x => x)
                .Select(x => x.First());
        }
    }
}