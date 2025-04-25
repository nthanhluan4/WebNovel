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
    public class ContributorController : Controller
    {
        private readonly ISlugService<Contributor> _service;

        public ContributorController(ISlugService<Contributor> service) => _service = service;

        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> CreateOrUpdate(int id = 0)
        {
            ViewData["Action"] = "Create";
            if (id == 0)
                return PartialView("CreateOrUpdate", new Contributor());

            var model = await _service.GetByIdAsync(id);
            if (model == null)
                return PartialView("CreateOrUpdate", new Contributor());
            ViewData["Action"] = "Update";
            return PartialView("CreateOrUpdate", model);
        }
    }
}
