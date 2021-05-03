using System.Runtime.CompilerServices;

namespace FluentAsync.CovariantTask
{
    public interface INotifyCompletionExtended<out T> : INotifyCompletion
    {
        bool IsCompleted { get; }
        T GetResult();
    }
}