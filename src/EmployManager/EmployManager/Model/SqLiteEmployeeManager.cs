using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace EmployManager.Model
{
    /// <summary>
    /// Employee manager that utilizes SqLite as the backing store.
    /// </summary>
    public class SqLiteEmployeeManager : IEmployeeManager, IDisposable
    {
        private readonly EmployeeDatabaseContext _context = new();
        private bool _disposed;

        public event EventHandler<Employee> EmployeeAdded;
        public event EventHandler<Employee> EmployeeDeleted;
        public event EventHandler<Employee> EmployeeUpdated;

        /// <inheritdoc/>
        public IEnumerable<Employee> Employees { get; }

        public SqLiteEmployeeManager()
        {
            Employees = _context.Employees;
        }
        
        /// <inheritdoc/>
        public void Initialize()
        {
            _context.Database.EnsureCreated();
            if (_context.Employees.Any()) return;

            Debug.WriteLine("Loading defaults");
            Add("Co-Manager", "Jim Halpert", "images/jim.png");
            Add("Co-Manager", "Michael Scott", "images/michael.png");
            Add("Secretary", "Erin Hannon", "images/erin.jpg");
            Add("Customer Service", "Kelly Kapoor");
            Add("Sales", "Pam Halpert", "images/pam.jpg");
            Add("Sales", "Stanley Hudson");
            Add("Sales", "Phyllis Vance");
            Add("Sales", "Dwight Bratton");
            Add("Sales", "Andy Bernard", "images/andy.jpg");
            Add("Quality Assurance", "Creed Bratton");
            Add("Human Resources", "Toby Flenderson");
            Add("Accounting", "Angela Martin", "images/angela.jpg");
            Add("Accounting", "Kevin Malone");
            Add("Accounting", "Oscar Martinez");
            Add("Temp", "Ryan Howard", "images/ryan.png");
        }

        private void Add(string title, string name, string imagePath = null)
        {
            CheckDisposed();
            byte[] imageData = null;
            if (imagePath != null && File.Exists(imagePath))
            {
                imageData = File.ReadAllBytes(imagePath);
            }
            var newEmployee = new Employee {JobTitle = title, Name = name, Image = imageData};
            _context.Add(newEmployee);
            _context.SaveChanges();
        }

        /// <inheritdoc/>
        public void Delete(Employee employee)
        {
            CheckDisposed();
            _context.Remove(employee);
            _context.SaveChanges();
            EmployeeDeleted?.Invoke(this, employee);
        }

        /// <inheritdoc/>
        public void Save(Employee employee)
        {
            CheckDisposed();
            var added = !Employees.Contains(employee);
            if (added)
            {
                _context.Add(employee);
            }
            _context.SaveChanges();
            if (added)
            {
                EmployeeAdded?.Invoke(this, employee);
            }
            else
            {
                EmployeeUpdated?.Invoke(this, employee);
            }
        }

        /// <inheritdoc/>
        public void Revert(Employee employee)
        {
            CheckDisposed();
            _context.Entry(employee).Reload();
        }

        private void CheckDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().ShortDisplayName(), CoreStrings.ContextDisposed);
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            if (_disposed) return;
            _context.Dispose();
            _disposed = true;
        }
    }
}
