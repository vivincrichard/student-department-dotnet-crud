using CRUD_Application.Dtos;
using CRUD_Application.Models;
using CRUD_Application.Repositories.CollegeManagement.Repositories;
using CRUD_Application.Services.Interface;

namespace CRUD_Application.Services
{

    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _repository;

        public DepartmentService(IDepartmentRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Department>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Department?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<Department> CreateAsync(DepartmentDto dto)
        {
            var existingDepartment =
                await _repository.GetByCodeAsync(dto.DepartmentCode);

            if (existingDepartment != null)
                throw new Exception("Department Code already exists.");

            var department = new Department
            {
                DepartmentName = dto.DepartmentName.Trim(),
                DepartmentCode = dto.DepartmentCode.Trim().ToUpper(),
                Description = dto.Description
            };

            return await _repository.AddAsync(department);
        }

        public async Task<bool> UpdateAsync(int id, DepartmentDto dto)
        {
            var department = await _repository.GetByIdAsync(id);

            if (department == null)
                return false;

            department.DepartmentName = dto.DepartmentName.Trim();
            department.DepartmentCode = dto.DepartmentCode.Trim().ToUpper();
            department.Description = dto.Description;

            await _repository.UpdateAsync(department);

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var department = await _repository.GetByIdAsync(id);

            if (department == null)
                return false;

            await _repository.DeleteAsync(department);

            return true;
        }
    }
}
