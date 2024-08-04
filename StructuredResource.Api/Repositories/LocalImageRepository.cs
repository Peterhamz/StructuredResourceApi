using StructuredResource.Api.Data;
using StructuredResource.Api.Models.Domain;

namespace StructuredResource.Api.Repositories
{
    public class LocalImageRepository : IImageRepository
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IHttpContextAccessor contextAccessor;
        private readonly StructuredDbContext structuredDbContext;

        public LocalImageRepository(IWebHostEnvironment webHostEnvironment,
            IHttpContextAccessor contextAccessor, StructuredDbContext structuredDbContext)
        {
            this.webHostEnvironment = webHostEnvironment;
            this.contextAccessor = contextAccessor;
            this.structuredDbContext = structuredDbContext;
        }
        public async Task<Image> Upload(Image image)
        {

            var localFilePath = Path.Combine(webHostEnvironment.ContentRootPath, "Images",
                image.FileName, image.FileExtension);

            // upload image to local path
            using var stream = new FileStream(localFilePath, FileMode.Create);
            await image.File.CopyToAsync(stream);

            //the local path location

            var urlFilePath = $"{contextAccessor.HttpContext.Request.Scheme}://{contextAccessor.HttpContext.Request.Host}{contextAccessor.HttpContext.Request.PathBase}/Images/{image.FileName}{image.FileExtension}";

            image.FilePath = urlFilePath;

            // save to database in image Table

            await structuredDbContext.Images.AddAsync(image);
            await structuredDbContext.SaveChangesAsync();

            return image;
        }
    }

}
