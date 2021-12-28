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
        /// Delete the given employee
        /// </summary>
        /// <param name="employee">The employee to delete</param>
        void Delete(Employee employee);

        /// <summary>
        /// Save the changes to the employee
        /// </summary>
        /// <param name="employee">The employee to save</param>
        void Save(Employee employee);

        /// <summary>
        /// Revert the changes to the employee
        /// </summary>
        /// <param name="employee">The employee to revert</param>
        void Revert(Employee employee);
    }
}
