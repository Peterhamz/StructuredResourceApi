using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace StructuredResource.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAllStudents() 
        {
            string[] students = new string[] {"Peter", "John", "James", "Samson"};

            return Ok(students);
        }
    }
}
