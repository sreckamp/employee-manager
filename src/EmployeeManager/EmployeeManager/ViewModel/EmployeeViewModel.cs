using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using EmployeeManager.Model;
using Microsoft.Toolkit.Mvvm.Input;

namespace EmployeeManager.ViewModel
{
    public class EmployeeViewModel : INotifyPropertyChanged
    {
        private readonly IEmployeeManager _manager;
        private readonly Employee _employee;
        private BitmapImage _image;
        private readonly DispatcherTimer _timer = new() { Interval = TimeSpan.FromSeconds(8)};

        public EventHandler EditCanceled;
        public EventHandler ViewClosed;

        private readonly string[] _imageFormats = new[]
        {
            ".bmp",
            ".jpg",
            ".png"
        };

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
            DropImageCommand = new RelayCommand<DragEventArgs>(DropImage);
            _timer.Tick += (_, _) => HideWarnings();
        }

        private void DropImage(DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop)) return;
            var files = (string[])e.Data.GetData(DataFormats.FileDrop) ?? Array.Empty<string>();
            var newImage = Employee.ReadImageFile(
                files.FirstOrDefault(path => _imageFormats.Contains(Path.GetExtension(path).ToLower())));
            if(newImage == null) return;
            _employee.Image = newImage;
            _image = null;
            OnPropertyChanged(nameof(Image));
        }

        public IRelayCommand EditCommand { get; }
        public IRelayCommand SaveCommand { get; }
        public IRelayCommand RevertCommand { get; }
        public IRelayCommand ShowVerificationCommand { get; }
        public IRelayCommand HideVerificationCommand { get; }
        public IRelayCommand DeleteCommand { get; }
        public IRelayCommand<DragEventArgs> DropImageCommand { get; }

        private Visibility _nameWarningVisibility = Visibility.Hidden;
        public Visibility NameWarningVisibility
        {
            get => _nameWarningVisibility;
            set
            {
                _nameWarningVisibility = value;
                if (value == Visibility.Visible)
                {
                    _timer.Start();
                }
                OnPropertyChanged(nameof(NameWarningVisibility));
            }
        }

        private Visibility _titleWarningVisibility = Visibility.Hidden;
        public Visibility TitleWarningVisibility
        {
            get => _titleWarningVisibility;
            set
            {
                _titleWarningVisibility = value;
                if (value == Visibility.Visible)
                {
                    _timer.Start();
                }
                OnPropertyChanged(nameof(TitleWarningVisibility));
            }
        }

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

        private void HideWarnings()
        {
            _timer.Stop();
            NameWarningVisibility = Visibility.Hidden;
            TitleWarningVisibility = Visibility.Hidden;
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

        private bool _isVerification;

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
            var save = true;
            if (string.IsNullOrEmpty(Name))
            {
                NameWarningVisibility = Visibility.Visible;
                save = false;
            }
            if (string.IsNullOrEmpty(JobTitle))
            {
                TitleWarningVisibility = Visibility.Visible;
                save = false;
            }

            if (!save) return;

            HideWarnings();
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
            (IsReadOnly ? ViewClosed : EditCanceled)?.Invoke(this, EventArgs.Empty);
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
