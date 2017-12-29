using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Commands;
using WpfAppUndoRedoPattern.Models;
using Unre;
using Prism.Mvvm;

namespace WpfAppUndoRedoPattern.ViewModels
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

        public UnreRepository<Employee> Repository { get; set; } = UnreRepository<Employee>.Instance;

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
                RaiseEachCanExecute();

            });

            RedoCommand = new DelegateCommand(() =>
            {
                var emp = Repository.Redo();
                Status = emp.UserId.ToString();
                RaiseEachCanExecute();
            });

            UndoCommand = new DelegateCommand(() =>
            {
                var emp = Repository.Undo();
                Status = emp.UserId.ToString();
                RaiseEachCanExecute();

            }, ()=> Repository.IsCanUndo);
        }

        public MainWindowViewModel(DelegateCommand doCommand)
        {
            DoCommand = doCommand;
        }

        private void RaiseEachCanExecute()
        {
            UndoCommand?.RaiseCanExecuteChanged();
            RedoCommand?.RaiseCanExecuteChanged();
        }
    }
}
