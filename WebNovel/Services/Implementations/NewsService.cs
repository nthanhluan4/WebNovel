using Microsoft.EntityFrameworkCore.Metadata;
using WebNovel.Models;
using WebNovel.Models.Dtos;
using WebNovel.Repositories.Implementations;
using WebNovel.Repositories.Interfaces;
using WebNovel.Services.Interfaces;

namespace WebNovel.Services.Implementations
{
    public class NewsService : SlugService<News>, INewsService
    {
        private readonly INewsRepository _newsRepo;

        public NewsService(INewsRepository newsRepo, IModel model) : base(newsRepo, model)
        {
            _newsRepo = newsRepo;
        }

        public async Task<ServiceResponse<List<News>>> GetPinnedAsync()
        {
            var list = await _newsRepo.GetPinnedAsync();
            return ServiceResponse<List<News>>.Ok(list);
        }

        async Task<List<News>> INewsService.GetPinnedAsync()
        {
            return await _newsRepo.GetPinnedAsync();
        }
    }


}
