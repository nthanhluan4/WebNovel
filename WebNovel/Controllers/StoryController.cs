using Microsoft.AspNetCore.Mvc;
using WebNovel.Services.Interfaces;

namespace WebNovel.Controllers
{
    [Route("truyen")]
    public class StoryController : Controller
    {
        private readonly IStoryService _storyService;
        public StoryController(IStoryService storyService) => _storyService = storyService;

        [HttpGet("{slug}")]
        public async Task<IActionResult> DetailAsync(string slug)
        {
            var story = await _storyService.GetStoryDtoBySlugAsync(slug);
            if (story == null) return NotFound();
            return View("Detail", story);
        }
    }
}
