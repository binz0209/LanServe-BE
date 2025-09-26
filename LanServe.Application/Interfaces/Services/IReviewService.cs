using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LanServe.Domain.Entities;

namespace LanServe.Application.Interfaces.Services
{
    public interface IReviewService
    {
        Task<Review> CreateAsync(Review review);
        Task<Review?> GetByIdAsync(string id);
        Task<IEnumerable<Review>> GetByUserIdAsync(string userId);
        Task DeleteAsync(string id);
    }
}
