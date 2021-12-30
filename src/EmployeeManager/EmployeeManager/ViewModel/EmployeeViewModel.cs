using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media.Imaging;
using EmployeeManager.Model;
using Microsoft.Toolkit.Mvvm.Input;

namespace EmployeeManager.ViewModel
{
    public class EmployeeViewModel : INotifyPropertyChanged
    {
        private readonly IEmployeeManager _manager;
        private readonly Employee _employee;
        private BitmapImage _image = null;

        public EmployeeViewModel(IEmployeeManager manager, Employee employee = null)
        {
            _manager = manager ?? throw new ArgumentNullException(nameof(manager));
            _employee = employee ?? new Employee();
            DeleteCommand = new RelayCommand(DeleteSelectedEmployee);
            EditCommand = new RelayCommand(EditActiveEmployee);
            SaveCommand = new RelayCommand(SaveActiveEmployee);
            RevertCommand = new RelayCommand(RevertSelectedEmployee);
        }

        public IRelayCommand DeleteCommand { get; }
        public IRelayCommand EditCommand { get; }
        public IRelayCommand SaveCommand { get; }
        public IRelayCommand RevertCommand { get; }

        public string Name
        {
            get => _employee?.Name;
            set
            {
                _employee.Name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public string JobTitle
        {
            get => _employee?.JobTitle;
            set
            {
                _employee.JobTitle = value;
                OnPropertyChanged(nameof(JobTitle));
            }
        }

        public BitmapImage Image
        {
            get
            {
                if (_image != null || _employee.Image == null || _employee.Image.Length == 0) return _image;
                _image = new BitmapImage();
                using (var mem = new MemoryStream(_employee.Image))
                {
                    mem.Position = 0;
                    _image.BeginInit();
                    _image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                    _image.CacheOption = BitmapCacheOption.OnLoad;
                    _image.UriSource = null;
                    _image.StreamSource = mem;
                    _image.EndInit();
                }

                _image.Freeze();
                return _image;
            }
        }

        public Visibility EditVisibility => !IsReadOnly ? Visibility.Visible : Visibility.Collapsed;
        public Visibility ReadOnlyVisibility => IsReadOnly ? Visibility.Visible : Visibility.Collapsed;

        private bool _readOnly = true;
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

        private void DeleteSelectedEmployee()
        {
            if (MessageBoxResult.Yes != MessageBox.Show($"Are you sure you'd like to delete {Name}?",
                "Delete Confirmation",
                MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No)) return;

            _manager.Delete(_employee);
            OnPropertyChanged(nameof(EditVisibility));
        }

        private void EditActiveEmployee()
        {
            IsReadOnly = false;
        }

        private void SaveActiveEmployee()
        {
            _manager.Save(_employee);
            IsReadOnly = true;
        }

        private void RevertSelectedEmployee()
        {
            _image = null;
            _manager.Revert(_employee);
            OnPropertyChanged(nameof(Name));
            OnPropertyChanged(nameof(JobTitle));
            OnPropertyChanged(nameof(Image));
            IsReadOnly = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
