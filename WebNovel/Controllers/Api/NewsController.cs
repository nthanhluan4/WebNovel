using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebNovel.Models;
using WebNovel.Services.Interfaces;

namespace WebNovel.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class NewsController : ControllerBase
    {
        private readonly INewsService _service;
        private readonly UserManager<ApplicationUser> _userManager;

        public NewsController(INewsService service, UserManager<ApplicationUser> userManager)
        {
            _service = service;
            _userManager = userManager;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpPost("grid")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetGrid([DataSourceRequest] DataSourceRequest request) =>
            Ok(await _service.GetAllDataSourceAsync(request));

        [HttpGet("dropdown")]
        [AllowAnonymous]
        public async Task<IActionResult> GetDropdown() => Ok(await _service.GetDropdownDataAsync());

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            return  Ok(result);
        }

        [HttpGet("slug/{slug}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetBySlug(string slug)
        {
            var result = await _service.GetBySlugAsync(slug);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpGet("pinned")]
        [AllowAnonymous]
        public async Task<IActionResult> GetPinned()
        {
            var result = await _service.GetPinnedAsync();
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] News model)
        {
            var user = await _userManager.GetUserAsync(User);
            model.AuthorId = user?.Id;
            var result = await _service.CreateAsync(model);
            return CreatedAtAction(nameof(GetById), new { id = result.Data?.Id }, result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] News model)
        {
            var result = await _service.UpdateAsync(id, model);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteAsync(id);
            return result.Success ? Ok(result) : NotFound(result);
        }
    }
}
