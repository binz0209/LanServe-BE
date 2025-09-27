using LanServe.Domain.Entities;

namespace LanServe.Application.Interfaces.Repositories;

public interface IPaymentRepository
{
    Task<Payment?> GetByIdAsync(string id);
    Task<IEnumerable<Payment>> GetByContractIdAsync(string contractId);
    Task<Payment> InsertAsync(Payment entity);
    Task<bool> UpdateStatusAsync(string id, string newStatus);
    Task<bool> DeleteAsync(string id);
}
