using CRUD_Application.Dtos;
using CRUD_Application.Models;

namespace CRUD_Application.Services.Interface
{
    public interface IStudentService
    {
        Task<List<Student>> GetAllStudentsAsync();

        Task<Student?> GetStudentByIdAsync(int id);

        Task<Student> CreateStudentAsync(StudentDto dto);

        Task<Student> UpdateStudentAsync(int id, StudentDto dto);

        Task<bool> DeleteStudentAsync(int id);
    }
}
