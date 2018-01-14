using System.Windows.Controls.Primitives;
using Prism.Commands;
using Prism.Mvvm;
using Unre;
using UnreSampleWithAction.Models;

namespace UnreSampleWithAction.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private string _title = "Unre Action Sample";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public DelegateCommand DoCommand { get; set; }

        public DelegateCommand UndoCommand { get; set; }

        public DelegateCommand RedoCommand { get; set; }

        public UnreRepository<Employee> Repository { get; set; } = new UnreRepository<Employee>();

        private string _myStatus = "not do/undo/redo";

        public string Status
        {
            get => _myStatus;
            set => SetProperty(ref _myStatus, value);
        }

        private int _myCount = 0;
        public MainWindowViewModel()
        {
            DoCommand = new DelegateCommand(() =>
            {
                var emp = new Employee();
                emp.UserId = (_myCount += 1);
                Repository.Do(emp,res=> Status = res?.UserId.ToString());
            });

            UndoCommand = new DelegateCommand(() =>
            {
                Repository.Undo(res =>
                {
                    Status = res?.UserId.ToString();
                });

            }, () => Repository.IsCanUndo);

            RedoCommand = new DelegateCommand(() =>
            {
                Repository.Redo(res =>
                {
                    Status = res?.UserId.ToString();
                });

            }, () => Repository.IsCanRedo);

            //CommonCommands Set.
            Repository.SetCommonCommand(DoCommand, nameof(DoCommand.RaiseCanExecuteChanged))
                .SetCommonCommand(RedoCommand, nameof(RedoCommand.RaiseCanExecuteChanged))
                .SetCommonCommand(UndoCommand, nameof(UndoCommand.RaiseCanExecuteChanged));

            // For each Do, Redo, Undo, set the command CanExecute to Raise
            Repository.SetIsCommonCommandsExecureRaise(true);
        }
    }
}
