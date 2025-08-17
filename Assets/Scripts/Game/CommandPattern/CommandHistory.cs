

using System.Collections.Generic;

namespace CommandPattern
{
    public class CommandHistory<TContext>
    {
        public Stack<ICommand<TContext>> UndoStack { get; private set; } = new();
        public Stack<ICommand<TContext>> RedoStack { get; set; } = new();

        public void ExecuteCommand(ICommand<TContext> command, TContext context)
        {
            UndoStack.Push(command);
            command.Execute(context);

            RedoStack.Clear();
        }

        public void Undo(TContext context)
        {
            if (UndoStack.Count == 0) return;

            ICommand<TContext> command = UndoStack.Pop();
            command.Undo(context);

            RedoStack.Push(command);
        }

        public void Redo(TContext context)
        {
            if (RedoStack.Count == 0) return;

            ICommand<TContext> command = RedoStack.Pop();
            command.Execute(context);

            UndoStack.Push(command);
        }
    }
}
