using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebNovel.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebNovel.Services.Interfaces;

namespace WebNovel.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    [Authorize]
    public class StoryController : Controller
    {
        private readonly ISlugService<Story> _service;
        private readonly IAuthorService _authorService;

        public StoryController(ISlugService<Story> service, IAuthorService authorService)
        {
            _service = service;
            _authorService = authorService;
        }

        public IActionResult Index()
        {
            return View();
        }

       
        public async Task<IActionResult> CreateOrUpdate(int id = 0)
        {
            ViewData["Action"] = "Create";
            if (id == 0)
                return PartialView("CreateOrUpdate", new Story());

            var model = await _service.GetByIdAsync(id);
            if (model == null)
                return PartialView("CreateOrUpdate", new Story());
            ViewData["Action"] = "Update";
            return PartialView("CreateOrUpdate", model);
        }

        public async Task<IActionResult> DetailAsync(int id)
        {
            var model = await _service.GetByIdAsync(id);
            return View("Detail", model);
        }

    }

}

