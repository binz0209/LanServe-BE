using LanServe.Domain.Entities;
using MongoDB.Driver;
using LanServe.Application.Interfaces.Repositories;
using System.Linq; // dùng cho Average

namespace LanServe.Infrastructure.Repositories
{
    public class ReviewRepository : GenericRepository<Review>, IReviewRepository
    {
        public ReviewRepository(IMongoCollection<Review> collection) : base(collection) { }

        // (giữ API hiện có nhưng dùng await cho gọn)
        public async Task<IEnumerable<Review>> GetForUserAsync(string revieweeId)
        {
            var list = await _collection.Find(r => r.RevieweeId == revieweeId).ToListAsync();
            return list.AsEnumerable();
        }

        // >>> Bổ sung: implement đúng chữ ký interface yêu cầu
        // Giả định interface: Task<IReadOnlyList<Review>> GetByUserIdAsync(string userId);
        // Map userId -> RevieweeId theo domain hiện tại
        public async Task<IReadOnlyList<Review>> GetByUserIdAsync(string userId)
        {
            var list = await _collection.Find(r => r.RevieweeId == userId)
                                        .SortByDescending(r => r.CreatedAt)
                                        .ToListAsync();
            return list;
        }

        public async Task<double> GetAverageRatingAsync(string revieweeId)
        {
            var reviews = await _collection.Find(r => r.RevieweeId == revieweeId).ToListAsync();
            return reviews.Count == 0 ? 0 : reviews.Average(r => r.Rating);
        }
    }
}
