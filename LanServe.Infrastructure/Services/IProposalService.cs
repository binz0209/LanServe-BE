using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanServe.Infrastructure.Services
{
    public interface IProposalService
    {
        Task<Proposal> CreateAsync(Proposal proposal);
        Task<Proposal?> GetByIdAsync(string id);
        Task<IEnumerable<Proposal>> GetByProjectIdAsync(string projectId);
        Task UpdateAsync(Proposal proposal);
        Task DeleteAsync(string id);
    }

}
