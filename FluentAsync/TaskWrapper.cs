using System.Threading.Tasks;

namespace FluentAsync
{
    public class TaskWrapper<T> : ITask<T>
    {
        private readonly Task<T> wrappedTask;

        public TaskWrapper(Task<T> wrappedTask) => this.wrappedTask = wrappedTask;

        public INotifyCompletionExtended<T> GetAwaiter() => new TaskAwaiterWrapper<T>(wrappedTask.GetAwaiter());
    }
}