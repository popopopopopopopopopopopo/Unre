### SetCommands And Raise
========================================

Features
--------
By setting a commands, when Do / Undo / Redo,
You can call RaiseExecuteCommand.

Execute Do/Undo/Redo a Instance Repository(Recommend)
------------------------------------------------------------

Example usage:

```csharp

            //CommonCommands Set.
            Repository.SetCommonCommand(DoCommand, nameof(DoCommand.RaiseCanExecuteChanged))
                .SetCommonCommand(RedoCommand, nameof(RedoCommand.RaiseCanExecuteChanged))
                .SetCommonCommand(UndoCommand, nameof(UndoCommand.RaiseCanExecuteChanged));

            // For each Do, Redo, Undo, set the command CanExecute to Raise
            Repository.SetIsCommonCommandsExecureRaise(true);

            DoCommand = new DelegateCommand(() =>
            {
                var emp = new Employee();
                emp.UserId = (_myCount += 1);
                Repository.Do(emp);
                Status = emp.UserId.ToString();
            });

            RedoCommand = new DelegateCommand(() =>
            {
                var emp = Repository.Redo();
                Status = emp.UserId.ToString();
            }, ()=> Repository.IsCanRedo);

            UndoCommand = new DelegateCommand(() =>
            {
                var emp = Repository.Undo();
                Status = emp.UserId.ToString();
            }, ()=> Repository.IsCanUndo);


```

Execute Do/Undo/Redo by static Repository(Not Recommend)
------------------------------------------------------------
Not Implemented.