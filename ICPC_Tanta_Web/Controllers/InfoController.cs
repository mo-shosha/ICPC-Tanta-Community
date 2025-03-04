using Core.DTO;
using Core.DTO.InfoDTO;
using Core.Entities;
using Core.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ICPC_Tanta_Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InfoController : ControllerBase
    {
        private readonly IInfoService _infoService;

        public InfoController(IInfoService infoService)
        {
            _infoService = infoService;
        }

        [HttpGet]
        public async Task<IActionResult> GetInfo()
        {
            try
            {
                var info = await _infoService.GetInfAsync();
                if (info == null) return NotFound("No information found.");
                return Ok(ApiResponse<InfoDto>.SuccessResponse("Info retrieved successfully.",info));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.ErrorResponse(ex.Message));
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddInfo([FromForm] CreateInfoDto createInfoDto)
        {
            try
            {
                await _infoService.AddAsync(createInfoDto);
                return Ok(ApiResponse<string>.SuccessResponse("Info added successfully."));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ApiResponse<string>.ErrorResponse(ex.Message));
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ApiResponse<string>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.ErrorResponse(ex.Message));
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateInfo([FromForm] UpdateInfoDto updateInfoDto)
        {
            try
            {
                await _infoService.UpdateAsync(updateInfoDto);
                return Ok(ApiResponse<string>.SuccessResponse("Info updated successfully."));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiResponse<string>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.ErrorResponse(ex.Message));
            }
        }

    }
}
