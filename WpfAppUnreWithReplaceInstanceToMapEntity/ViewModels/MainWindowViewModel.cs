using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Mvvm;
using Unre;
using WpfAppUnreWithReplaceInstanceToMapEntity.Models;

namespace WpfAppUnreWithReplaceInstanceToMapEntity.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private static MainWindowViewModel _myInstance = new MainWindowViewModel();

        public static MainWindowViewModel Instance
        {
            get => _myInstance;
            set => _myInstance = value;
        }

        public DelegateCommand DoCommand { get; set; }

        public DelegateCommand UndoCommand { get; set; }

        public DelegateCommand RedoCommand { get; set; }

        public UnreRepository<Employee> Repository { get; set; } = new UnreRepository<Employee>();

        private Employee _myCurrentEmployee = new Employee();

        public Employee CurrentEmployee
        {
            get => _myCurrentEmployee;
            set => SetProperty(ref _myCurrentEmployee, value);
        }

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
                Repository.Do(ref _myCurrentEmployee, emp);
                Status = CurrentEmployee.UserId.ToString();
                RaiseEachCanExecute();
            });

            RedoCommand = new DelegateCommand(() =>
            {
                Repository.Redo(ref _myCurrentEmployee);
                Status = CurrentEmployee.UserId.ToString();
                RaiseEachCanExecute();
            }, () => Repository.IsCanRedo);

            UndoCommand = new DelegateCommand(() =>
            {
                Repository.Undo(ref _myCurrentEmployee);
                Status = CurrentEmployee.UserId.ToString();
                RaiseEachCanExecute();
            }, () => Repository.IsCanUndo);
        }

        private void RaiseEachCanExecute()
        {
            UndoCommand?.RaiseCanExecuteChanged();
            RedoCommand?.RaiseCanExecuteChanged();
        }
    }
}
