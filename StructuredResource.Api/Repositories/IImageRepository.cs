using StructuredResource.Api.Models.Domain;

namespace StructuredResource.Api.Repositories
{
    public interface IImageRepository
    {
        Task<Image>Upload(Image image);
    }
}
