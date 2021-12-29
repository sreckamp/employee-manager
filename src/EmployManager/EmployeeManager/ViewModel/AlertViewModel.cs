using System.ComponentModel;
using System.Runtime.CompilerServices;
using EmployManager.Annotations;
using EmployManager.Model;

namespace EmployManager.ViewModel
{
    public class AlertViewModel : INotifyPropertyChanged
    {
        public AlertViewModel(IEmployeeManager manager)
        {
            manager.EmployeeAdded += (_, employee) => AlertText = $"Added {employee.Name}";
            manager.EmployeeDeleted += (_, employee) => AlertText = $"Deleted {employee.Name}";
            manager.EmployeeUpdated += (_, employee) => AlertText = $"Updated {employee.Name}";
        }

        private string _alertText;

        public string AlertText
        {
            get => _alertText;
            private set
            {
                _alertText = value;
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