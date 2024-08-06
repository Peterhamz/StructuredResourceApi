using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StructuredResource.Api.CustomActionFilter;
using StructuredResource.Api.Models.Domain;
using StructuredResource.Api.Models.DTO;
using StructuredResource.Api.Repositories;

namespace StructuredResource.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IWalkRepository walkRepository;

        public WalksController(IMapper mapper, IWalkRepository walkRepository)
        {
            this.mapper = mapper;
            this.walkRepository = walkRepository;
        }
        //Create Walks
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody] AddWalksRequestDto addWalksRequestDto)
        {
           
                var walkDomain = mapper.Map<Walk>(addWalksRequestDto);

                await walkRepository.CreateAsync(walkDomain);

                return Ok(mapper.Map<WalkDto>(walkDomain));        

        }

        //Get all walks 
        [HttpGet]
        public async Task<IActionResult> GetWalks([FromQuery] string? filterOn, [FromQuery] string? filterQuery,
            [FromQuery] string? sortBy, [FromQuery] bool? isAscending,
            [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 1000 )
        {
            var walkDomain = await walkRepository.GetAllAsync(filterOn, filterQuery, sortBy, isAscending ?? true, 
                pageNumber, pageSize);

            throw new Exception("This is a new exception");

            return Ok(mapper.Map<List<WalkDto>>(walkDomain));
        }


        // Get Walks by ID
        [HttpGet]
        [Route("{id:Guid}")] 
        public async Task<IActionResult> GetWalkByIdAsync([FromRoute] Guid id)
        {
            var walkDomain = await walkRepository.GetWalkByIdAsync(id);

            if (walkDomain == null)
            {
                return NotFound();  
            }

            return Ok(mapper.Map<WalkDto>(walkDomain));
        }

        // Update Walk by Id

        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        public async Task<IActionResult> UpdateWalkById([FromRoute] Guid id, [FromBody] UpdateWalkDto updateWalkDto)
        {
                var walkDomain = mapper.Map<Walk>(updateWalkDto);

                walkDomain = await walkRepository.UpdateWalkAsync(id, walkDomain);

                if (walkDomain == null)
                {
                    return NotFound();
                }

                return Ok(mapper.Map<WalkDto>(walkDomain));
        }

        // Delete by Id
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteWalkById([FromRoute] Guid id)
        {
            var deletedWalk = await walkRepository.DeleteWalkByIdAsync(id);
            if(deletedWalk == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<WalkDto>(deletedWalk));

        }

    }

}
