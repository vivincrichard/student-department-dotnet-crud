using System.ComponentModel.DataAnnotations;

namespace CRUD_Application.Dtos
{
    public class StudentDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        public int Age { get; set; }

        [Required]
        public string Email { get; set; } = string.Empty;

        //public string Department { get; set; } = string.Empty;


        [Required]
        public int DepartmentId { get; set; }
    }
}
