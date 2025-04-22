using Microsoft.AspNetCore.Mvc;
using WebNovel.Models;
using WebNovel.Services.Interfaces;

namespace WebNovel.Controllers.Api
{
    using Microsoft.AspNetCore.Authorization;

    [ApiController]
    [Route("api/[controller]")]
    public class TagController : ControllerBase
    {
        private readonly ITagService _tagService;

        public TagController(ITagService tagService)
        {
            _tagService = tagService;
        }

        //  Ai cũng xem được danh sách tag
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var tags = await _tagService.GetAllTagsAsync();
            return Ok(tags);
        }

        //  Ai cũng xem được tag theo id
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int id)
        {
            var tag = await _tagService.GetTagByIdAsync(id);
            if (tag == null) return NotFound();
            return Ok(tag);
        }

        //  Chỉ Contributor hoặc Admin mới được tạo tag
        [HttpPost]
        [Authorize(Roles = "Admin,Contributor")]
        public async Task<IActionResult> Create([FromBody] Tag tag)
        {
            var created = await _tagService.CreateTagAsync(tag);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // Chỉ Contributor hoặc Admin mới được sửa tag
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Contributor")]
        public async Task<IActionResult> Update(int id, [FromBody] Tag tag)
        {
            var updated = await _tagService.UpdateTagAsync(id, tag);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        // Chỉ Admin được xóa tag
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _tagService.DeleteTagAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }
    }


}
