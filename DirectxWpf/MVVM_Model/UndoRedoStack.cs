using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DirectxWpf.MVVM_Model
{
    public static class UndoRedoStack
    {
        private static Stack<Tuple<ICommand, object>>  _Undo ;
        private static Stack<Tuple<ICommand, object>> _Redo;

        private static Stack<Tuple<ICommand, object>> UndoStack 
        {
            get { if (_Undo == null) _Undo = new Stack<Tuple<ICommand, object>>(); return _Undo; }
        }

        private static Stack<Tuple<ICommand, object>> RedoStack
        {
            get { if (_Redo == null) _Redo = new Stack<Tuple<ICommand, object>>(); return _Redo; }
        }

        public static void AddUndoCommand<T>( ICommand Undo, T parameter) where T : class
        {
            var undoParameter = new Tuple<ICommand, object>(Undo, parameter);
            UndoStack.Push(undoParameter);
        }

        public static void AddRedoCommand<T>(ICommand Redo, T parameter) where T : class
        {
            var redoParameter = new Tuple<ICommand, object>(Redo, parameter);
            RedoStack.Push(redoParameter);
        }
        public static void Undo()
        {
            if (UndoStack.Count == 0)
                return;

           Tuple<ICommand, object> tuple = UndoStack.Pop();
           var undoCommand = tuple.Item1 as ICommand;
           if (undoCommand == null) return;

           undoCommand.Execute(tuple.Item2);

        }

        public static void Redo() 
        {
            if (RedoStack.Count == 0)
                return;

            Tuple<ICommand, object> tuple = RedoStack.Pop();
            var redoCommand = tuple.Item1 as ICommand;
            if (redoCommand == null) return;

            redoCommand.Execute(tuple.Item2);           
        }

        public static void Clear() 
        {
            UndoStack.Clear();
            RedoStack.Clear();
        }

        public static void ClearRedoStack() 
        {
            RedoStack.Clear();
        }
    }
}
