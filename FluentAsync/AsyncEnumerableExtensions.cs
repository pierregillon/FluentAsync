using System.Collections.Generic;
using System.Threading.Tasks;

namespace FluentAsync
{
    public static class AsyncEnumerableExtensions
    {
        public static async Task<IReadOnlyCollection<TResult>> WhenAll<TResult>(this IAsyncEnumerable<TResult> enumerable)
        {
            var results = new List<TResult>();
            await foreach (var element in enumerable) {
                results.Add(element);
            }
            return results;
        }
    }
}