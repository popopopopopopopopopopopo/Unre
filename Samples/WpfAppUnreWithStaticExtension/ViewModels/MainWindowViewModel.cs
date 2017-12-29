using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Commands;
using WpfAppUnreWithStaticExtension.Models;
using Unre;
using Prism.Mvvm;

namespace WpfAppUnreWithStaticExtension.ViewModels
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

        public Employee CurrentEmployee { get; set; }

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
                CurrentEmployee = new Employee();
                CurrentEmployee.UserId = (_myCount += 1);
                CurrentEmployee.Do();
                Status = CurrentEmployee.UserId.ToString();
                RaiseEachCanExecute();
            });

            RedoCommand = new DelegateCommand(() =>
            {
                CurrentEmployee = CurrentEmployee.Redo();
                Status = CurrentEmployee.UserId.ToString();
                RaiseEachCanExecute();

            },()=> CurrentEmployee != null && CurrentEmployee.IsCanRedo());

            UndoCommand = new DelegateCommand(() =>
            {
                CurrentEmployee = CurrentEmployee.Undo();
                Status = CurrentEmployee.UserId.ToString();
                RaiseEachCanExecute();
            },()=> CurrentEmployee != null && CurrentEmployee.IsCanUndo());
        }

        private void RaiseEachCanExecute()
        {
            UndoCommand?.RaiseCanExecuteChanged();
            RedoCommand?.RaiseCanExecuteChanged();
        }
    }
}
