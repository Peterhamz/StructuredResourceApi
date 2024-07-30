using StructuredResource.Api.Models.Domain;

namespace StructuredResource.Api.Repositories
{
    public interface IRegionRepository
    {
       Task<List<Region>> GetAllAsync();

       Task<Region?> GetRegionById(Guid regionId);

       Task<Region> Create(Region region);

        Task<Region?> Update(Guid Id, Region region);

        Task<Region?> DeleteRegionByIdAsync(Guid RegionId);
    }
}
