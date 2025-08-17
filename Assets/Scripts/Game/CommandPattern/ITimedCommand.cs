

namespace CommandPattern
{
    public interface ITimedCommand<TContext> : ICommand<TContext>
    {
        float Time { get; }
    }
}
