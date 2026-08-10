using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CRUD_Application.Models
{
    public class Student
    {

        [Key]
        public int StudentId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Range(18, 60)]
        public int Age { get; set; }

        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        //[MaxLength(50)]
        //public string Department { get; set; } = string.Empty;


        // Foreign Key
        [Required]
        public int DepartmentId { get; set; }

        // Navigation Property
        [ForeignKey(nameof(DepartmentId))]
        [JsonIgnore]
        public Department? Department { get; set; }
    }
}
