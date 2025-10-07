using LanServe.Application.Interfaces.Repositories;
using LanServe.Domain.Entities;
using MongoDB.Driver;

namespace LanServe.Infrastructure.Repositories;

public class WalletTransactionRepository : IWalletTransactionRepository
{
    private readonly IMongoCollection<WalletTransaction> _collection;

    public WalletTransactionRepository(IMongoCollection<WalletTransaction> collection)
    {
        _collection = collection;
    }

    public async Task InsertAsync(WalletTransaction txn, CancellationToken ct = default)
        => await _collection.InsertOneAsync(txn, cancellationToken: ct);
}
