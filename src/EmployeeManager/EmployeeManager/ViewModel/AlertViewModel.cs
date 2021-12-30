using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Threading;
using EmployeeManager.Model;

namespace EmployeeManager.ViewModel
{
    public class AlertViewModel : INotifyPropertyChanged
    {
        public AlertViewModel(IEmployeeManager manager)
        {
            manager.EmployeeAdded += (_, employee) =>
            {
                AlertText = $"Added {employee.Name}";
            };
            manager.EmployeeDeleted += (_, employee) =>
            {
                AlertText = $"Deleted {employee.Name}";
            };
            manager.EmployeeUpdated += (_, employee) => AlertText = $"Updated {employee.Name}";
            _timer.Tick += (_, _) =>
            {
                _timer.Stop();
                AlertText = string.Empty;
            };
        }

        private string _alertText;
        private DispatcherTimer _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(8)};

        public string AlertText
        {
            get => _alertText;
            private set
            {
                Debug.WriteLine($"AlertText Updated: {_alertText}=>{value}");
                _alertText = value;
                if (!string.IsNullOrEmpty(value))
                {
                    _timer.Start();
                }
                OnPropertyChanged(nameof(AlertText));
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