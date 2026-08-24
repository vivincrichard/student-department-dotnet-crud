using CRUD_Application.Dtos;
using CRUD_Application.Models;
using CRUD_Application.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRUD_Application.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _service;

        public StudentController(IStudentService service)
        {
            _service = service;
        }

        // GET: /api/Student
        // GET: /api/Student?embed=department
        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] string? embed = null)
        {
            var students =
                await _service.GetAllStudentsAsync(embed);

            return Ok(students);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(
            int id,
            [FromQuery] string? embed = null)
        {
            var student =
                await _service.GetStudentByIdAsync(
                    id,
                    embed);

            return Ok(student);
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] StudentDto dto)
        {
            var student =
                await _service.CreateStudentAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = student.StudentId },
                student);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] StudentDto dto)
        {
            var student =
                await _service.UpdateStudentAsync(
                    id,
                    dto);

            return Ok(student);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteStudentAsync(id);

            return Ok(new
            {
                success = true,
                message = "Student deleted successfully."
            });
        }
    }
}
