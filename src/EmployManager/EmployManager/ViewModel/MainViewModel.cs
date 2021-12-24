using System;
using System.Collections.Generic;
using System.Linq;
using EmployManager.Model;
using Microsoft.Toolkit.Mvvm.Input;

namespace EmployManager.ViewModel
{
    public class MainViewModel
    {
        private IEmployeeManager _manager;

        public MainViewModel(IEmployeeManager manager)
        {
            _manager = manager ?? throw new ArgumentNullException(nameof(manager));
            AddCommand = new RelayCommand<string>(AddEmployee);
            DeleteCommand = new RelayCommand<EmployeeViewModel>(DeleteEmployee);
            SaveCommand = new RelayCommand<EmployeeViewModel>(SaveEmployee);
        }

        public IEnumerable<EmployeeViewModel> Employees =>
            _manager.Employees.Select(employee => new EmployeeViewModel(employee));
        public IRelayCommand<string> AddCommand { get; }
        public IRelayCommand<EmployeeViewModel> DeleteCommand { get; }
        public IRelayCommand<EmployeeViewModel> SaveCommand { get; }

        private void AddEmployee(string name)
        {
            _manager.Add("", name);
        }
        
        private void DeleteEmployee(EmployeeViewModel employee)
        {
            // _manager.Delete();
        }

        private void SaveEmployee(EmployeeViewModel employee)
        {
            // _manager.Save();
        }
    }
}
