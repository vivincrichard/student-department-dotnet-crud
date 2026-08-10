using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CRUD_Application.Models
{
    public class Department
    {
        [Key]
        public int DepartmentId { get; set; }

        [Required(ErrorMessage = "Department Name is required.")]
        [StringLength(100, ErrorMessage = "Department Name cannot exceed 100 characters.")]
        public string DepartmentName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Department Code is required.")]
        [StringLength(10)]
        public string DepartmentCode { get; set; } = string.Empty;

        [StringLength(250)]
        public string? Description { get; set; }


        // Navigation Property (One Department -> Many Students)
        [JsonIgnore]
        public ICollection<Student> Students { get; set; } = new List<Student>();
    }
}
