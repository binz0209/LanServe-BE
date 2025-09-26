using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanServe.Infrastructure.Services
{
    public interface ISkillService
    {
        Task<Skill> CreateAsync(Skill skill);
        Task<IEnumerable<Skill>> GetAllAsync();
        Task DeleteAsync(string id);
    }

}
