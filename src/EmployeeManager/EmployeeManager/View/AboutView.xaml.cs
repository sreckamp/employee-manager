using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace EmployeeManager.View
{
    public partial class AboutView
    {
        public AboutView()
        {
            InitializeComponent();
        }
        
        public Brush Fill
        {
            get { return (Brush)GetValue(FillProperty); }
            set { SetValue(FillProperty, value); }
        }

        public static readonly DependencyProperty FillProperty = DependencyProperty.Register("Fill", typeof(Brush), 
            typeof(AboutView), new PropertyMetadata(new SolidColorBrush(Colors.White)));
        
        public double Bar
        {
            get { return (double)GetValue(BarProperty); }
            set { SetValue(BarProperty, value); }
        }

        public static readonly DependencyProperty BarProperty = DependencyProperty.Register("Bar", typeof(double), 
            typeof(AboutView), new PropertyMetadata(10.0));
        
        public double LeftColumn
        {
            get { return (double)GetValue(LeftColumnProperty); }
            set { SetValue(LeftColumnProperty, value); }
        }

        public static readonly DependencyProperty LeftColumnProperty = DependencyProperty.Register("LeftColumn", typeof(double), 
            typeof(AboutView), new PropertyMetadata(20.0));
        
        public double RightColumn
        {
            get { return (double)GetValue(RightColumnProperty); }
            set { SetValue(RightColumnProperty, value); }
        }

        public static readonly DependencyProperty RightColumnProperty = DependencyProperty.Register("RightColumn", typeof(double), 
            typeof(AboutView), new PropertyMetadata(20.0));
        
        public double InnerArcRadius
        {
            get { return (double)GetValue(InnerArcRadiusProperty); }
            set { SetValue(InnerArcRadiusProperty, value); }
        }

        public static readonly DependencyProperty InnerArcRadiusProperty = DependencyProperty.Register("InnerArcRadius", typeof(double), 
            typeof(AboutView), new PropertyMetadata(20.0, InnerArcRadiusChanged));

        public static readonly DependencyProperty TopSpaceMarginProperty = DependencyProperty.Register("TopSpaceMargin", typeof(Thickness), 
            typeof(AboutView), new PropertyMetadata(new Thickness(0,20.0,0,0)));

        public static readonly DependencyProperty BottomSpaceMarginProperty = DependencyProperty.Register("BottomSpaceMargin", typeof(Thickness), 
            typeof(AboutView), new PropertyMetadata(new Thickness(0,0,0,20.0)));

        private static void InnerArcRadiusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Debug.WriteLine("Update InnerArcRadius");
            var space = (double) e.NewValue;
            d.SetValue(TopSpaceMarginProperty, new Thickness(0, space, 0, 0));
            d.SetValue(BottomSpaceMarginProperty, new Thickness(0, 0, 0, space));
        }
    }
}