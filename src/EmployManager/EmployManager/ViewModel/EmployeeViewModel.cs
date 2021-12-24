using System.IO;
using System.Windows.Media.Imaging;
using EmployManager.Model;

namespace EmployManager
{
    public class EmployeeViewModel
    {
        private readonly Employee _employee;
        private BitmapImage _image = null;

        public EmployeeViewModel(Employee employee)
        {
            _employee = employee;
        }

        public string Name => _employee?.Name;
        public string JobTitle => _employee?.JobTitle;

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
    }
}
