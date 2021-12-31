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
            NotificationViewModel = new NotificationViewModel(_manager);
            AddCommand = new RelayCommand(AddEmployee);
            ToggleAboutCommand = new RelayCommand(ToggleAbout);
            MinimizeWindowCommand = new RelayCommand(() => WindowState = WindowState.Minimized);
            MaximizeWindowCommand = new RelayCommand(() => WindowState = WindowState == WindowState.Maximized
                ? WindowState.Normal
                : WindowState.Maximized);
            ShutdownCommand = new RelayCommand(() => Application.Current.MainWindow.Close());
        }

        public IRelayCommand MinimizeWindowCommand { get; }
        public IRelayCommand MaximizeWindowCommand { get; }
        public IRelayCommand ShutdownCommand { get; }
        public IRelayCommand ToggleAboutCommand { get; }

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
        private void CloseEmployeeView(object sender, EventArgs e)
        {
            Debug.WriteLine("Close");
            Active = null;
            Selected = null;
        }

        private void ListChanged(object sender, Employee e)
        {
            OnPropertyChanged(nameof(Employees));
            CloseEmployeeView(sender, EventArgs.Empty);
        }

        private void ToggleAbout()
        {
            DisplayAbout = !DisplayAbout;
            OnPropertyChanged(nameof(DisplayAbout));
        }

        public bool DisplayAbout { get; private set; }

        public NotificationViewModel NotificationViewModel { get; }

        private EmployeeViewModel _selected;
        public EmployeeViewModel Selected
        {
            get => _selected;
            set
            {
                BeforeActiveChange();
                _selected = value;
                AfterActiveChange();
                OnPropertyChanged(nameof(Selected));
            }
        }

        private EmployeeViewModel _active;
        public EmployeeViewModel Active
        {
            get => _active ?? _selected;
            set
            {
                BeforeActiveChange();
                _active = value;
                AfterActiveChange();
            }
        }

        private void BeforeActiveChange()
        {
            if (_active != null)
            {
                _active.EditCanceled -= CloseEmployeeView;
            }
            if (Active == null) return;
            Active.ViewClosed -= CloseEmployeeView;
        }
        
        private void AfterActiveChange()
        {
            OnPropertyChanged(nameof(Active));
            if (_active != null)
            {
                _active.EditCanceled += CloseEmployeeView;
            }
            if (Active == null) return;
            Active.ViewClosed += CloseEmployeeView;
        }
        
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
