
using System.Collections.Generic;

namespace CommandPattern
{
    public class CompositeCommand<TContext> : ICommand<TContext>
    {
        private readonly List<ICommand<TContext>> _childCommands;

        public CompositeCommand(List<ICommand<TContext>> childCommands)
        {
            _childCommands = childCommands;
        }

        public void Execute(TContext context)
        {
            // Execute the child commands in order
        }

        public void Undo(TContext context)
        {
            // Undo the child commands in reverse order
        }
    }
}
