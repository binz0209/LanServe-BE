using LanServe.Application.Interfaces.Repositories;
using LanServe.Domain.Entities;
using MongoDB.Driver;

namespace LanServe.Infrastructure.Repositories;

public class PaymentRepository : IPaymentRepository
{
    private readonly IMongoCollection<Payment> _collection;

    public PaymentRepository(IMongoCollection<Payment> collection)
    {
        _collection = collection;
    }

    public async Task<Payment?> GetByIdAsync(string id)
        => await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task<IEnumerable<Payment>> GetByContractIdAsync(string contractId)
        => await _collection.Find(x => x.ContractId == contractId)
                            .SortByDescending(x => x.CreatedAt)
                            .ToListAsync();

    public async Task<Payment> InsertAsync(Payment entity)
    {
        await _collection.InsertOneAsync(entity);
        return entity;
    }

    public async Task<bool> UpdateStatusAsync(string id, string newStatus)
    {
        var update = Builders<Payment>.Update.Set(x => x.Status, newStatus);
        var result = await _collection.UpdateOneAsync(x => x.Id == id, update);
        return result.ModifiedCount > 0;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var result = await _collection.DeleteOneAsync(x => x.Id == id);
        return result.DeletedCount > 0;
    }
}
