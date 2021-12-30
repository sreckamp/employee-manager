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
            ShowVerificationCommand = new RelayCommand(() => IsVerification = true);
            HideVerificationCommand = new RelayCommand(() => IsVerification = false);
        }

        public IRelayCommand EditCommand { get; }
        public IRelayCommand SaveCommand { get; }
        public IRelayCommand RevertCommand { get; }
        public IRelayCommand ShowVerificationCommand { get; }
        public IRelayCommand HideVerificationCommand { get; }
        public IRelayCommand DeleteCommand { get; }

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
            _manager.Delete(_employee);
            OnPropertyChanged(nameof(EditVisibility));
        }

        private bool _isVerification = false;

        private bool IsVerification
        {
            get => _isVerification;
            set
            {
                _isVerification = value;
                OnPropertyChanged(nameof(VerificationVisible));
            }
        }

        public Visibility VerificationVisible => IsVerification ? Visibility.Visible : Visibility.Collapsed;

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
