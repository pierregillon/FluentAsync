using System;
using System.Threading.Tasks;

namespace FluentAsync
{
    public static class FunctionalProgramingExtensions
    {
        public static TResult Pipe<T, TResult>(this T element, Func<T, TResult> projection) => projection(element);

        public static async Task<TResult> PipeAsync<T, TResult>(this Task<T> element, Func<T, TResult> projection) => projection(await element);
    }
}