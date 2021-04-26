using System.Diagnostics;
using System.Threading.Tasks;
using FluentAsync.CovariantTask;

namespace FluentAsync
{
    [DebuggerStepThrough]
    public static class TaskExtensions
    {
        public static ITask<T> ToCovariantTask<T>(this Task<T> task) => new TaskWrapper<T>(task);
    }
}