using StructuredResource.Api.Models.Domain;
using StructuredResource.Api.Models.DTO;

namespace StructuredResource.Api.Repositories
{
    public interface IWalkRepository
    {
       Task<Walk> CreateAsync(Walk walk);
        Task<List<Walk>> GetAllAsync();
        Task<Walk?> GetWalkByIdAsync(Guid walkId);

        Task<Walk?> UpdateWalkAsync(Guid id, Walk walk);

        Task<Walk?> DeleteWalkByIdAsync(Guid id);   
    }
}
