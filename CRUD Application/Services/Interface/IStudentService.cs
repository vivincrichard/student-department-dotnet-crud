using CRUD_Application.Dtos;
using CRUD_Application.Scaffolded.Models;
//using CRUD_Application.Models;

namespace CRUD_Application.Services.Interface
{
    public interface IStudentService
    {
        Task<List<Student>> GetAllStudentsAsync(
            string? embed = null);

        Task<Student?> GetStudentByIdAsync(
            int id,
            string? embed = null);

        Task<Student> CreateStudentAsync(StudentDto dto);

        Task<Student> UpdateStudentAsync(int id, StudentDto dto);

        Task<bool> DeleteStudentAsync(int id);
    }
}
