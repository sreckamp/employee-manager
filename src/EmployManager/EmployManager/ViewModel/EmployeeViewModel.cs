using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Media.Imaging;
using EmployManager.Annotations;
using EmployManager.Model;

namespace EmployManager.ViewModel
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

        public void Delete()
        {
            _manager.Delete(_employee);
        }

        public void Save()
        {
            _manager.Save(_employee);
        }

        public void Revert()
        {
            _image = null;
            _manager.Revert(_employee);
            OnPropertyChanged(nameof(Name));
            OnPropertyChanged(nameof(JobTitle));
            OnPropertyChanged(nameof(Image));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
