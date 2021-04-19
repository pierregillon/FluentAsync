namespace FluentAsync
{
    public interface ITask<out T>
    {
        INotifyCompletionExtended<T> GetAwaiter();
    }
}