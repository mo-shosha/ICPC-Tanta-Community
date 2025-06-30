using Core.DTO;
using Core.Entities;
using Core.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ICPC_Tanta_Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StickyNoteController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public StickyNoteController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        //[Authorize]
        [HttpPost("add-sticky")]
        public async Task<IActionResult> Create([FromBody] StickyNoteCreateDto stickyNoteCreateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<string>.ErrorResponse("Validation failed."));

            try
            {
                var userName = User.Identity?.Name ?? "Anonymous";

                var sticky = new StickyNotes
                {
                    Content = stickyNoteCreateDto.Content,
                    AuthorName = userName
                };

                await _unitOfWork.stickyNoteRepository.AddAsync(sticky);
                await _unitOfWork.SaveChangesAsync();

                return Ok(ApiResponse<string>.SuccessResponse("Sticky note created successfully."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.ErrorResponse(ex.Message));
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var stickies = await _unitOfWork.stickyNoteRepository.GetAllAsync();

                if (stickies == null || !stickies.Any())
                {
                    return Ok(ApiResponse<string>.SuccessResponse("No sticky notes found."));
                }

                var stickiesDto = stickies.Select(s => new StickyNoteDto
                {
                    Content = s.Content,
                    AuthorName = s.AuthorName
                });

                return Ok(ApiResponse<IEnumerable<StickyNoteDto>>.SuccessResponse("Sticky notes retrieved successfully.",stickiesDto));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.ErrorResponse(ex.Message));
            }
        }
    }
}
