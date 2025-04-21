using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebNovel.Models;
using WebNovel.Services.Interfaces;

namespace WebNovel.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class GenreApiController : ControllerBase
    {
        private readonly IGenreService _service;

        public GenreApiController(IGenreService service)
        {
            _service = service;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

        [AllowAnonymous]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var genre = await _service.GetByIdAsync(id);
            return genre == null ? NotFound() : Ok(genre);
        }

        [AllowAnonymous]
        [HttpGet("slug/{slug}")]
        public async Task<IActionResult> GetBySlug(string slug)
        {
            var genre = await _service.GetBySlugAsync(slug);
            return genre == null ? NotFound() : Ok(genre);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Genre genre)
        {
            var result = await _service.CreateAsync(genre);
            return result ? Ok(genre) : Conflict("Tên thể loại đã tồn tại.");
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] Genre genre)
        {
            if (id != genre.Id) return BadRequest();
            var result = await _service.UpdateAsync(genre);
            return result ? Ok(genre) : NotFound();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteAsync(id);
            return result ? Ok() : NotFound();
        }
    }
}

