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
    public class ProposalService : IProposalService
    {
        private readonly IProposalRepository _proposalRepository;

        public ProposalService(IProposalRepository proposalRepository)
        {
            _proposalRepository = proposalRepository;
        }

        public async Task<Proposal> CreateAsync(Proposal proposal)
        {
            await _proposalRepository.AddAsync(proposal);
            return proposal;
        }

        public async Task<Proposal?> GetByIdAsync(string id) => await _proposalRepository.GetByIdAsync(id);

        public async Task<IEnumerable<Proposal>> GetByProjectIdAsync(string projectId)
            => await _proposalRepository.GetByProjectIdAsync(projectId);

        public async Task UpdateAsync(Proposal proposal) => await _proposalRepository.UpdateAsync(proposal);

        public async Task DeleteAsync(string id) => await _proposalRepository.DeleteAsync(id);
    }
}
