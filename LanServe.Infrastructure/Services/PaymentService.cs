using LanServe.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanServe.Infrastructure.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;

        public PaymentService(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        public async Task<Payment> CreateAsync(Payment payment)
        {
            await _paymentRepository.AddAsync(payment);
            return payment;
        }

        public async Task<Payment?> GetByIdAsync(string id)
            => await _paymentRepository.GetByIdAsync(id);
    }
}
