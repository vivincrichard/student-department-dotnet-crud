using CRUD_Application.Dtos;
using CRUD_Application.Models;

namespace CRUD_Application.Services.Interface
{
    public interface IDepartmentService
    {
        Task<List<Department>> GetAllAsync();

        Task<Department?> GetByIdAsync(int id);

        Task<Department> CreateAsync(DepartmentDto dto);

        Task<bool> UpdateAsync(int id, DepartmentDto dto);

        Task<bool> DeleteAsync(int id);
    }
}
