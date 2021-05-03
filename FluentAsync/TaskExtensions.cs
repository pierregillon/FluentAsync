using System.Diagnostics;
using System.Threading.Tasks;
using FluentAsync.CovariantTask;

namespace FluentAsync
{
    [DebuggerStepThrough]
    public static class TaskExtensions
    {
        /// <summary>
        /// Transform a Task{T} into a covariant ITask{T}, that allows fluent extension methods.
        /// </summary>
        /// <returns></returns>
        public static ITask<T> ChainWith<T>(this Task<T> task) => new TaskWrapper<T>(task);
    }
}