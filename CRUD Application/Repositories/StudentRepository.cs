using CRUD_Application.Data;
using CRUD_Application.Models;
using CRUD_Application.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace CRUD_Application.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly ApplicationDbContext _context;

        public StudentRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<Student>> GetAllAsync(
           bool includeDepartment = false)
        {
            IQueryable<Student> query = _context.Students;

            if (includeDepartment)
            {
                query = query.Include(s => s.Department);
            }

            return await query.ToListAsync();
        }

        public async Task<Student?> GetByIdAsync(
           int id,
           bool includeDepartment = false)
        {
            IQueryable<Student> query = _context.Students;

            if (includeDepartment)
            {
                query = query.Include(s => s.Department);
            }

            return await query
                .FirstOrDefaultAsync(s => s.StudentId == id);
        }


        public async Task<Student?> GetByEmailAsync(string email)
        {
            return await _context.Students
                .FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<Student> AddAsync(Student student)
        {
            await _context.Students.AddAsync(student);
            await _context.SaveChangesAsync();

            return student;
        }

        public async Task UpdateAsync(Student student)
        {
            _context.Students.Update(student);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Student student)
        {
            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
        }
    }
}
