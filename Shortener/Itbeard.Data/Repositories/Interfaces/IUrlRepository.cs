using Itbeard.Data.Entites;
using Itbeard.Models;

namespace Itbeard.Data.Repositories.Interfaces
{
    public interface IUrlRepository : IRepositoryBase<Url>
    {
        Task<List<UrlModel>> GetAllUrlsWithShortStatByDateDescAsync();
    }
}