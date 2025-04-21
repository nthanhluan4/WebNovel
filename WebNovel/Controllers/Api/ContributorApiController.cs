using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebNovel.Models;
using WebNovel.Services.Interfaces;

namespace WebNovel.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContributorApiController : ControllerBase
    {
        private readonly IContributorService _service;

        public ContributorApiController(IContributorService service)
        {
            _service = service;
        }

        [AllowAnonymous]
        [HttpGet("slug/{slug}")]
        public async Task<IActionResult> GetBySlug(string slug)
        {
            var contributor = await _service.GetBySlugAsync(slug);
            return contributor == null ? NotFound() : Ok(contributor);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("pending")]
        public async Task<IActionResult> GetAllPending() => Ok(await _service.GetAllPendingAsync());

        [Authorize(Roles = "Admin")]
        [HttpPost("approve/{id:int}")]
        public async Task<IActionResult> Approve(int id)
        {
            var result = await _service.ApproveAsync(id);
            return result ? Ok() : NotFound();
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> GetMyProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var contributor = await _service.GetByUserIdAsync(userId);
            return contributor == null ? NotFound() : Ok(contributor);
        }

        [Authorize]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] Contributor contributor)
        {
            contributor.CreatedByUserId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var result = await _service.RequestToBecomeAsync(contributor);
            return result ? Ok() : Conflict("You have already registered as contributor.");
        }

        [Authorize]
        [HttpPut("update")]  // chỉ cho chính mình cập nhật mô tả, avatar...
        public async Task<IActionResult> Update([FromBody] Contributor contributor)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var existing = await _service.GetByUserIdAsync(userId);
            if (existing == null || existing.Id != contributor.Id) return Forbid();

            contributor.CreatedByUserId = userId;
            contributor.IsApproved = existing.IsApproved;

            var result = await _service.UpdateProfileAsync(contributor);
            return result ? Ok(contributor) : BadRequest();
        }
    }

}
