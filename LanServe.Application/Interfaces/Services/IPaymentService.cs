using LanServe.Domain.Entities;

namespace LanServe.Application.Interfaces.Services;

public interface IPaymentService
{
    Task<Payment?> GetByIdAsync(string id);
    Task<IEnumerable<Payment>> GetByContractIdAsync(string contractId);
    Task<Payment> CreateAsync(Payment entity);
    Task<bool> UpdateStatusAsync(string id, string newStatus);
    Task<bool> DeleteAsync(string id);

    // Mock checkout (fake payment gateway)
    Task<Payment> MockCheckoutAsync(string contractId, decimal amount);
}
