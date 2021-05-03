using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAsync.Tests.Utils;
using Xunit;

namespace FluentAsync.Tests.Collections
{
    public class SelectAsyncTests
    {
        private readonly IEnumerable<string> _websites = new List<string>
        {
            "https://eatorganic.com",
            "https://savetheplanet.com",
            "https://doyourpart.net"
        };

        [Fact]
        public async Task Asynchronously_project_each_elements()
        {
            var results = await _websites
                .SelectAsync(DownloadPage)
                .EnumerateAsync();

            results
                .Should()
                .BeEquivalentTo(
                    "fake page content of https://eatorganic.com",
                    "fake page content of https://savetheplanet.com",
                    "fake page content of https://doyourpart.net"
                );
        }

        private static Task<string> DownloadPage(string url)
            => TaskUtils.WaitAndReturn($"fake page content of {url}");
    }
}