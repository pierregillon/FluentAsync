using System.Collections.Generic;
using System.Threading.Tasks;

namespace FluentAsync
{
    public static class AsyncEnumerableExtensions
    {
        /// <summary>
        /// Enumerate all the elements of the async enumerable and returns a collection result.
        /// Warning : be aware that if the async enumerable is an infinite loop enumeration, this method will block the current thread for ever.
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="enumerable"></param>
        /// <returns></returns>
        public static async Task<IReadOnlyCollection<TResult>> EnumerateAll<TResult>(this IAsyncEnumerable<TResult> enumerable)
        {
            var results = new List<TResult>();
            await foreach (var element in enumerable) {
                results.Add(element);
            }
            return results;
        }
    }
}