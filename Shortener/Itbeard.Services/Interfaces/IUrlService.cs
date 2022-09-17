using System.Threading.Tasks;
using Itbeard.Models;

namespace Itbeard.Services.Interfaces
{
    public interface IUrlService
    {
        Task<UrlModel> ReduceAsync(string targetUrl, string shortName);
        Task EditAsync(Guid id, string targetUrl);
        Task<UrlModel> GetByNameAsync(string shortName);
        Task<UrlModel> GetByIdAsync(Guid id);
        Task<List<UrlModel>> GetAllAsync();
        Task DeleteAsync(Guid id);
    }
}