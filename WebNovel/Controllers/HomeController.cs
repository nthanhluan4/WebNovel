using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Diagnostics;
using WebNovel.Models;
using WebNovel.Models.Dtos;
using WebNovel.Services.Interfaces;

namespace WebNovel.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IStoryService _storyService;
        private readonly IAntiforgery _antiforgery;
        private readonly IMemoryCache _cache;
        public HomeController(ILogger<HomeController> logger,
            IStoryService storyService, 
            IAntiforgery antiforgery,
            IMemoryCache cache)
        {
            _logger = logger;
            _storyService = storyService;
            _antiforgery = antiforgery;
            _cache = cache;
        }

        [HttpGet]
        public async Task<IActionResult> IndexAsync()
        {
            ViewBag.AntiForgeryToken = _antiforgery.GetAndStoreTokens(HttpContext).RequestToken;
            var cacheKey = "TopVotedStories";
            if (!_cache.TryGetValue(cacheKey, out List<StoryDto> stories))
            {
                stories = await _storyService.GetRandomStoriesAsync(4);
                _cache.Set(cacheKey, stories, TimeSpan.FromMinutes(5)); // Cache 5 phút
            }
            ViewBag.RandomStories = stories;
            return View();
        }

  

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
