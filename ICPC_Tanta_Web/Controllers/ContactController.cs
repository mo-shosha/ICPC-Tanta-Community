using Core.DTO;
using Core.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ICPC_Tanta_Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly IContactUsServices _contactService;

        public ContactController(IContactUsServices contactService)
        {
            _contactService = contactService;
        }

        [HttpPost]
        public async Task<IActionResult> ContactUs([FromBody] ContactUsDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<string>.ErrorResponse("Invalid input."));

            await _contactService.HandleContactUsAsync(dto);

            return Ok(ApiResponse<string>.SuccessResponse("Your message has been sent successfully."));
        }
    }
}
