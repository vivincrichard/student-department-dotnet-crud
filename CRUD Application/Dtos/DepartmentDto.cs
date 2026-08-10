using System.ComponentModel.DataAnnotations;

namespace CRUD_Application.Dtos
{
    public class DepartmentDto
    {
        [Required(ErrorMessage = "Department Name is required.")]
        [StringLength(100)]
        public string DepartmentName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Department Code is required.")]
        [StringLength(10)]
        public string DepartmentCode { get; set; } = string.Empty;

        [StringLength(250)]
        public string? Description { get; set; }
    }
}
