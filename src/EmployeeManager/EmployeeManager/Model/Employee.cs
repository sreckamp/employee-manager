using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeManager.Model
{
    public class Employee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        // ReSharper disable once UnassignedGetOnlyAutoProperty
        public int Id { get; }
        [Required]
        public string JobTitle { get; set; }
        [Required]
        public string Name { get; set; }
        public byte[] Image { get; set; }
    }
}
