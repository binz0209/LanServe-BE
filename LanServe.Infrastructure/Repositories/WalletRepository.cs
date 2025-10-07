using LanServe.Application.Interfaces.Repositories;
using LanServe.Domain.Entities;
using MongoDB.Driver;

namespace LanServe.Infrastructure.Repositories;

public class WalletRepository : IWalletRepository
{
    private readonly IMongoCollection<Wallet> _collection;

    public WalletRepository(IMongoCollection<Wallet> collection)
    {
        _collection = collection;
    }

    public async Task<Wallet> GetOrCreateByUserAsync(string userId, CancellationToken ct = default)
    {
        var wallet = await _collection.Find(x => x.UserId == userId).FirstOrDefaultAsync(ct);
        if (wallet != null) return wallet;

        wallet = new Wallet { UserId = userId, Balance = 0 };
        await _collection.InsertOneAsync(wallet, cancellationToken: ct);
        return wallet;
    }

    public async Task UpdateAsync(Wallet wallet, CancellationToken ct = default)
        => await _collection.ReplaceOneAsync(x => x.Id == wallet.Id, wallet, cancellationToken: ct);
}
