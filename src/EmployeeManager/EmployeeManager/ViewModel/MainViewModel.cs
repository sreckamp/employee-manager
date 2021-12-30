using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using EmployeeManager.Model;
using Microsoft.Toolkit.Mvvm.Input;

namespace EmployeeManager.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly IEmployeeManager _manager;

        public MainViewModel(IEmployeeManager manager)
        {
            _manager = manager ?? throw new ArgumentNullException(nameof(manager));
            _manager.EmployeeAdded += ListChanged;
            _manager.EmployeeDeleted += ListChanged;
            _manager.EmployeeUpdated += CloseEmployeeView;
            _manager.EmployeeReverted += CloseEmployeeView;
            AlertViewModel = new AlertViewModel(_manager);
            AddCommand = new RelayCommand(AddEmployee);
            MinimizeWindowCommand = new RelayCommand(() => WindowState = WindowState.Minimized);
            MaximizeWindowCommand = new RelayCommand(() => WindowState = WindowState == WindowState.Maximized
                ? WindowState.Normal
                : WindowState.Maximized);
            ShutdownCommand = new RelayCommand(() => Application.Current.MainWindow.Close());
        }

        public IRelayCommand MinimizeWindowCommand { get; }
        public IRelayCommand MaximizeWindowCommand { get; }
        public IRelayCommand ShutdownCommand { get; }

        private WindowState _windowState = WindowState.Maximized;
        public WindowState WindowState
        {
            get => _windowState;
            set
            {
                _windowState = value;
                OnPropertyChanged(nameof(WindowState));
            }
        }
        private void CloseEmployeeView(object sender, Employee e)
        {
            Debug.WriteLine("Close View");
            Active = null;
            Selected = null;
        }

        private void ListChanged(object sender, Employee e)
        {
            Debug.WriteLine("List Changed");
            OnPropertyChanged(nameof(Employees));
            CloseEmployeeView(sender, e);
        }

        private EmployeeViewModel _active;
        private EmployeeViewModel _selected;

        public bool ShowingSelection => Selected == Active;
        public bool NotShowingSelection => !ShowingSelection;

        public AlertViewModel AlertViewModel { get; }

        public EmployeeViewModel Selected
        {
            get => _selected;
            set
            {
                _selected = value;
                OnPropertyChanged(nameof(Active));
                OnPropertyChanged(nameof(Selected));
                OnPropertyChanged(nameof(ListVisibility));
                OnPropertyChanged(nameof(EmployeeVisibility));
                OnPropertyChanged(nameof(ShowingSelection));
                OnPropertyChanged(nameof(NotShowingSelection));
            }
        }

        public EmployeeViewModel Active
        {
            get => _active ?? _selected;
            set
            {
                _active = value;
                OnPropertyChanged(nameof(Active));
                OnPropertyChanged(nameof(ListVisibility));
                OnPropertyChanged(nameof(EmployeeVisibility));
                OnPropertyChanged(nameof(ShowingSelection));
                OnPropertyChanged(nameof(NotShowingSelection));
            }
        }

        public Visibility ListVisibility => Active == null ? Visibility.Visible : Visibility.Collapsed;
        public Visibility EmployeeVisibility => Active != null ? Visibility.Visible : Visibility.Collapsed;

        public IEnumerable<EmployeeViewModel> Employees =>
            _manager.Employees.Select(employee => new EmployeeViewModel(_manager, employee));
        public IRelayCommand AddCommand { get; }

        private void AddEmployee()
        {
            Active = new EmployeeViewModel(_manager);
            Active.IsReadOnly = false;
        }
        
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
