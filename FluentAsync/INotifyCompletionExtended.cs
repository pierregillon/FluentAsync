using System.Runtime.CompilerServices;

namespace FluentAsync
{
    public interface INotifyCompletionExtended<out T> : INotifyCompletion
    {
        bool IsCompleted { get; }
        T GetResult();
    }
}