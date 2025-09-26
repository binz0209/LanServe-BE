using LanServe.Domain.Entities;

namespace LanServe.Application.Interfaces.Repositories;

public interface IPaymentRepository : IGenericRepository<Payment>
{
    Task<IEnumerable<Payment>> GetByContractAsync(string contractId);
    Task<IEnumerable<Payment>> GetByStatusAsync(string status, int take = 50);
}
