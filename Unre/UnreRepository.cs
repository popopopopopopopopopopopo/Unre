using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

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

        public UnreRepository() { }

        public UnreRepository(int maxUndo = 5, int maxRedo = 5, bool isDisposeRemovedState = true)
        {
            MaxUndoStack = maxUndo;
            MaxRedoStack = maxRedo;
            IsDisposeRemovedState = isDisposeRemovedState;
        }

        public LinkedList<T> UndoList { get; set; } = new LinkedList<T>();

        public LinkedList<T> RedoList { get; set; } = new LinkedList<T>();

        public T State { get; set; } = null;

        public int MaxUndoStack { get; set; } = 5;

        public int MaxRedoStack { get; set; } = 5;

        public bool IsDisposeRemovedState { get; set; } = true;

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

        public bool IsCommonCommandsExecureRaise { get; set; } = false;

        public T Do(T entity, string raiseCommandGroupName = "")
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
            if (MaxUndoStack <= UndoList.Count) UndoList.RemoveLast(IsDisposeRemovedState);
            UndoList.AddFirst(addEntity);
            if (RedoList.Any()) RedoList.Clear();

            ExecuteCommonsRaiseIsIfRequire();
            ExecuteGroupsRaiseIsRequire(raiseCommandGroupName);

            return entity;
        }

        public T Do(ref T toMapEntity, T entity, string raiseCommandGroupName = "")
        {
            toMapEntity = Do(entity);
            return toMapEntity;
        }

        public T Undo(T entity = null, string raiseCommandGroupName = "")
        {
            T returnState = null;
            if (IsCanUndo)
            {
                var tOut = UndoList.First.Value;
                UndoList.RemoveFirst();
                if (MaxRedoStack <= RedoList.Count) RedoList.RemoveLast(IsDisposeRemovedState);
                if (!IsStateEmpty) RedoList.AddFirst(State);
                State = tOut;
                returnState = tOut;
            }
            if (returnState == null) returnState = entity ?? null;

            ExecuteCommonsRaiseIsIfRequire();
            ExecuteGroupsRaiseIsRequire(raiseCommandGroupName);

            return returnState;
        }

        public T Undo(ref T toMapEntity, T entity = null, string raiseCommandGroupName = "")
        {
            toMapEntity = Undo(entity, raiseCommandGroupName);

            return toMapEntity;
        }

        public T Redo(T entity = null, string raiseCommandGroupName = "")
        {
            T returnState = null;
            if (IsCanRedo)
            {
                var tOut = RedoList.First.Value;
                RedoList.RemoveFirst();
                if (MaxUndoStack <= UndoList.Count) UndoList.RemoveLast(IsDisposeRemovedState);
                if (!IsStateEmpty) UndoList.AddFirst(State);
                State = tOut;
                returnState = tOut;
            }
            if(returnState == null) returnState = entity ?? null;

            ExecuteCommonsRaiseIsIfRequire();
            ExecuteGroupsRaiseIsRequire(raiseCommandGroupName);

            return returnState;
        }

        public T Redo(ref T toMapEntity, T entity = null, string raiseCommandGroupName = "")
        {
            toMapEntity = Redo(entity, raiseCommandGroupName);
            return toMapEntity;
        }

        public void SetMaxUndo(int max) => MaxUndoStack = max;

        public void SetMaxRedo(int max) => MaxRedoStack = max;

        public void SetIsDisposeRequire(bool isRequire) => IsDisposeRemovedState = isRequire;

        public void SetIsCommonCommandsExecureRaise(bool isRequire) => IsCommonCommandsExecureRaise = isRequire;

        public List<Tuple<ICommand, string, MethodInfo>> CommonCommandsCache { get; set; }

        public ConcurrentDictionary<string, List<Tuple<ICommand,string,MethodInfo>>> CommandsCache { get; set; }

        public void ClearGroupsCommands()
        {
            if (CommandsCache != null)
            {
                foreach (var pair in CommandsCache)
                {
                    pair.Value.Clear();
                }
                CommandsCache.Clear();
            }
            CommandsCache = null;
        }

        public UnreRepository<T> SetGroupCommand(ICommand command, string raiseExecuteMethodName, string commandGroupName)
        {
            if (CommandsCache == null)
                CommandsCache = new ConcurrentDictionary<string, List<Tuple<ICommand, string, MethodInfo>>>();

            var groupCommandlist = CommandsCache.GetOrAdd(commandGroupName, key => new List<Tuple<ICommand, string, MethodInfo>>());

            var raiseMethod = command.GetType().GetMethods()?.FirstOrDefault(m => m.Name.Equals(raiseExecuteMethodName));

            groupCommandlist.Add(new Tuple<ICommand, string, MethodInfo>(command,raiseExecuteMethodName, raiseMethod));

            return this;
        }
        
        public UnreRepository<T> SetGroupCommands(IEnumerable<ICommand> commands, string raiseExecuteMethodName, string commandGroupName)
        {
            //TODO:commands null or notany check
            foreach (var command in commands)
            {
                SetGroupCommand(command, raiseExecuteMethodName, commandGroupName);
            }

            return this;
        }

        public void ClearCommonsCommands()
        {
            CommonCommandsCache?.Clear();
            CommonCommandsCache = null;
        }

        public UnreRepository<T> SetCommonCommand(ICommand command, string raiseExecuteMethodName)
        {
            if (CommonCommandsCache == null)
                CommonCommandsCache = new List<Tuple<ICommand, string, MethodInfo>>();

            var raiseMethod = command.GetType().GetMethods()?.FirstOrDefault(m => m.Name.Equals(raiseExecuteMethodName));

            CommonCommandsCache.Add(new Tuple<ICommand, string, MethodInfo>(command, raiseExecuteMethodName, raiseMethod));

            return this;
        }

        public UnreRepository<T> SetCommonCommands(IEnumerable<ICommand> commands, string raiseExecuteMethodName)
        {
            //TODO:commands null or notany check
            foreach (var command in commands)
            {
                SetCommonCommand(command, raiseExecuteMethodName);
            }

            return this;
        }

        public void ExecuteGroupsRaise(string commandGroupName)
        {
            if (CommandsCache == null) return;

            List<Tuple<ICommand, string, MethodInfo>> list = null;
            if (CommandsCache.TryGetValue(commandGroupName, out list))
            {
                if(list.Any()) list.ForEach(t => t.Item3?.Invoke(t.Item1,null));
            }
        }

        private void ExecuteGroupsRaiseIsRequire(string commandGroupName)
        {
            if (!string.IsNullOrEmpty(commandGroupName)) ExecuteGroupsRaise(commandGroupName);
        }

        public void ExecuteCommonsRaise()
        {
            if (CommonCommandsCache == null) return;
            if (CommonCommandsCache.Any()) CommonCommandsCache.ForEach(t => t.Item3?.Invoke(t.Item1, null));
        }

        private void ExecuteCommonsRaiseIsIfRequire()
        {
            if (CommonCommandsCache != null 
                && CommonCommandsCache.Any()
                && IsCommonCommandsExecureRaise) ExecuteCommonsRaise();
        }
    }
}
