using IKitaplik.Business.Abstract;
using IKitaplik.Entities.DTOs;
using IKitaplik.Entities.DTOs.WriterDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace IKitaplik.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,User")]
    public class WriterController : ControllerBase
    {
        private readonly IWriterService _writerService;

        public WriterController(IWriterService writerService)
        {
            _writerService = writerService;
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetAll(PageRequestDto page)
        {
            var res = await _writerService.GetAllAsync(page.Page, page.PageSize);
            if (!res.Success)
                return BadRequest(res);
            return Ok(res);
        }

        [HttpGet("getById")]
        public async Task<IActionResult> GetById(int id)
        {
            var res = await _writerService.GetByIdAsync(id);
            if (!res.Success)
                return BadRequest(res);
            return Ok(res);
        }

        [HttpGet("writerNameContains")]
        public async Task<IActionResult> GetByName(string name)
        {
            var res = await _writerService.GetAllFilteredNameContainsAsync(name);
            if (!res.Success)
                return BadRequest(res);
            return Ok(res);
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add(WriterAddDto writerAddDto)
        {
            var res = await _writerService.AddAsync(writerAddDto);
            if (!res.Success)
                return BadRequest(res);
            return Ok(res);
        }

        [HttpPost("update")]
        public async Task<IActionResult> Update(WriterUpdateDto writerUpdateDto)
        {
            var res = await _writerService.UpdateAsync(writerUpdateDto);
            if (!res.Success)
                return BadRequest(res);
            return Ok(res);
        }

        [HttpPost("delete")]
        public async Task<IActionResult> Delete(DeleteDto deleteDto)
        {
            var res = await _writerService.DeleteAsync(deleteDto.Id);
            if (!res.Success)
                return BadRequest(res);
            return Ok(res);
        }
    }
}
