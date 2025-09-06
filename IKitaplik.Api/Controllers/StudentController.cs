using IKitaplik.Business.Abstract;
using IKitaplik.Entities.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace IKitaplik.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,User")]
    public class StudentController : ControllerBase
    {
        IStudentService _studentService;
        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }
        [HttpGet("getall")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _studentService.GetAllAsync();
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpGet("getallisactive")]
        public async Task<ActionResult> GetAllActive()
        {
            var res = await _studentService.GetAllActiveAsync();
            if (res.Success)
                return Ok(res);
            return BadRequest(res);
        }

        [HttpGet("getallbyname")]
        public async Task<IActionResult> GetAllByName(string name)
        {
            var result = await _studentService.GetAllByNameAsync(name);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("getbyid")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _studentService.GetByIdAsync(id);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add(StudentAddDto studentAddDto)
        {
            var result = await _studentService.AddAsync(studentAddDto);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("update")]
        public async Task<IActionResult> Update(StudentUpdateDto studentUpdateDto)
        {
            var result = await _studentService.UpdateAsync(studentUpdateDto);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("delete")]
        public async Task<IActionResult> Delete(DeleteDto deleteDto)
        {
            var result = await _studentService.DeleteAsync(deleteDto.Id);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
