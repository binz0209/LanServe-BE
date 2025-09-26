using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LanServe.Application.Interfaces.Services;
using LanServe.Domain.Entities;
using LanServe.Application.Interfaces.Repositories;

namespace LanServe.Infrastructure.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepository;

        public ReviewService(IReviewRepository reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }

        public async Task<Review> CreateAsync(Review review)
        {
            await _reviewRepository.AddAsync(review);
            return review;
        }

        public async Task<Review?> GetByIdAsync(string id) => await _reviewRepository.GetByIdAsync(id);

        public async Task<IEnumerable<Review>> GetByUserIdAsync(string userId)
            => await _reviewRepository.GetByUserIdAsync(userId);

        public async Task DeleteAsync(string id) => await _reviewRepository.DeleteAsync(id);
    }
}
