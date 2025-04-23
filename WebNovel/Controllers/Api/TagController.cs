using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebNovel.Models;
using WebNovel.Services.Interfaces;

[Route("api/[controller]")]
[ApiController]
public class TagController : ControllerBase
{
    private readonly ISlugService<Tag> _service;

    public TagController(ISlugService<Tag> service) => _service = service;

    [HttpGet("all")]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

    [HttpPost("grid")]
    [Authorize(Roles = "Admin,Contributor")]
    public async Task<IActionResult> GetGrid([FromBody] DataSourceRequest request) =>
        Ok(await _service.GetAllDataSourceAsync(request));

    [HttpGet("dropdown")]
    [AllowAnonymous]
    public async Task<IActionResult> GetDropdown() => Ok(await _service.GetDropdownDataAsync());

    [HttpGet("{id:int}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(int id) =>
        await _service.GetByIdAsync(id) is var r && r.Success ? Ok(r) : NotFound(r);

    [HttpGet("slug/{slug}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetBySlug(string slug) =>
        await _service.GetBySlugAsync(slug) is var r && r.Success ? Ok(r) : NotFound(r);

    [HttpPost]
    [Authorize(Roles = "Admin,Contributor")]
    public async Task<IActionResult> Create([FromBody] Tag model) => Ok(await _service.CreateAsync(model));

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Contributor")]
    public async Task<IActionResult> Update(int id, [FromBody] Tag model) =>
        await _service.UpdateAsync(id, model) is var r && r.Success ? Ok(r) : NotFound(r);

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id) =>
        await _service.DeleteAsync(id) is var r && r.Success ? Ok(r) : NotFound(r);
}
