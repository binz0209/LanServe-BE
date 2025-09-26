using LanServe.Domain.Entities;
using MongoDB.Driver;

namespace LanServe.Infrastructure.Repositories;

public class PaymentRepository : GenericRepository<Payment>, IPaymentRepository
{
    public PaymentRepository(IMongoCollection<Payment> collection) : base(collection) { }

    public async Task<IEnumerable<Payment>> GetByContractAsync(string contractId)
        => await _collection.Find(p => p.ContractId == contractId).ToListAsync();

    public async Task<IEnumerable<Payment>> GetByStatusAsync(string status, int take = 50)
        => await _collection.Find(p => p.Status == status).Limit(take).ToListAsync();
}
