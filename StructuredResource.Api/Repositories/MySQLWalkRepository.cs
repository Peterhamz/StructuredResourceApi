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

        public async Task<List<Walk>> GetAllAsync(String? filterOn = null, String? filterQuery = null,
            string? sortBy = null, bool isAscending = true, int pageNumber = 1, int pageSize = 1000)
        {
            var walks = structuredDbContext.Walks.Include("Region").Include("Difficulty").AsQueryable();

            // filtering 
            if(string.IsNullOrWhiteSpace(filterOn) == false && string.IsNullOrWhiteSpace(filterQuery) == false)
            {
                if(filterOn.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    walks = walks.Where(x => x.Name.Contains(filterQuery));
                }
            }

            //Sorting
            if(string.IsNullOrWhiteSpace(sortBy) == false)
            {
                if(sortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    walks = isAscending ? walks.OrderBy(x => x.Name) : walks.OrderByDescending(x => x.Name);
                } else if(sortBy.Equals("Length", StringComparison.OrdinalIgnoreCase))
                {
                    walks = isAscending ? walks.OrderBy(x => x.LengthInKm) : walks.OrderByDescending(x => x.LengthInKm);
                }
            }

            //Pagination
            var skipResult = (pageNumber - 1) * pageSize;

             
            return await walks.Skip(skipResult).Take(pageSize).ToListAsync();
            //return structuredDbContext.Walks.Include("Region").Include("Difficulty").ToListAsync();
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
