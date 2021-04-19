using System;
using System.Runtime.CompilerServices;

namespace FluentAsync
{
    public class TaskAwaiterWrapper<T> : INotifyCompletionExtended<T>
    {
        private readonly TaskAwaiter<T> awaiter;

        public bool IsCompleted { get; private set; }

        public T GetResult() => this.awaiter.GetResult();

        public TaskAwaiterWrapper(TaskAwaiter<T> awaiter)
        {
            this.awaiter = awaiter;
        }

        public void OnCompleted(Action continuation)
        {
            this.IsCompleted = true;
            this.awaiter.OnCompleted(continuation);
        }
    }
}