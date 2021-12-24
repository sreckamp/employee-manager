using System.Collections.Generic;

namespace EmployManager.Model
{
    /// <summary>
    /// Manages the state of the set of employees
    /// </summary>
    public interface IEmployeeManager
    {
        /// <summary>
        /// The list of active employees
        /// </summary>
        IEnumerable<Employee> Employees { get; }

        /// <summary>
        /// Initialize the data source
        /// </summary>
        void Initialize();

        /// <summary>
        /// Add a new employee
        /// </summary>
        /// <param name="title">The job title of the employee</param>
        /// <param name="name">The name of the employee</param>
        /// <param name="imagePath">A path to an image of the employee</param>
        /// <returns>The newly created employee</returns>
        Employee Add(string title, string name, string imagePath = null);

        /// <summary>
        /// Delete the given employee
        /// </summary>
        /// <param name="employee">The employee to delete</param>
        void Delete(Employee employee);

        /// <summary>
        /// Save the changes to the employee
        /// </summary>
        /// <param name="employee">The employee to save</param>
        void Save(Employee employee);
    }
}
