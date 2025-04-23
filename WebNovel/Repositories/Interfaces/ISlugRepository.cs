namespace WebNovel.Repositories.Interfaces
{
    public interface ISlugRepository<T> : IBaseRepository<T> where T : class
    {
        Task<T?> GetBySlugAsync(string slug);
    }

}
