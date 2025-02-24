using Core.DTO.AccountDTO;
using Core.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Core.DTO;
namespace ICPC_Tanta_Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserServices _userServices;

        public UserController(IUserServices userServices)
        {
            _userServices = userServices;
        }

        [HttpGet("instructors")]
        public async Task<ActionResult<IEnumerable<Userinfo>>> GetAllInstructors()
        {
            try
            {
                var instructors = await _userServices.GetAllInstructors();
                return Ok(ApiResponse<IEnumerable<Userinfo>>.SuccessResponse("Instructors retrieved successfully.", instructors));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.ErrorResponse(ex.Message));
            }
        }


        [HttpGet("users")]
        public async Task<ActionResult<IEnumerable<Userinfo>>> GetAllUsers()
        {
            try
            {
                var users = await _userServices.GetAllUsers();
                return Ok(ApiResponse<IEnumerable<Userinfo>>.SuccessResponse("Users retrieved successfully.", users));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.ErrorResponse(ex.Message));
            }
        }


        [HttpGet("users/with-rating")]
        public async Task<ActionResult<IEnumerable<UserRatingDto>>> GetAllUsersWithRating()
        {
            try
            {
                var usersWithRating = await _userServices.GetAllUsersWithRating();
                return Ok(ApiResponse<IEnumerable<UserRatingDto>>.SuccessResponse("Users with rating retrieved successfully.", usersWithRating));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.ErrorResponse(ex.Message));
            }
        }


        [HttpGet("Instructors/with-rating")]
        public async Task<ActionResult<IEnumerable<UserRatingDto>>> GetAllInstructorsWithRating()
        {
            try
            {
                var instructorsWithRating = await _userServices.GetAllInstructorWithRating();
                return Ok(ApiResponse<IEnumerable<UserRatingDto>>.SuccessResponse("Instructors with rating retrieved successfully.", instructorsWithRating));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.ErrorResponse(ex.Message));
            }
        }


        [HttpGet("user/with-ranking")]
        public async Task<IActionResult> GetUserRanking([FromQuery] string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return BadRequest(ApiResponse<string>.ErrorResponse("UserId is required."));

            try
            {
                var sortedUsers = await _userServices.GetAllUsersWithRating();
                int ranking = _userServices.GetUserRanking(userId, sortedUsers);

                if (ranking == -1)
                    return NotFound(ApiResponse<string>.ErrorResponse("User not found."));

                return Ok(ApiResponse<object>.SuccessResponse("User ranking retrieved successfully.", new { userId, ranking }));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.ErrorResponse(ex.Message));
            }
        }
   
    }
}
