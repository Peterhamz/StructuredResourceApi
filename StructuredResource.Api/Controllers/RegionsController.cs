using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using StructuredResource.Api.Data;
using StructuredResource.Api.Models.Domain;
using StructuredResource.Api.Models.DTO;
using StructuredResource.Api.Repositories;

namespace StructuredResource.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly StructuredDbContext dbContext;
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;

        public RegionsController(StructuredDbContext dbContext, IRegionRepository regionRepository, 
            IMapper mapper)
        {
            this.dbContext = dbContext;
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }

        // Get All Region method and controller
        [HttpGet]
        public async Task<IActionResult> GetAllRegion()
        {
            // get the entity from the database

            var regionsDomain = await regionRepository.GetAllAsync();

            // map the gotten data from damain model to DTO

            //var regionDto = new List<RegionDto>();
            //foreach (var regionDomain in regionsDomain) 
            //{
            //    regionDto.Add(new RegionDto
            //    {
            //        Id = regionDomain.Id,
            //        Name = regionDomain.Name,
            //        Code = regionDomain.Code,
            //        RegionImageUrl = regionDomain.RegionImageUrl
            //    });
            //}
            return Ok(mapper.Map<List<RegionDto>>(regionsDomain)); 
        }

        // Get Region by Id
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetRegionById([FromRoute] Guid id)
        {
            // var region = dbContext.Regions.Find(id);
            //get the entity from the database
            var regionDomain = await regionRepository.GetRegionById(id);
            //check if it exist
            if (regionDomain == null)
            {
                return NotFound();
            }
            // map domain model to dto
            return Ok(mapper.Map<RegionDto>(regionDomain));
        }


        //Create a region Controller and implementation
        [HttpPost]
        public async Task<IActionResult> CreateRegion([FromBody] AddRegionRequestDto addRegionRequestDto)
        {

            if (ModelState.IsValid)
            {
                // Map or convert Dto to domain model
                var regionDomain = mapper.Map<Region>(addRegionRequestDto);


                //use Domain model to create 
                regionDomain = await regionRepository.Create(regionDomain);
                //map domain model back to Dto so we can return DTO
                var regionDto = mapper.Map<RegionDto>(regionDomain);

                return CreatedAtAction(nameof(GetRegionById), new { id = regionDto.Id }, regionDto);
            } else
            {
                return BadRequest(ModelState);
            }
          
        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdateRegion([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {
            if (ModelState.IsValid)
            {
                var regeionDomainModel = mapper.Map<Region>(updateRegionRequestDto);

                // Check if region exist
                regeionDomainModel = await regionRepository.Update(id, regeionDomainModel);

                if (regeionDomainModel == null)
                {
                    return NotFound();
                }


                // convert domain model to Dto
                var regeionDto = mapper.Map<RegionDto>(regeionDomainModel);

                return Ok(regeionDto);
            } else
            {
                return BadRequest(ModelState);
            }
          
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteRegion([FromRoute] Guid id)
        {
            // check if region exist
            var regionDomainModel = await regionRepository.DeleteRegionByIdAsync(id);
            if(regionDomainModel == null)
            {
                return NotFound();
            }
            // Map domain model to Dto
            var regeionDto = mapper.Map<RegionDto>(regionDomainModel);
             
            return Ok(regeionDto);
        }


    } 
}
