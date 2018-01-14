### With Action
========================================

Features
--------
By setting a Action<T>,
Do/Undo/Redo With Action

Execute Do/Undo/Redo a Instance Repository(Recommend)
------------------------------------------------------------

Example usage:

```csharp

            DoCommand = new DelegateCommand(() =>
            {
                var emp = new Employee();
                emp.UserId = (_myCount += 1);

				//Do With Action
                Repository.Do(emp,res=> Status = res?.UserId.ToString());
            });

            UndoCommand = new DelegateCommand(() =>
            {
				//Undo With Action
                Repository.Undo(res =>
                {
                    Status = res?.UserId.ToString();
                });

            }, () => Repository.IsCanUndo);

            RedoCommand = new DelegateCommand(() =>
            {
				//Redo With Action
                Repository.Redo(res =>
                {
                    Status = res?.UserId.ToString();
                });

            }, () => Repository.IsCanRedo);


```

Execute Do/Undo/Redo by static Repository(Not Recommend)
------------------------------------------------------------
Not Implemented.