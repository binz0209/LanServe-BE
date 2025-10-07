// LanServe.Application/Interfaces/Repositories/IWalletTransactionRepository.cs
using LanServe.Domain.Entities;

namespace LanServe.Application.Interfaces.Repositories;

public interface IWalletTransactionRepository
{
    Task InsertAsync(WalletTransaction txn, CancellationToken ct = default);
}
