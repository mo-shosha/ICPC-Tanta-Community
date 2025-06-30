using Core.DTO.QAndADTO;
using Core.DTO;
using Core.Entities;
using Core.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ICPC_Tanta_Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QAndAController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public QAndAController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        //[Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateQuestion([FromBody] QAndACreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<string>.ErrorResponse("Validation failed."));

            try
            {
                var q = new QAndA
                {
                    Question = dto.Question,
                    CreatedAt = DateTime.UtcNow
                };

                await _unitOfWork.qAndARepository.AddAsync(q);
                await _unitOfWork.SaveChangesAsync();

                return Ok(ApiResponse<string>.SuccessResponse("Question submitted successfully."));
            }
            catch (Exception ex)
            {

                return StatusCode(500, ApiResponse<string>.ErrorResponse(ex.Message));
            }
        }

        [Authorize(Roles = "Admin,Instructor")]
        [HttpPatch("{id}/answer")]
        public async Task<IActionResult> AnswerQuestion(int id, [FromBody] QAndACreateAnsDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<string>.ErrorResponse("Validation failed."));

            try
            {
                var q = await _unitOfWork.qAndARepository.GetByIdAsync(id);
                if (q == null)
                    return NotFound(ApiResponse<string>.ErrorResponse("Question not found."));
                //if (!string.IsNullOrWhiteSpace(q.Answer))
                //    return BadRequest(ApiResponse<string>.ErrorResponse("Question has already been answered."));
                
                q.Answer = dto.Answer;
                q.AnsweredAt = DateTime.UtcNow;
                q.AnsweredBy = User.Identity?.Name??"Mohamed";

                _unitOfWork.qAndARepository.Update(q);
                await _unitOfWork.SaveChangesAsync();

                return Ok(ApiResponse<string>.SuccessResponse("Answer saved successfully."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.ErrorResponse($"Error: {ex.Message}"));
            }
        }


        [Authorize(Roles = "Admin,Instructor")]
        [HttpGet("filter")]
        public async Task<IActionResult> GetQuestions([FromQuery] string? filter)
        {
            try
            {
                var questions = await _unitOfWork.qAndARepository.GetAllAsync();

                IEnumerable<QAndA> result = questions;

                switch (filter?.ToLower())
                {
                    case "answered":
                        result = questions.Where(q => !string.IsNullOrWhiteSpace(q.Answer));
                        break;

                    case "unanswered":
                        result = questions.Where(q => string.IsNullOrWhiteSpace(q.Answer));
                        break;

                    case null:
                    case "all":
                        break;

                    default:
                        return BadRequest(ApiResponse<string>.ErrorResponse("Invalid filter. Use 'answered', 'unanswered', or 'all'."));
                }

                var message = filter switch
                {
                    "answered" => "Answered questions retrieved.",
                    "unanswered" => "Unanswered questions retrieved.",
                    _ => "All questions retrieved."
                };

                return Ok(ApiResponse<IEnumerable<QAndA>>.SuccessResponse(message, result));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.ErrorResponse($"Error: {ex.Message}"));
            }
        }


        [HttpGet("answered")]
        public async Task<IActionResult> GetAnsweredOnly()
        {
            try
            {
                var questions = await _unitOfWork.qAndARepository.GetAllAsync();

                var answered = questions
                    .Where(q => !string.IsNullOrWhiteSpace(q.Answer))
                    .Select(q => new QAndAReadDto
                    {
                        Question = q.Question,
                        Answer = q.Answer!,
                        AnsweredBy = q.AnsweredBy,
                        AnsweredAt = q.AnsweredAt!.Value,
                        CreatedAt = q.CreatedAt
                    });

                if (!answered.Any())
                {
                    return Ok(ApiResponse<IEnumerable<QAndAReadDto>>.SuccessResponse("No answered questions available.", answered));
                }

                return Ok(ApiResponse<IEnumerable<QAndAReadDto>>.SuccessResponse("Answered questions retrieved successfully.", answered));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.ErrorResponse($"Error: {ex.Message}"));
            }
        }


    }
}
