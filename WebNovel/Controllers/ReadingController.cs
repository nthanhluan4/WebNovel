using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebNovel.Models;
using WebNovel.Services;
using WebNovel.Services.Interfaces;

namespace WebNovel.Controllers
{
    [Route("doc-truyen")]
    public class ReadingController : Controller
    {
        private readonly IStoryService _storyService;
        private readonly IChapterService _chapterService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IBackgroundTaskQueue _backgroundTaskQueue;

        public ReadingController(IStoryService storyService, 
            IChapterService chapterService, 
            UserManager<ApplicationUser> userManager,
            IBackgroundTaskQueue backgroundTaskQueue)
        {
            _storyService = storyService;
            _chapterService = chapterService;
            _userManager = userManager;
            _backgroundTaskQueue = backgroundTaskQueue;
        }
        [Route("{storySlug}/{chapterSlug}")]
        [AllowAnonymous]
        public async Task<IActionResult> Read(string storySlug, string chapterSlug)
        {
            var chapter = await _chapterService.GetChapterReadingAsync(storySlug, chapterSlug);
            var user = await _userManager.GetUserAsync(User);
            _backgroundTaskQueue.QueueBackgroundWorkItem(async (sp, token) =>
            {
                var chapterService = sp.GetRequiredService<IChapterService>();
                await chapterService.IncreaseReadCountAsync(chapterSlug, storySlug, user?.Id);
            });
            return View("Read", chapter);
        }
    }
}
