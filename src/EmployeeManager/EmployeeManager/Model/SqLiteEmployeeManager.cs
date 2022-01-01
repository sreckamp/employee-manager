using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace EmployeeManager.Model
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
            Add("Q", "Q", "images/Q.jpg");
            Add("Captain", "Jean-Luc Picard", "images/Picard.jpg");
            Add("Commander", "William T. Riker", "images/Riker.jpg");
            Add("Commander", "Dr. Beverly Crusher", "images/DrCrusher.jpg");
            Add("Commander", "Deanna Troi", "images/Troi.jpg");
            Add("Lieutenant Commander", "Data", "images/Data.jpg");
            Add("Lieutenant Commander", "Geordi La Forge", "images/LaForge.jpg");
            Add("Lieutenant Commander", "Worf", "images/Worf.jpg");
            Add("Lieutenant", "Natasha Yar", "images/Yar.jpg");
            Add("First Petty Officer", "Miles O'Brien", "images/OBrien.jpg");
            Add("Ensign", "Wesley Crusher", "images/Crusher.jpg");
            Add("Lounge Hostess", "Guinan", "images/Guinan.jpg");
            Add("Expendable", "Red Shirt");
        }

        private void Add(string title, string name, string imagePath = null)
        {
            CheckDisposed();
            var imageData = Employee.ReadImageFile(imagePath);
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
