using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
        private bool _readOnly = true;

        public MainViewModel(IEmployeeManager manager)
        {
            _manager = manager ?? throw new ArgumentNullException(nameof(manager));
            AddCommand = new RelayCommand(AddEmployee);
            DeleteSelectedCommand = new RelayCommand(DeleteSelectedEmployee);
            EditSelectedCommand = new RelayCommand(EditActiveEmployee);
            SaveCommand = new RelayCommand(SaveActiveEmployee);
            RevertCommand = new RelayCommand(RevertSelectedEmployee);
        }

        private EmployeeViewModel _active;
        private EmployeeViewModel _selected;

        public bool ShowingSelection => Selected == Active;
        public bool NotShowingSelection => !ShowingSelection;

        public EmployeeViewModel Selected
        {
            get => _selected;
            set
            {
                _selected = value;
                OnPropertyChanged(nameof(Active));
                OnPropertyChanged(nameof(Selected));
                OnPropertyChanged(nameof(ActiveVisibility));
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
                OnPropertyChanged(nameof(ActiveVisibility));
                OnPropertyChanged(nameof(ShowingSelection));
                OnPropertyChanged(nameof(NotShowingSelection));
            }
        }

        public Visibility ActiveVisibility => Active != null ? Visibility.Visible : Visibility.Hidden;
        public Visibility EditVisibility => !IsReadOnly ? Visibility.Visible : Visibility.Collapsed;
        public Visibility ReadOnlyVisibility => IsReadOnly ? Visibility.Visible : Visibility.Collapsed;

        public IEnumerable<EmployeeViewModel> Employees =>
            _manager.Employees.Select(employee => new EmployeeViewModel(_manager, employee));
        public IRelayCommand AddCommand { get; }
        public IRelayCommand DeleteSelectedCommand { get; }
        public IRelayCommand EditSelectedCommand { get; }
        public IRelayCommand SaveCommand { get; }
        public IRelayCommand RevertCommand { get; }

        public bool IsReadOnly
        {
            get => _readOnly;
            set
            {
                _readOnly = value;
                OnPropertyChanged(nameof(IsReadOnly));
                OnPropertyChanged(nameof(EditVisibility));
                OnPropertyChanged(nameof(ReadOnlyVisibility));
            }
        }

        private void AddEmployee()
        {
            Active = new EmployeeViewModel(_manager);
            IsReadOnly = false;
        }
        
        private void DeleteSelectedEmployee()
        {
            if (MessageBoxResult.Yes != MessageBox.Show($"Are you sure you'd like to delete {_selected.Name}?",
                "Delete Confirmation",
                MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No)) return;

            _selected.Delete();
            OnPropertyChanged(nameof(Employees));
            OnPropertyChanged(nameof(EditVisibility));
        }

        private void EditActiveEmployee()
        {
            IsReadOnly = false;
        }

        private void SaveActiveEmployee()
        {
            Active.Save();
            if (!ShowingSelection)
            {
                ToasterText = $"Added {Active.Name}";
                OnPropertyChanged(nameof(Employees));
            }
            else
            {
                ToasterText = $"Updated {Active.Name}";
            }
            IsReadOnly = true;
            Active = null;
        }

        private void RevertSelectedEmployee()
        {
            Active.Revert();
            Active = null;
            IsReadOnly = true;
        }


        private string _toasterText;
        public string ToasterText
        {
            get => _toasterText;
            set
            {
                Debug.WriteLine($"ToasterText:{_toasterText}=>{value}");
                _toasterText = value;
                OnPropertyChanged(nameof(ToasterText));
            }
        }
        
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
