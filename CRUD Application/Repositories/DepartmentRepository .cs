//using CRUD_Application.Data;
//using CRUD_Application.Models;
using CRUD_Application.Repositories.CollegeManagement.Repositories;
using CRUD_Application.Scaffolded.Data;
using CRUD_Application.Scaffolded.Models;
using Microsoft.EntityFrameworkCore;

namespace CRUD_Application.Repositories
{

    public class DepartmentRepository : IDepartmentRepository
    {

        //Code First Approach
        //private readonly ApplicationDbContext _context;

        //public DepartmentRepository(ApplicationDbContext context)
        //{
        //    _context = context;
        //}

        //DB first Approach
        private readonly CollegeDbContext _context;

        public DepartmentRepository(CollegeDbContext context)
        {
            _context = context;
        }

        public async Task<List<Department>> GetAllAsync()
        {
            return await _context.Departments
                                 .OrderBy(d => d.DepartmentName)
                                 .ToListAsync();
        }

        public async Task<Department?> GetByIdAsync(int id)
        {
            return await _context.Departments.FindAsync(id);
        }

        public async Task<Department?> GetByCodeAsync(string code)
        {
            return await _context.Departments
                                 .FirstOrDefaultAsync(x => x.DepartmentCode == code);
        }

        public async Task<Department> AddAsync(Department department)
        {
            await _context.Departments.AddAsync(department);
            await _context.SaveChangesAsync();

            return department;
        }

        public async Task UpdateAsync(Department department)
        {
            _context.Departments.Update(department);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Department department)
        {
            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();
        }
    }
}
