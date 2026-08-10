using CRUD_Application.Dtos;
using CRUD_Application.Models;
using CRUD_Application.Repositories;
using CRUD_Application.Repositories.CollegeManagement.Repositories;
using CRUD_Application.Repositories.Interface;
using CRUD_Application.Services.Interface;
using System.ComponentModel.DataAnnotations;

namespace CRUD_Application.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _repository;
        private readonly IDepartmentRepository _departmentRepository;

        public StudentService(
            IStudentRepository studentRepository,
            IDepartmentRepository departmentRepository)
        {
            _repository = studentRepository;
            _departmentRepository = departmentRepository;
        }

        public async Task<List<Student>> GetAllStudentsAsync()
        {
            var students = await _repository.GetAllAsync();
            return students.ToList();
        }

        public async Task<Student?> GetStudentByIdAsync(int id)
        {
            var student = await _repository.GetByIdAsync(id);

            if (student == null)
                throw new Exception("ID not found");

            return student;
        }

        public async Task<Student> CreateStudentAsync(StudentDto dto)
        {
            if (!new EmailAddressAttribute().IsValid(dto.Email))
                throw new Exception("Invalid email format.");

            var existingStudent = await _repository.GetByEmailAsync(dto.Email);

            if (existingStudent != null)
                throw new Exception("Email already exists.");

            // Check Department Exists
            var department = await _departmentRepository.GetByIdAsync(dto.DepartmentId);

            if (department == null)
                throw new Exception("Department not found.");

            var student = new Student
            {
                Name = dto.Name,
                Age = dto.Age,
                Email = dto.Email,
                DepartmentId = dto.DepartmentId
            };

            return await _repository.AddAsync(student);
        }

        public async Task<Student> UpdateStudentAsync(int id, StudentDto dto)
        {
            var student = await _repository.GetByIdAsync(id);

            if (student == null)
                throw new Exception("Student not found.");

            var department = await _departmentRepository.GetByIdAsync(dto.DepartmentId);

            if (department == null)
                throw new Exception("Department not found.");

            var existingStudent = await _repository.GetByEmailAsync(dto.Email);

            if (existingStudent != null && existingStudent.StudentId != id)
                throw new Exception("Email already exists.");

            student.Name = dto.Name;
            student.Age = dto.Age;
            student.Email = dto.Email;
            student.DepartmentId = dto.DepartmentId;

            await _repository.UpdateAsync(student);

            return student;
        }

        public async Task<bool> DeleteStudentAsync(int id)
        {
            var student = await _repository.GetByIdAsync(id);

            if (student == null)
                throw new Exception("ID not found");

            if (student == null)
                return false;

            await _repository.DeleteAsync(student);

            return true;
        }
    }
}
