
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebNovel.Services;
using WebNovel.Models;
using global::WebNovel.Services.Interfaces;
using global::WebNovel.Models.Dtos;
using global::WebNovel.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebNovel.Services.Implementations;

namespace WebNovel.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class StoryController : Controller
    {
        private readonly IStoryService _storyService;
        private readonly IAuthorService _authorService;

        public StoryController(IStoryService storyService, IAuthorService authorService)
        {
            _storyService = storyService;
            _authorService = authorService; 
        }

        public async Task<IActionResult> Index(string? keyword)
        {
            var stories = await _storyService.GetAllAsync();

            //if (!string.IsNullOrEmpty(keyword))
            //    stories = stories.Where(s => s.Title.Contains(keyword, StringComparison.OrdinalIgnoreCase));

            var result = stories.Select(s => new StoryDto
            {
                Id = s.Id,
                Title = s.Title,
                Description = s.Description,
                AuthorName = s.AuthorName,
                TotalChapters = s.TotalChapters,
                TotalWords = s.TotalWords,
                CreatedAt = s.CreatedAt
            }).ToList();

            ViewBag.Keyword = keyword;
            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> CreateAsync()
        {
            ViewBag.Authors = new SelectList(await _authorService.GetAllAsync(), "Id", "Name");
            return View("Create", new Story());
        }

        [HttpPost]
        public async Task<IActionResult> Create(Story story)
        {
            if (!ModelState.IsValid) return View(story);
            await _storyService.CreateAsync(story);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var story = await _storyService.GetByIdAsync(id);
            if (story == null) return NotFound();
            return View(story);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Story updated)
        {
            if (!ModelState.IsValid) return View(updated);
            await _storyService.UpdateAsync(updated);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _storyService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }

}

