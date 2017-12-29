using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Commands;
using Prism.Mvvm;
using WpfAppUnreWithCommonCommandsRaise.Models;
using Unre;

namespace WpfAppUnreWithCommonCommandsRaise.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private static MainWindowViewModel _myInstance = new MainWindowViewModel();

        public static MainWindowViewModel Instance
        {
            get => _myInstance;
            set => _myInstance = value;
        }

        private ObservableCollection<Employee> _myEmployees = Employee.GetSamples();
        public ObservableCollection<Employee> Employees
        {
            get => _myEmployees;
            set => SetProperty(ref _myEmployees, value);
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

            //CommonCommands Set.
            Repository.SetCommonCommand(DoCommand, nameof(DoCommand.RaiseCanExecuteChanged))
                .SetCommonCommand(RedoCommand, nameof(RedoCommand.RaiseCanExecuteChanged))
                .SetCommonCommand(UndoCommand, nameof(UndoCommand.RaiseCanExecuteChanged));

            // For each Do, Redo, Undo, set the command CanExecute to Raise
            Repository.SetIsCommonCommandsExecureRaise(true);

        }
    }
}
