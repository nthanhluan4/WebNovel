using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebNovel.Models;
using WebNovel.Services.Interfaces;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ContributorController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ISlugService<Contributor> _service;

    public ContributorController(ISlugService<Contributor> service, UserManager<ApplicationUser> userManager)
    {
        _service = service;
        _userManager = userManager;
    }

   

    [HttpGet("all")]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

    [HttpPost("grid")]
    [Authorize(Roles = "Admin,Contributor")]
    public async Task<IActionResult> GetGrid([DataSourceRequest] DataSourceRequest request) =>
        Ok(await _service.GetAllDataSourceAsync(request));

    [HttpGet("dropdown")]
    [AllowAnonymous]
    public async Task<IActionResult> GetDropdown() => Ok(await _service.GetDropdownDataAsync());

    [HttpGet("{id:int}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(int id) => Ok(await _service.GetByIdAsync(id));

    [HttpGet("slug/{slug}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetBySlug(string slug) =>
        await _service.GetBySlugAsync(slug) is var r && r.Success ? Ok(r) : NotFound(r);

    [HttpGet("types-dropdown")]
    [AllowAnonymous]
    public async Task<IActionResult> GetTypesDropdown()
    {
        var statuses = new List<object>
        {
            new { text = "Dịch giả", value = 1 },
            new { text = "Converter", value = 2 },
            new { text = "Sáng tác", value = 3 }
        };
        return Ok(statuses);
    }
    [HttpPost]
    [Authorize(Roles = "Admin,Contributor")]
    public async Task<IActionResult> Create([FromBody] Contributor model)
    {
        var user = await _userManager.GetUserAsync(User);
        model.CreatedByUserId = user?.Id;
        var result = await _service.CreateAsync(model);
        return Ok(result);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Contributor")]
    public async Task<IActionResult> Update(int id, [FromBody] Contributor model) =>
        await _service.UpdateAsync(id, model) is var r && r.Success ? Ok(r) : NotFound(r);

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id) =>
        await _service.DeleteAsync(id) is var r && r.Success ? Ok(r) : NotFound(r);


    [HttpPost("delete-multiple")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteMultiple([FromBody] string ids)
    {
        var lstId = ids.Split(',').Select(s => int.Parse(s)).ToList();
        if (lstId == null || !lstId.Any())
            return BadRequest(new { Success = false, Message = "Không có ID nào được gửi lên" });

        foreach (var id in lstId)
        {
            var result = await _service.DeleteAsync(id);
            if (!result.Success)
            {
                // Nếu 1 cái không xóa được thì trả lỗi toàn bộ
                return NotFound(new { Success = false, Message = $"Không tìm thấy ID: {id}" });
            }
        }

        return Ok(new { Success = true, Message = $"Đã xóa {lstId.Count} Contributor" });
    }
}
