using Microsoft.EntityFrameworkCore;
using StructuredResource.Api.Data;
using StructuredResource.Api.Models.Domain;

namespace StructuredResource.Api.Repositories
{
    public class MySQLRegionRepository : IRegionRepository
    {
        private readonly StructuredDbContext structuredDbContext;

        public MySQLRegionRepository(StructuredDbContext structuredDbContext)
        {
            this.structuredDbContext = structuredDbContext;
        }

        public async Task<Region> Create(Region region)
        {
            await structuredDbContext.AddAsync(region);
            await structuredDbContext.SaveChangesAsync();
            return region;
        }

        public async Task<Region?> DeleteRegionByIdAsync(Guid RegionId)
        {
            var existingRegion = structuredDbContext.Regions.FirstOrDefault(x => x.Id == RegionId);

            if (existingRegion == null)
            {
                return null;
            }

            structuredDbContext.Regions.Remove(existingRegion);
            await structuredDbContext.SaveChangesAsync();
            return existingRegion;
        }

        public async Task<List<Region>> GetAllAsync()
        {
            return await structuredDbContext.Regions.ToListAsync();
        }

        public async Task<Region?> GetRegionById(Guid regionId)
        {
            return await structuredDbContext.Regions.FirstOrDefaultAsync(x => x.Id == regionId);
        }

        public async Task<Region?> Update(Guid Id, Region region)
        {
            var existingRegion = await structuredDbContext.Regions.FirstOrDefaultAsync(x => x.Id == Id);

            if (existingRegion == null)
            {
                return null;
            }

            existingRegion.Code = region.Code;
            existingRegion.Name = region.Name;
            existingRegion.RegionImageUrl = region.RegionImageUrl;

            await structuredDbContext.SaveChangesAsync();
            return existingRegion;
        }
    }
}
      