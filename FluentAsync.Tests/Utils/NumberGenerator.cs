using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FluentAsync.Tests.Utils {
    public class NumberGenerator
    {
        private readonly Random _random = new Random((int)DateTime.Now.Ticks);
        private readonly int max;
        private readonly int min;

        public NumberGenerator(int min = 0, int max = int.MaxValue)
        {
            this.min = min;
            this.max = max;
        }

        public async IAsyncEnumerable<int> GenerateNumbers(int count)
        {
            for (var i = 0; i < count; i++)
            {
                yield return await Random();
            }
        }

        private Task<int> Random() => Task.FromResult(_random.Next(min, max));
    }
}