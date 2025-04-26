using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebNovel.Models;
using WebNovel.Models.Dtos;
using WebNovel.Services.Implementations;
using WebNovel.Services.Interfaces;

namespace WebNovel.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ChapterController : ControllerBase
    {
        private readonly IChapterService _service;
        private readonly IBackgroundTaskQueue _taskQueue;
        private readonly ILogger<ChapterController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;

        public ChapterController(IChapterService service,
            IBackgroundTaskQueue taskQueue,
            ILogger<ChapterController> logger,
            UserManager<ApplicationUser> userManager)
        {
            _service = service;
            _taskQueue = taskQueue;
            _logger = logger;
            _userManager = userManager;
        }

        [HttpGet("story/{storyId:int}")]
        public async Task<IActionResult> GetByStory(int storyId)
        {
            var lstChapter = await _service.GetByStoryIdAsync(storyId);
            var result = lstChapter.Where(s => s.IsPublic == true);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var chapter = await _service.GetByIdAsync(id);
            if (chapter == null || chapter.IsPublic == false)
                return NotFound();
            return Ok(chapter);
        }

        [HttpGet("{id}/content")]
        public async Task<IActionResult> GetContent(string id)
        {
            var chapter = await _service.GetByIdAsync(id);
            if (chapter == null) return NotFound();
            _taskQueue.QueueBackgroundTask(async token =>
            {
                try
                {
                    await _service.IncreaseReadCountAsync(chapter.Id);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Không thể cập nhật lượt đọc cho chương [{chapter.Id} - {chapter.Title}], truyện [{chapter.StoryId}].");
                }
            });
            var content = await _service.LoadContentAsync(chapter);
            return Ok(new { chapter.Id, chapter.Title, content });
        }

        [Authorize(Roles = "Poster,Admin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Chapter model)
        {
            var user = await _userManager.GetUserAsync(User);
            model.CreatedByUserId = user?.Id;
            model.Id = Guid.NewGuid().ToString();
            var result = await _service.CreateAsync(model, model.Content);
            return result ? Ok(model) : BadRequest();
        }

        [Authorize(Roles = "Poster,Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] Chapter model)
        {
            var chapter = await _service.GetByIdAsync(id);
            if (chapter == null || chapter.Id != id) return BadRequest();

            chapter.Title = model.Title;
            chapter.Order = model.Order;
            var result = await _service.UpdateAsync(chapter, model.Content);
            return result ? Ok(chapter) : BadRequest();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _service.DeleteAsync(id);
            return result ? Ok() : NotFound();
        }
    }

}
