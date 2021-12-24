using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using EmployManager.Model;
using Ninject;

namespace EmployManager
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        // private IKernel _iocKernel;
        protected override void OnStartup(StartupEventArgs startupEvent)
        {
            base.OnStartup(startupEvent);
            IEmployeeManager manager = new SqLiteEmployeeManager();
            manager.Initialize();
            Debug.WriteLine("::Initialized");
            foreach (var e in manager.Employees)
            {
                Debug.WriteLine($"{e.JobTitle} {e.Name} [{e.Id}] [{e.Image?.Length ?? 0} bytes]");
            }
            // _iocKernel = new StandardKernel();
        }
    }
}