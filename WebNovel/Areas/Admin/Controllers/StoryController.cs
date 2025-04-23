using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebNovel.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebNovel.Services.Interfaces;

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
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> CreateAsync()
        {
            return View("CreateOrUpdate", new Story());
        }

        [HttpPost]
        public async Task<IActionResult> Create(Story story)
        {
            if (!ModelState.IsValid) 
                return View(story);
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
            if (!ModelState.IsValid) 
                return View(updated);
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

