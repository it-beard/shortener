using Itbeard.Data.Entites;
using Itbeard.Data.Repositories.Interfaces;
using Itbeard.Models;
using Microsoft.EntityFrameworkCore;

namespace Itbeard.Data.Repositories
{
    public class UrlRepository : RepositoryBase<Url>, IUrlRepository
    {
        // ReSharper disable once NotAccessedField.Local
        private readonly ApplicationDbContext context;
        
        public UrlRepository(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }

        public async Task<List<UrlModel>> GetAllUrlsWithShortStatByDateDescAsync()
        {
            return await context.Urls
                .Select(u => new UrlModel
                {
                   Id = u.Id,
                   CreatedAt = u.CreatedAt,
                   UpdatedAt = u.UpdatedAt,
                   ShortName = u.ShortName,
                   TargetUrl = u.TargetUrl,
                   VisitCount = u.Statistics.Count
                })
                .OrderByDescending(p =>p.CreatedAt)
                .ToListAsync();
        }
    }
}