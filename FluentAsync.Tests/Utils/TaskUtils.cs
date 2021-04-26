using System.Threading.Tasks;

namespace FluentAsync.Tests.Utils
{
    public static class TaskUtils
    {
        public static async Task<T> WaitAndReturn<T>(T result, int timeToWait = 1)
        {
            await Task.Delay(timeToWait);
            return result;
        }
    }
}