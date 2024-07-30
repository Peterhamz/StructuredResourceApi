using System.ComponentModel.DataAnnotations;

namespace StructuredResource.Api.Models.DTO
{
    public class AddRegionRequestDto
    {
        [Required]
        [MaxLength(3, ErrorMessage = "Code has to be a Max of 3 characters")]
        [MinLength(3, ErrorMessage = "Code has to be a min of 3 characters")]
        public string Code { get; set; }

        [Required]
        [MaxLength(100, ErrorMessage = "Name has to be a Max of 100 characters")]
        public string Name { get; set; }
        public string? RegionImageUrl { get; set; }
    }
}
