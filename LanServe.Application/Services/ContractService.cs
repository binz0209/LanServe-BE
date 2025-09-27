using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LanServe.Application.Interfaces.Services;
using LanServe.Domain.Entities;
using LanServe.Application.Interfaces.Repositories;

namespace LanServe.Infrastructure.Services
{
    public class ContractService : IContractService
    {
        private readonly IContractRepository _contractRepository;

        public ContractService(IContractRepository contractRepository)
        {
            _contractRepository = contractRepository;
        }

        public async Task<Contract> CreateAsync(Contract contract)
        {
            await _contractRepository.AddAsync(contract);
            return contract;
        }

        public async Task<Contract?> GetByIdAsync(string id) => await _contractRepository.GetByIdAsync(id);

        public async Task<IEnumerable<Contract>> GetAllAsync() => await _contractRepository.GetAllAsync();

        public async Task UpdateAsync(Contract contract)  => await _contractRepository.UpdateAsync(contract.Id, contract);

        public async Task DeleteAsync(string id) => await _contractRepository.DeleteAsync(id);
    }
}
