using CRUD_Application.Dtos;
using CRUD_Application.Models;
using CRUD_Application.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace CRUD_Application.Controllers
{
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
            return Ok(
                await _service.GetAllStudentsAsync(embed));
        }

        // GET: /api/Student/1
        // GET: /api/Student/1?embed=department
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(
            int id,
            [FromQuery] string? embed = null)
        {
            try
            {
                var student =
                    await _service.GetStudentByIdAsync(
                        id,
                        embed);

                return Ok(student);
            }
            catch (Exception ex)
            {
                return NotFound(new
                {
                    message = ex.Message
                });
            }
        }


        [HttpPost]
        public async Task<IActionResult> Create(StudentDto dto)
        {
            try
            {
                var student = await _service.CreateStudentAsync(dto);

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = student.StudentId },
                    student
                );
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, StudentDto dto)
        {
            try
            {
                var result = await _service.UpdateStudentAsync(id, dto);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _service.DeleteStudentAsync(id);


                return Ok(result);
            } catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });

            }
        }
    }
}
