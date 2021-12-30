using System;
using System.Diagnostics;
using System.Windows;
using EmployeeManager.Model;
using EmployeeManager.ViewModel;
using Microsoft.Extensions.DependencyInjection;

namespace EmployeeManager
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            Services = ConfigureServices();
        }

        // private IKernel _iocKernel;
        protected override void OnStartup(StartupEventArgs startupEvent)
        {
            base.OnStartup(startupEvent);
            // _iocKernel = new StandardKernel();
        }
        
        public IServiceProvider Services { get; }

        private static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();
            services.AddSingleton<IEmployeeManager>(provider =>
            {
                var manager = new SqLiteEmployeeManager();
                manager.Initialize();
                Debug.WriteLine("::Initialized");
                foreach (var e in manager.Employees)
                {
                    Debug.WriteLine($"{e.JobTitle} {e.Name} [{e.Id}] [{e.Image?.Length ?? 0} bytes]");
                }
                return manager;
            });
            services.AddSingleton<MainViewModel>();
            return services.BuildServiceProvider();
        }

        public MainViewModel MainViewModel => Services.GetService<MainViewModel>();
        
        public new static App Current => (App) Application.Current;
    }
}