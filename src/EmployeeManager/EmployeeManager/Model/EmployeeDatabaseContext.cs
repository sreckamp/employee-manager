using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManager.Model
{
    public class EmployeeDatabaseContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=Employees.db", options =>
            {
                options.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName);
            });
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>().ToTable("employees", "employee-manager");
            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasKey(employee => employee.Id);
            });
            base.OnModelCreating(modelBuilder);
        }
    }
}