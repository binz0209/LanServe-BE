// LanServe.Api/Controllers/WalletsController.cs
using LanServe.Application.Interfaces.Repositories;
using LanServe.Domain.Entities;
using LanServe.Infrastructure.Data;              // 👈 dùng để query lịch sử (WalletTransactions)
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace LanServe.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WalletsController : ControllerBase
{
    private readonly IWalletRepository _wallets;
    private readonly IWalletTransactionRepository _walletTxns;
    private readonly IPaymentRepository _payments;
    private readonly MongoDbContext _ctx;      // 👈 thêm để truy vấn lịch sử

    public WalletsController(
        IWalletRepository wallets,
        IWalletTransactionRepository walletTxns,
        IPaymentRepository payments,
        MongoDbContext ctx)                     // 👈 inject context
    {
        _wallets = wallets;
        _walletTxns = walletTxns;
        _payments = payments;
        _ctx = ctx;
    }

    /// <summary>
    /// (Dev/Test) Lấy số dư ví theo userId.
    /// Production nên có /me lấy từ JWT.
    /// </summary>
    [HttpGet("{userId}")]
    public async Task<IActionResult> GetBalance(string userId, CancellationToken ct)
    {
        var w = await _wallets.GetOrCreateByUserAsync(userId, ct);
        return Ok(new { balance = w.Balance });
    }

    /// <summary>
    /// Trả về số dư + 10 giao dịch gần nhất để FE hiển thị nhanh.
    /// </summary>
    [HttpGet("summary")]
    public async Task<IActionResult> GetSummary([FromQuery] string userId, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(userId)) return BadRequest("Missing userId");
        var w = await _wallets.GetOrCreateByUserAsync(userId, ct);

        // Lấy 10 giao dịch gần nhất (mới -> cũ)
        var items = await _ctx.WalletTransactions
            .Find(x => x.UserId == userId)
            .SortByDescending(x => x.CreatedAt)
            .Limit(10)
            .ToListAsync(ct);

        var recent = items.Select(x => new
        {
            id = x.Id,
            userId = x.UserId,
            walletId = x.WalletId,
            type = x.Type,
            amount = x.Amount,
            balanceAfter = x.BalanceAfter,
            note = x.Note,
            createdAt = x.CreatedAt
        });

        return Ok(new { balance = w.Balance, recent });
    }

    /// <summary>
    /// Danh sách giao dịch ví, có sort theo ngày.
    /// sort = "desc" (mặc định) hoặc "asc"; take tối đa 100.
    /// </summary>
    [HttpGet("history")]
    public async Task<IActionResult> GetHistory(
        [FromQuery] string userId,
        [FromQuery] int take = 20,
        [FromQuery] string sort = "desc",
        CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(userId)) return BadRequest("Missing userId");
        take = Math.Clamp(take, 1, 100);

        var filter = Builders<WalletTransaction>.Filter.Eq(x => x.UserId, userId);
        var sortDesc = string.Equals(sort, "asc", StringComparison.OrdinalIgnoreCase)
            ? Builders<WalletTransaction>.Sort.Ascending(x => x.CreatedAt)
            : Builders<WalletTransaction>.Sort.Descending(x => x.CreatedAt);

        var list = await _ctx.WalletTransactions
            .Find(filter)
            .Sort(sortDesc)
            .Limit(take)
            .ToListAsync(ct);

        var items = list.Select(x => new
        {
            id = x.Id,
            userId = x.UserId,
            walletId = x.WalletId,
            type = x.Type,
            amount = x.Amount,
            balanceAfter = x.BalanceAfter,
            note = x.Note,
            createdAt = x.CreatedAt
        });

        return Ok(items);
    }

    /// <summary>
    /// FE gọi ở /payment-success để XÁC NHẬN nạp tiền theo orderId (vnp_TxnRef).
    /// Trả về trạng thái giao dịch + số dư ví hiện tại.
    /// </summary>
    [HttpGet("topup-result")]
    public async Task<IActionResult> GetTopupResult([FromQuery] string orderId, [FromQuery] string userId, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(orderId)) return BadRequest("Missing orderId (vnp_TxnRef)");
        if (string.IsNullOrWhiteSpace(userId)) return BadRequest("Missing userId");

        var payment = await _payments.GetByTxnRefAsync(orderId, ct);
        if (payment == null) return NotFound(new { message = "Payment not found" });

        if (payment.UserId != userId)
            return BadRequest(new { message = "Payment does not belong to this user" });

        var wallet = await _wallets.GetOrCreateByUserAsync(payment.UserId, ct);

        return Ok(new
        {
            orderId = payment.Vnp_TxnRef,
            status = payment.Status,              // "Paid"/"Failed"/"Pending"
            responseCode = payment.Vnp_ResponseCode,
            walletBalance = wallet.Balance,
            amount = payment.Amount,
            paidAt = payment.PaidAt,
        });
    }
}
