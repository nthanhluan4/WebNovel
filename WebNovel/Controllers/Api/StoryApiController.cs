using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebNovel.Models;
using WebNovel.Services.Interfaces;

namespace WebNovel.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class StoryApiController : ControllerBase
    {
        private readonly IStoryService _service;

        public StoryApiController(IStoryService service)
        {
            _service = service;
        }
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var stories = await _service.GetAllAsync();
            return Ok(stories);
        }
        [AllowAnonymous]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var story = await _service.GetByIdAsync(id);
            if (story == null) return NotFound();
            return Ok(story);
        }
        [AllowAnonymous]
        [HttpGet("slug/{slug}")]
        public async Task<IActionResult> GetBySlug(string slug)
        {
            var story = await _service.GetBySlugAsync(slug);
            if (story == null) return NotFound();
            return Ok(story);
        }
        [AllowAnonymous]
        [HttpGet("genre/{genreId:int}")]
        public async Task<IActionResult> GetByGenre(int genreId)
        {
            var stories = await _service.GetByGenreAsync(genreId);
            return Ok(stories);
        }
        [AllowAnonymous]
        [HttpGet("author/{authorId:int}")]
        public async Task<IActionResult> GetByAuthor(int authorId)
        {
            var stories = await _service.GetByAuthorIdAsync(authorId);
            return Ok(stories);
        }
        [AllowAnonymous]
        [HttpGet("contributor/{contributorId:int}")]
        public async Task<IActionResult> GetByContributor(int contributorId)
        {
            var stories = await _service.GetByContributorIdAsync(contributorId);
            return Ok(stories);
        }
        [AllowAnonymous]
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string keyword)
        {
            var stories = await _service.SearchAsync(keyword);
            return Ok(stories);
        }

        [Authorize(Roles = "Poster,Admin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Story story)
        {
            story.CreatedByUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _service.CreateAsync(story);
            if (!result) return BadRequest("Cannot create story.");
            return Ok(story);
        }

        [Authorize(Roles = "Poster,Admin")]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] Story story)
        {
            if (id != story.Id) return BadRequest("Mismatched story ID.");

            var existing = await _service.GetByIdAsync(id);
            if (existing == null) return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isOwner = existing.CreatedByUserId == userId;
            var isAdmin = User.IsInRole("Admin");

            if (!isOwner && !isAdmin)
                return Forbid();

            var result = await _service.UpdateAsync(story);
            return result ? Ok(story) : NotFound();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteAsync(id);
            if (!result) return NotFound();
            return Ok();
        }
    }
}
