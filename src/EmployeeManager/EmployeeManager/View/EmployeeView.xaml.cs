using System.Windows;
using System.Windows.Media;

namespace EmployeeManager.View
{
    public partial class EmployeeView
    {
        public EmployeeView()
        {
            InitializeComponent();
        }

        public Brush Fill
        {
            get { return (Brush)GetValue(FillProperty); }
            set { SetValue(FillProperty, value); }
        }

        public static readonly DependencyProperty FillProperty = DependencyProperty.Register("Fill", typeof(Brush), 
            typeof(EmployeeView), new PropertyMetadata(new SolidColorBrush(Colors.White)));
        
        public double Bar
        {
            get { return (double)GetValue(BarProperty); }
            set { SetValue(BarProperty, value); }
        }

        public static readonly DependencyProperty BarProperty = DependencyProperty.Register("Bar", typeof(double), 
            typeof(EmployeeView), new PropertyMetadata(10.0));
        
        public double Column
        {
            get { return (double)GetValue(ColumnProperty); }
            set { SetValue(ColumnProperty, value); }
        }

        public static readonly DependencyProperty ColumnProperty = DependencyProperty.Register("Column", typeof(double), 
            typeof(EmployeeView), new PropertyMetadata(20.0));
        
        public double InnerArcRadius
        {
            get { return (double)GetValue(InnerArcRadiusProperty); }
            set { SetValue(InnerArcRadiusProperty, value); }
        }

        public static readonly DependencyProperty InnerArcRadiusProperty = DependencyProperty.Register("InnerArcRadius", typeof(double), 
            typeof(EmployeeView), new PropertyMetadata(20.0));
    }
}