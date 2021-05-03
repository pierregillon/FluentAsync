namespace FluentAsync.CovariantTask
{
    public interface ITask<out T>
    {
        INotifyCompletionExtended<T> GetAwaiter();
    }
}