using LanServe.Domain.Entities;
using MongoDB.Driver;

namespace LanServe.Infrastructure.Repositories;

public class ReviewRepository : GenericRepository<Review>, IReviewRepository
{
    public ReviewRepository(IMongoCollection<Review> collection) : base(collection) { }

    public Task<IEnumerable<Review>> GetForUserAsync(string revieweeId)
       => _collection.Find(r => r.RevieweeId == revieweeId).ToListAsync().ContinueWith(task => task.Result.AsEnumerable());

    public async Task<double> GetAverageRatingAsync(string revieweeId)
    {
        var reviews = await _collection.Find(r => r.RevieweeId == revieweeId).ToListAsync();
        return reviews.Count == 0 ? 0 : reviews.Average(r => r.Rating);
    }
}
