using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StructuredResource.Api.Models.Domain;
using StructuredResource.Api.Models.DTO;
using StructuredResource.Api.Repositories;

namespace StructuredResource.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImageRepository imageRepository;

        public ImagesController(IImageRepository imageRepository)
        {
            this.imageRepository = imageRepository;
        }

        [HttpPost]
        [Route("Upload")]
        public async Task<IActionResult> Upload([FromForm] ImageRequestDto imageRequestDto)
        {
            ValidateFileUpload(imageRequestDto);

            if (ModelState.IsValid)
            {

                //Convert Dto to Domain Model

                var imageDomainModel = new Image
                {
                    File = imageRequestDto.File,
                    FileDescription = imageRequestDto.FileDescription,
                    FileExtension = Path.GetExtension(imageRequestDto.File.FileName),
                    FileSizeInBytes = imageRequestDto.File.Length,
                    FileName = imageRequestDto.FileName,
                };
               //  Use repository to upload images

                await imageRepository.Upload(imageDomainModel);
                return Ok(imageDomainModel);
            }
            return BadRequest(ModelState);    
        }

        private void ValidateFileUpload(ImageRequestDto imageRequestDto)
        {
            var allowedExtensions = new String[] {".jpg",".png",".jpeg" };

            if(!allowedExtensions.Contains(Path.GetExtension(imageRequestDto.File.FileName)))
            {
                ModelState.AddModelError("file", "Unsorported file extension");
            }
            if(imageRequestDto.File.Length > 10485760)
            {
                ModelState.AddModelError("file", "file size is greater than 10mb, please upload a smaller size file");
            }

        }
    }
}
