using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfAppUndoRedoPattern
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

        public void Do(T entity)
        {
            T addEntity = null;
            if (State == null)
            {
                State = entity;
                addEntity = entity;
            }
            else
            {
                addEntity = State;
                State = entity;
            }
            if (MaxUndoStack <= UndoList.Count) UndoList.RemoveLast();
            UndoList.AddFirst(addEntity);
            if (RedoList.Any()) RedoList.Clear();
        }

        public T Undo()
        {
            if (UndoList.Any())
            {
                var tOut = UndoList.First.Value;
                UndoList.RemoveFirst();
                if(MaxRedoStack <= RedoList.Count) RedoList.RemoveLast();
                if(State != null) RedoList.AddFirst(State);
                State = tOut;
                return tOut;
            }
            else
            {
                return null;
            }
        }

        public T Redo()
        {
            if (RedoList.Any())
            {
                var tOut = RedoList.First.Value;
                RedoList.RemoveFirst();
                if(MaxUndoStack <= UndoList.Count) UndoList.RemoveLast();
                if (State != null) UndoList.AddFirst(State);
                State = tOut;

                return tOut;
            }
            else
            {
                return null;
            }
        }

        public bool IsCanUndo
        {
            get => UndoList.Count > 0;
        }

        public bool IsCanRedo
        {
            get => RedoList.Count > 0;
        }
    }
}
