using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Windows.Media.Imaging;

namespace EmployManager.Model
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
