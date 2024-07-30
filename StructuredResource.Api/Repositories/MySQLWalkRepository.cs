using Microsoft.EntityFrameworkCore;
using StructuredResource.Api.Data;
using StructuredResource.Api.Models.Domain;
using StructuredResource.Api.Models.DTO;

namespace StructuredResource.Api.Repositories
{
    public class MySQLWalkRepository : IWalkRepository
    {
        private readonly StructuredDbContext structuredDbContext;

        public MySQLWalkRepository(StructuredDbContext structuredDbContext)
        {
            this.structuredDbContext = structuredDbContext;
        }
        public async Task<Walk> CreateAsync(Walk walk)
        {
          await structuredDbContext.Walks.AddAsync(walk);
            await structuredDbContext.SaveChangesAsync();
            return walk;
        }

        public async Task<Walk?> DeleteWalkByIdAsync(Guid id)
        {
            var existingWalk = structuredDbContext.Walks.FirstOrDefault(x => x.Id == id);
            if (existingWalk == null)
            {
                return null;
            }
            structuredDbContext.Walks.Remove(existingWalk);
            await structuredDbContext.SaveChangesAsync();
            return existingWalk;
        }

        public Task<List<Walk>> GetAllAsync()
        {
            return structuredDbContext.Walks.Include("Region").Include("Difficulty").ToListAsync();
        }

        public async Task<Walk?> GetWalkByIdAsync(Guid walkId)
        {
            return await structuredDbContext
                .Walks
                .Include("Region")
                .Include("Difficulty")
                .FirstOrDefaultAsync( x => x.Id == walkId);
        }

        public async Task<Walk?> UpdateWalkAsync(Guid id, Walk walk)
        {
            var existingWalk = await structuredDbContext.Walks.FirstOrDefaultAsync(x => x.Id == id); 
            if (existingWalk == null)
            {
                return null;
            }

            existingWalk.DifficultyId = walk.DifficultyId;
            existingWalk.Description = walk.Description;
            existingWalk.Name = walk.Name;
            existingWalk.WalkImageUrl = walk.WalkImageUrl;
            existingWalk.RegionId = walk.RegionId;
            existingWalk.LengthInKm = walk.LengthInKm;
            await structuredDbContext.SaveChangesAsync();

            return existingWalk;
        }
    }
}
