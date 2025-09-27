using LanServe.Application.Interfaces.Repositories;
using LanServe.Application.Interfaces.Services;
using LanServe.Domain.Entities;

namespace LanServe.Application.Services;

public class PaymentService : IPaymentService
{
    private readonly IPaymentRepository _repo;

    public PaymentService(IPaymentRepository repo)
    {
        _repo = repo;
    }

    public Task<Payment?> GetByIdAsync(string id)
        => _repo.GetByIdAsync(id);

    public Task<IEnumerable<Payment>> GetByContractIdAsync(string contractId)
        => _repo.GetByContractIdAsync(contractId);

    public Task<Payment> CreateAsync(Payment entity)
    {
        entity.CreatedAt = DateTime.UtcNow;
        entity.Status = "Pending";
        return _repo.InsertAsync(entity);
    }

    public Task<bool> UpdateStatusAsync(string id, string newStatus)
        => _repo.UpdateStatusAsync(id, newStatus);

    public Task<bool> DeleteAsync(string id)
        => _repo.DeleteAsync(id);

    public async Task<Payment> MockCheckoutAsync(string contractId, decimal amount)
    {
        var payment = new Payment
        {
            ContractId = contractId,
            Amount = amount,
            Status = "Paid",
            CreatedAt = DateTime.UtcNow
        };
        return await _repo.InsertAsync(payment);
    }
}
