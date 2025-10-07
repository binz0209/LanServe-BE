// LanServe.Application/Interfaces/Repositories/IWalletRepository.cs
using LanServe.Domain.Entities;

namespace LanServe.Application.Interfaces.Repositories;

public interface IWalletRepository
{
    Task<Wallet> GetOrCreateByUserAsync(string userId, CancellationToken ct = default);
    Task UpdateAsync(Wallet wallet, CancellationToken ct = default);
}
