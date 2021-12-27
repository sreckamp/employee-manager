using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using EmployManager.Annotations;
using EmployManager.Model;
using Microsoft.Toolkit.Mvvm.Input;

namespace EmployManager.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly IEmployeeManager _manager;

        public MainViewModel(IEmployeeManager manager)
        {
            _manager = manager ?? throw new ArgumentNullException(nameof(manager));
            AddCommand = new RelayCommand<string>(AddEmployee);
            DeleteSelectedCommand = new RelayCommand(DeleteSelectedEmployee);
            SaveCommand = new RelayCommand<EmployeeViewModel>(SaveEmployee);
        }

        private EmployeeViewModel _selected;

        public EmployeeViewModel Selected
        {
            get => _selected;
            set
            {
                _selected = value;
                OnPropertyChanged(nameof(DisplayEmployeeVisibility));
            }
        }

        public Visibility DisplayEmployeeVisibility => Selected != null ? Visibility.Visible : Visibility.Hidden;
        public Visibility EditEmployeeVisibility => Selected != null ? Visibility.Visible : Visibility.Hidden;

        public IEnumerable<EmployeeViewModel> Employees =>
            _manager.Employees.Select(employee => new EmployeeViewModel(_manager, employee));
        public IRelayCommand<string> AddCommand { get; }
        public IRelayCommand DeleteSelectedCommand { get; }
        public IRelayCommand<EmployeeViewModel> SaveCommand { get; }

        private void AddEmployee(string name)
        {
            _manager.Add("", name);
        }
        
        private void DeleteSelectedEmployee()
        {
            _selected.Delete();
            OnPropertyChanged(nameof(Employees));
        }

        private void SaveEmployee(EmployeeViewModel employee)
        {
            // _manager.Save();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
