using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace EmployeeManager.View
{
    public partial class ToasterAlert : UserControl
    {
        public ToasterAlert()
        {
            InitializeComponent();

            Toaster.DataContext = this;
        }
    
        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        /// <summary>
        /// Identified the Label dependency property
        /// </summary>
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string),
                typeof(ToasterAlert), new PropertyMetadata(""));
    }
}