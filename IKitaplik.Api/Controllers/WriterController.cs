using IKitaplik.Business.Abstract;
using IKitaplik.Entities.DTOs.WriterDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult GetAll()
        {
            var res = _writerService.GetAll();
            if (!res.Success)
                return BadRequest(res);
            return Ok(res);
        }

        [HttpGet("getById")]
        public IActionResult GetById(int id)
        {
            var res = _writerService.GetById(id);
            if (!res.Success)
                return BadRequest(res);
            return Ok(res);
        }

        [HttpGet("writerNameContains")]
        public IActionResult GetByName(string name)
        {
            var res = _writerService.GetAllFilteredNameContains(name);
            if (!res.Success)
                return BadRequest(res);
            return Ok(res);
        }

        [HttpPost("add")]
        public IActionResult Add(WriterAddDto writerAddDto)
        {
            var res = _writerService.Add(writerAddDto);
            if (!res.Success)
                return BadRequest(res);
            return Ok(res);
        }

        [HttpPost("update")]
        public IActionResult Update(WriterUpdateDto writerUpdateDto)
        {
            var res = _writerService.Update(writerUpdateDto);
            if (!res.Success)
                return BadRequest(res);
            return Ok(res);
        }

        [HttpPost("delete")]
        public IActionResult Delete(int id)
        {
            var res = _writerService.Delete(id);
            if (!res.Success)
                return BadRequest(res);
            return Ok(res);
        }
    }
}
