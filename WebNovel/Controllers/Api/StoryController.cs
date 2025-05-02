using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebNovel.Models;
using WebNovel.Models.Dtos;
using WebNovel.Services.Implementations;
using WebNovel.Services.Interfaces;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class StoryController : ControllerBase
{
    private readonly ISlugService<Story> _service;
    private readonly IStoryService _baseService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IWebHostEnvironment _env;

    public StoryController(ISlugService<Story> service, 
        UserManager<ApplicationUser> userManager,
        IStoryService baseService,
        IWebHostEnvironment env)
    {
        _service = service;
        _userManager = userManager;
        _baseService = baseService;
        _env = env;
    }

    [HttpGet("all")]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

    [HttpPost("grid")]
    [Authorize(Roles = "Admin,Contributor")]
    public async Task<IActionResult> GetGrid([DataSourceRequest] DataSourceRequest request)
    {
        return Ok(await _baseService.GetDataSourceAsync(request));
        //return Ok(await _service.GetAllDataSourceAsync(request));
    }
        

    [HttpGet("dropdown")]
    [AllowAnonymous]
    public async Task<IActionResult> GetDropdown() => Ok(await _service.GetDropdownDataAsync());


    [HttpGet("status-dropdown")]
    [AllowAnonymous]
    public async Task<IActionResult> GetStatusDropdown()
    {
        var statuses = new List<object>
        {
            new { text = "Đang ra", value = 1 },
            new { text = "Hoàn thành", value = 2 },
            new { text = "Tạm dừng", value = 3 }
        };
        return Ok(statuses);
    }


    [HttpGet("{id:int}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(int id) => Ok(await _service.GetByIdAsync(id));

    [HttpGet("slug/{slug}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetBySlug(string slug) =>
        await _service.GetBySlugAsync(slug) is var r && r.Success ? Ok(r) : NotFound(r);

    [HttpPost]
    [Authorize(Roles = "Admin,Contributor")]
    public async Task<IActionResult> Create([FromBody] Story model)
    {
        var user = await _userManager.GetUserAsync(User);
        model.CreatedByUserId = user?.Id;
        var result = await _service.CreateAsync(model);
        return Ok(result);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Contributor")]
    public async Task<IActionResult> Update(int id, [FromBody] Story model) =>
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

        return Ok(new { Success = true, Message = $"Đã xóa {lstId.Count} Story" });
    }

    [HttpPost("{storyId:int}/upload-cover")]
    [RequestSizeLimit(2 * 1024 * 1024)]
    public async Task<IActionResult> UploadCover(int storyId, List<IFormFile> files)
    {
        if (files == null || files.Count == 0)
            return BadRequest("Chưa có file.");

        var file = files[0];

        // Kiểm tra size & extension lần nữa
        if (file.Length > 2 * 1024 * 1024)
            return BadRequest("File quá lớn.");
        var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
        var permitted = new[] { ".jpg", ".jpeg", ".png", ".webp" };
        if (!permitted.Contains(ext))
            return BadRequest("Chỉ cho phép định dạng JPG, PNG hoặc WebP.");

        string fileName = DateTime.Now.ToString("yyyyMMdd_HHmmssfff") + ext;


        // Lấy slug của truyện để đặt tên
        var story = await _service.GetByIdAsync(storyId);
        if (story != null)
        {
            var slug = story.Slug ?? "";
            if (!string.IsNullOrEmpty(slug))
                fileName = $"{slug}{ext}";
        }

        var safeName = Path.GetFileName(fileName);
        var folder = Path.Combine(_env.WebRootPath,"uploads" ,"images", "story_covers");
        if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
        var path = Path.Combine(folder, safeName);

        // Lưu file
        using var stream = System.IO.File.Create(path);
        await file.CopyToAsync(stream);


        if (story != null)
        {
            // Cập nhật CoverUrl vào database
            story.CoverUrl = $"/uploads/images/story_covers/{safeName}";
            await _service.UpdateAsync(story.Id, story);
        }
       
        // Kendo Upload mong đợi JSON { uploaded, url }
        return Ok(new
        {
            uploaded = true,
            url = story.CoverUrl
        });
    }
}
