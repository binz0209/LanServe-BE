using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LanServe.Domain.Entities;

namespace LanServe.Application.Interfaces.Services
{
    public interface IContractService
    {
        Task<Contract> CreateAsync(Contract contract);
        Task<Contract?> GetByIdAsync(string id);
        Task<IEnumerable<Contract>> GetAllAsync();
        Task UpdateAsync(Contract contract);
        Task DeleteAsync(string id);
    }
}
