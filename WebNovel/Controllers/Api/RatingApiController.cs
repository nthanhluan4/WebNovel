using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebNovel.Models;
using WebNovel.Services.Interfaces;

namespace WebNovel.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class RatingApiController : ControllerBase
    {
        private readonly IRatingService _service;
        public RatingApiController(IRatingService service) => _service = service;

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int id)
        {
            var rating = await _service.GetByIdAsync(id);
            return rating == null ? NotFound() : Ok(rating);
        }

        [HttpGet("story/{storyId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByStory(int storyId) => Ok(await _service.GetByStoryIdAsync(storyId));

        [HttpGet("chapter/{chapterId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByChapter(int chapterId) => Ok(await _service.GetByChapterIdAsync(chapterId));

        [HttpPost]
        [Authorize(Roles = "Admin,Contributor,Reader")]
        public async Task<IActionResult> Create([FromBody] Rating rating)
        {
            var result = await _service.CreateAsync(rating);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Contributor")]
        public async Task<IActionResult> Update(int id, [FromBody] Rating rating)
        {
            var result = await _service.UpdateAsync(id, rating);
            return result == null ? NotFound() : Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _service.DeleteAsync(id);
            return success ? NoContent() : NotFound();
        }
    }

}
