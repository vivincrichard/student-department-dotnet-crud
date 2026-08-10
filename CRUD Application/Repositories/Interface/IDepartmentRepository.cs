using CRUD_Application.Models;

namespace CRUD_Application.Repositories
{
    namespace CollegeManagement.Repositories
    {
        public interface IDepartmentRepository
        {
            Task<List<Department>> GetAllAsync();

            Task<Department?> GetByIdAsync(int id);

            Task<Department?> GetByCodeAsync(string code);

            Task<Department> AddAsync(Department department);

            Task UpdateAsync(Department department);

            Task DeleteAsync(Department department);
        }
    }
}
