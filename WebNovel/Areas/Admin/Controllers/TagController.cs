using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebNovel.Models;
using WebNovel.Services.Implementations;
using WebNovel.Services.Interfaces;

namespace WebNovel.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    [Authorize]
    public class TagController : Controller
    {
        private readonly ISlugService<Tag> _service;

        public TagController(ISlugService<Tag> service) => _service = service;

        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> CreateOrUpdate(int id = 0)
        {
            ViewData["Action"] = "Create";
            if (id == 0)
                return PartialView("CreateOrUpdate", new Tag());

            var model = await _service.GetByIdAsync(id);
            if (model == null)
                return PartialView("CreateOrUpdate", new Tag());
            ViewData["Action"] = "Update";
            return PartialView("CreateOrUpdate", model);
        }
    }
}
