using LanServe.Domain.Entities;

namespace LanServe.Application.Interfaces.Repositories;

public interface IReviewRepository : IGenericRepository<Review>
{
    Task<IEnumerable<Review>> GetForUserAsync(string revieweeId);
    Task<double> GetAverageRatingAsync(string revieweeId);
    Task<IReadOnlyList<Review>> GetByUserIdAsync(string userId);
}
