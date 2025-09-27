using LanServe.Application.Interfaces.Services;
using LanServe.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LanServe.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentsController : ControllerBase
{
    private readonly IPaymentService _svc;
    public PaymentsController(IPaymentService svc) { _svc = svc; }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id) => Ok(await _svc.GetByIdAsync(id));

    [Authorize]
    [HttpGet("by-contract/{contractId}")]
    public async Task<IActionResult> ByContract(string contractId) => Ok(await _svc.GetByContractIdAsync(contractId));

    public record CheckoutDto(string ContractId, decimal Amount);
    [Authorize]
    [HttpPost("mock-checkout")]
    public async Task<IActionResult> MockCheckout([FromBody] CheckoutDto dto)
        => Ok(await _svc.MockCheckoutAsync(dto.ContractId, dto.Amount));

    public record UpdateStatusDto(string Status);
    [Authorize(Roles = "Admin")]
    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdateStatus(string id, [FromBody] UpdateStatusDto dto)
        => Ok(await _svc.UpdateStatusAsync(id, dto.Status));

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id) => Ok(await _svc.DeleteAsync(id));
}
