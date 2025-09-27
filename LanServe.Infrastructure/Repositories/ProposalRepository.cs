using LanServe.Application.Interfaces.Repositories;
using LanServe.Domain.Entities;
using MongoDB.Driver;

namespace LanServe.Infrastructure.Repositories;

public class ProposalRepository : IProposalRepository
{
    private readonly IMongoCollection<Proposal> _collection;

    public ProposalRepository(IMongoCollection<Proposal> collection)
    {
        _collection = collection;
    }

    public async Task<Proposal?> GetByIdAsync(string id)
        => await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task<IEnumerable<Proposal>> GetByProjectIdAsync(string projectId)
        => await _collection.Find(x => x.ProjectId == projectId)
                            .SortByDescending(x => x.CreatedAt)
                            .ToListAsync();

    public async Task<IEnumerable<Proposal>> GetByFreelancerIdAsync(string freelancerId)
        => await _collection.Find(x => x.FreelancerId == freelancerId)
                            .SortByDescending(x => x.CreatedAt)
                            .ToListAsync();

    public async Task<Proposal> InsertAsync(Proposal entity)
    {
        await _collection.InsertOneAsync(entity);
        return entity;
    }

    public async Task<bool> UpdateStatusAsync(string id, string status)
    {
        var update = Builders<Proposal>.Update.Set(x => x.Status, status);
        var result = await _collection.UpdateOneAsync(x => x.Id == id, update);
        return result.ModifiedCount > 0;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var result = await _collection.DeleteOneAsync(x => x.Id == id);
        return result.DeletedCount > 0;
    }
}
