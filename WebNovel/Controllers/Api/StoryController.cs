using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebNovel.Models;
using WebNovel.Services.Interfaces;

namespace WebNovel.Controllers.Api;

[Route("api/[controller]")]
[ApiController]
public class StoryController : ControllerBase
{
    private readonly ISlugService<Story> _service;

    public StoryController(ISlugService<Story> service)
    {
        _service = service;
    }

    /// <summary>
    /// Trả về tất cả truyện (không phân trang) — thường dùng cho export
    /// </summary>
    [HttpGet("all")]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll()
    {
        var result = await _service.GetAllAsync();
        return Ok(result);
    }

    /// <summary>
    /// Trả về dữ liệu dạng DataSource cho Kendo Grid
    /// </summary>
    [HttpPost("grid")]
    [Authorize(Roles = "Admin,Contributor")]
    public async Task<IActionResult> GetGrid([FromBody] DataSourceRequest request)
    {
        var result = await _service.GetAllDataSourceAsync(request);
        return Ok(result);
    }

    /// <summary>
    /// Trả về dữ liệu cho dropdown
    /// </summary>
    [HttpGet("dropdown")]
    [AllowAnonymous]
    public async Task<IActionResult> GetDropdown()
    {
        var result = await _service.GetDropdownDataAsync();
        return Ok(result);
    }

    /// <summary>
    /// Truy vấn truyện theo ID
    /// </summary>
    [HttpGet("{id:int}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(int id)
    {
        var response = await _service.GetByIdAsync(id);
        return response.Success ? Ok(response) : NotFound(response);
    }

    /// <summary>
    /// Truy vấn truyện theo Slug
    /// </summary>
    [HttpGet("slug/{slug}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetBySlug(string slug)
    {
        var response = await _service.GetBySlugAsync(slug);
        return response.Success ? Ok(response) : NotFound(response);
    }

    /// <summary>
    /// Thêm mới truyện
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin,Contributor")]
    public async Task<IActionResult> Create([FromBody] Story model)
    {
        var response = await _service.CreateAsync(model);
        return Ok(response);
    }

    /// <summary>
    /// Cập nhật truyện
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Contributor")]
    public async Task<IActionResult> Update(int id, [FromBody] Story model)
    {
        var response = await _service.UpdateAsync(id, model);
        return response.Success ? Ok(response) : NotFound(response);
    }

    /// <summary>
    /// Xoá truyện
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var response = await _service.DeleteAsync(id);
        return response.Success ? Ok(response) : NotFound(response);
    }
}
