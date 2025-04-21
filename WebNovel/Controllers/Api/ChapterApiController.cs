using Microsoft.AspNetCore.Authorization;
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
    public class ChapterApiController : ControllerBase
    {
        private readonly IChapterService _service;
        private readonly IBackgroundTaskQueue _taskQueue;
        private readonly ILogger<ChapterApiController> _logger;


        public ChapterApiController(IChapterService service, IBackgroundTaskQueue taskQueue, ILogger<ChapterApiController> logger)
        {
            _service = service;
            _taskQueue = taskQueue;
            _logger = logger;
        }

        [HttpGet("story/{storyId:int}")]
        public async Task<IActionResult> GetByStory(int storyId)
            => Ok(await _service.GetByStoryIdAsync(storyId));

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var chapter = await _service.GetByIdAsync(id);
            return chapter == null ? NotFound() : Ok(chapter);
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
        public async Task<IActionResult> Create([FromBody] ChapterCreateModel model)
        {
            var chapter = new Chapter
            {
                StoryId = model.StoryId,
                Title = model.Title,
                Order = model.Order,
                CreatedByUserId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? ""
            };
            var result = await _service.CreateAsync(chapter, model.Content);
            return result ? Ok(chapter) : BadRequest();
        }

        [Authorize(Roles = "Poster,Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] ChapterCreateModel model)
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
