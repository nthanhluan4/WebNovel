using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebNovel.Services.Interfaces;

namespace WebNovel.Controllers
{
    [Route("doc-truyen")]
    public class ReadingController : Controller
    {
        private readonly IStoryService _storyService;
        private readonly IChapterService _chapterService;

        public ReadingController(IStoryService storyService, IChapterService chapterService)
        {
            _storyService = storyService;
            _chapterService = chapterService;
        }
        [Route("{storySlug}/{chapterSlug}")]
        [AllowAnonymous]
        public async Task<IActionResult> Read(string storySlug, string chapterSlug)
        {
            var chapter = await _chapterService.GetChapterReadingAsync(storySlug, chapterSlug);

            return View("Read", chapter);
        }
    }
}
