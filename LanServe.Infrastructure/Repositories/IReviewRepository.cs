using LanServe.Domain.Entities;

namespace LanServe.Infrastructure.Repositories;

public interface IReviewRepository : IGenericRepository<Review>
{
    Task<IEnumerable<Review>> GetForUserAsync(string revieweeId);
    Task<double> GetAverageRatingAsync(string revieweeId);
}
