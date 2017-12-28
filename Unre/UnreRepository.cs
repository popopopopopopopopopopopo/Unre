using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unre
{
    public class UnreRepository<T> where T : class
    {
        private static UnreRepository<T> _myInstance;

        public static UnreRepository<T> Instance
        {
            get => _myInstance ?? GetInstance();
        }

        private static UnreRepository<T> GetInstance() => _myInstance ?? (_myInstance = new UnreRepository<T>());
        
        public LinkedList<T> UndoList { get; set; } = new LinkedList<T>();

        public LinkedList<T> RedoList { get; set; } = new LinkedList<T>();

        public T State { get; set; } = null;

        public int MaxUndoStack { get; set; } = 5;

        public int MaxRedoStack { get; set; } = 5;

        public bool IsRemovingRequireDispose { get; set; } = true;

        public bool IsStateEmpty
        {
            get => State == null;
        }

        public bool IsCanUndo
        {
            get => UndoList.Any();
        }

        public bool IsCanRedo
        {
            get => RedoList.Any();
        }

        public T Do(T entity)
        {
            T addEntity = null;
            if (IsStateEmpty)
            {
                addEntity = entity;
            }
            else
            {
                addEntity = State;
            }
            State = entity;
            if (MaxUndoStack <= UndoList.Count) UndoList.RemoveLast(IsRemovingRequireDispose);
            UndoList.AddFirst(addEntity);
            if (RedoList.Any()) RedoList.Clear();

            return entity;
        }

        public T Undo(T entity = null)
        {
            if (IsCanUndo)
            {
                var tOut = UndoList.First.Value;
                UndoList.RemoveFirst();
                if(MaxRedoStack <= RedoList.Count) RedoList.RemoveLast(IsRemovingRequireDispose);
                if(!IsStateEmpty) RedoList.AddFirst(State);
                State = tOut;
                return tOut;
            }
            return entity ?? null;
        }

        public T Redo(T entity = null)
        {
            if (IsCanRedo)
            {
                var tOut = RedoList.First.Value;
                RedoList.RemoveFirst();
                if(MaxUndoStack <= UndoList.Count) UndoList.RemoveLast(IsRemovingRequireDispose);
                if (!IsStateEmpty) UndoList.AddFirst(State);
                State = tOut;

                return tOut;
            }
            return entity ?? null;
        }
    }
}
