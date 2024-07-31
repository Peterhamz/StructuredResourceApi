using StructuredResource.Api.Models.Domain;
using StructuredResource.Api.Models.DTO;

namespace StructuredResource.Api.Repositories
{
    public interface IWalkRepository
    {
       Task<Walk> CreateAsync(Walk walk);
        Task<List<Walk>> GetAllAsync(string? filterOn = null, string? filterQuery = null,
            string? sortBy = null, bool isAscending = true, int pageNumber = 1, int pageSize = 1000);
        Task<Walk?> GetWalkByIdAsync(Guid walkId);

        Task<Walk?> UpdateWalkAsync(Guid id, Walk walk);

        Task<Walk?> DeleteWalkByIdAsync(Guid id);   
    }
}
