using Microsoft.AspNetCore.Mvc;
using WebNovel.Services.Interfaces;

namespace WebNovel.Controllers
{
    [Route("truyen")]
    public class StoryController : Controller
    {
        private readonly IStoryService _storyService;
        private readonly IChapterService _chapterService;

        public StoryController(IStoryService storyService, IChapterService chapterService)
        {
            _storyService = storyService;
            _chapterService = chapterService;
        }

        [HttpGet("{slug}")]
        public async Task<IActionResult> DetailAsync(string slug)
        {
            var story = await _storyService.GetStoryDtoBySlugAsync(slug);
            if (story == null) return NotFound();

            var newChapters = await _chapterService.GetNewChapterByStoryIdAsync(story.Id, 5);
            var storiesOfAuthor = await _storyService.GetByAuthorSlugAsync(story.AuthorSlug);
            var storiesOfContributor = await _storyService.GetByContributorSlugAsync(story.ContributorSlug);
            var storiesOfGenre = await _storyService.GetByGenreSlugAsync(story.ListGenreSlug[0]);

            ViewBag.StoriesOfAuthor = storiesOfAuthor;    
            ViewBag.StoriesOfContributor = storiesOfContributor;
            ViewBag.StoriesOfGenre = storiesOfGenre;
            ViewBag.NewChapters = newChapters;


            return View("Detail", story);
        }
    }
}
