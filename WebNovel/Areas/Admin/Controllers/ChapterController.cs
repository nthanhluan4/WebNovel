using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebNovel.Models;
using WebNovel.Services.Implementations;
using WebNovel.Services.Interfaces;

namespace WebNovel.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    [Authorize]
    public class ChapterController : Controller
    {
        private readonly ISlugService<Chapter> _baseService;
        private readonly IChapterService _service;
        private readonly IBackgroundTaskQueue _taskQueue;
        private readonly ILogger<ChapterController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;

        public ChapterController(IChapterService service,
            IBackgroundTaskQueue taskQueue,
            ILogger<ChapterController> logger,
            ISlugService<Chapter> baseService,
            UserManager<ApplicationUser> userManager)
        {
            _baseService = baseService;
            _service = service;
            _taskQueue = taskQueue;
            _logger = logger;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> CreateOrUpdate(string id, [FromQuery] int? storyId)
        {
            ViewData["Action"] = "Create";
            ViewData["StoryId"] = storyId ?? 0;

            if (string.IsNullOrEmpty(id) || id == "0")
                return PartialView("CreateOrUpdate", new Chapter());

            var model = await _service.GetByIdAsync(id);
            if (model == null)
                return PartialView("CreateOrUpdate", new Chapter());
            ViewData["Action"] = "Update";
            model.Content = await _service.LoadContentAsync(model);
            return PartialView("CreateOrUpdate", model);
        }
    }
}
