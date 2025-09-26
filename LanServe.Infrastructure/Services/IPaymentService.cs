using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanServe.Infrastructure.Services
{
    public interface IPaymentService
    {
        Task<Payment> CreateAsync(Payment payment);
        Task<Payment?> GetByIdAsync(string id);
    }
}
