using LanServe.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanServe.Infrastructure.Services
{
    public class SkillService : ISkillService
    {
        private readonly ISkillRepository _skillRepository;

        public SkillService(ISkillRepository skillRepository)
        {
            _skillRepository = skillRepository;
        }

        public async Task<Skill> CreateAsync(Skill skill)
        {
            await _skillRepository.AddAsync(skill);
            return skill;
        }

        public async Task<IEnumerable<Skill>> GetAllAsync() => await _skillRepository.GetAllAsync();

        public async Task DeleteAsync(string id) => await _skillRepository.DeleteAsync(id);
    }
}
