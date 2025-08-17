
namespace CommandPattern
{
    public interface ICommand<TContext>
    {
        void Execute(TContext context);
        void Undo(TContext context);
    }
}
