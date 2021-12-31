using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Threading;
using EmployeeManager.Model;

namespace EmployeeManager.ViewModel
{
    public class NotificationViewModel : INotifyPropertyChanged
    {
        private readonly DispatcherTimer _timer = new() { Interval = TimeSpan.FromSeconds(8)};

        public NotificationViewModel(IEmployeeManager manager)
        {
            manager.EmployeeAdded += (_, employee) =>
            {
                NotificationText = $"Added {employee.Name}";
            };
            manager.EmployeeDeleted += (_, employee) =>
            {
                NotificationText = $"Deleted {employee.Name}";
            };
            manager.EmployeeUpdated += (_, employee) => NotificationText = $"Updated {employee.Name}";
            _timer.Tick += (_, _) =>
            {
                _timer.Stop();
                NotificationText = string.Empty;
            };
        }

        private string _notificationText;
        public string NotificationText
        {
            get => _notificationText ?? string.Empty;
            private set
            {
                _notificationText = value;
                if (!string.IsNullOrEmpty(value))
                {
                    _timer.Start();
                }
                OnPropertyChanged(nameof(NotificationText));
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