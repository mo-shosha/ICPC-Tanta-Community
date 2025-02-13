using Core.DTO.AccountDTO;
using Core.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
                return Ok(instructors);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("users")]
        public async Task<ActionResult<IEnumerable<Userinfo>>> GetAllUsers()
        {
            try
            {
                var users = await _userServices.GetAllUsers();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("users/with-rating")]
        public async Task<ActionResult<IEnumerable<UserRatingDto>>> GetAllUsersWithRating()
        {
            try
            {
                var usersWithRating = await _userServices.GetAllUsersWithRating();
                return Ok(usersWithRating);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("Instructors/with-rating")]
        public async Task<ActionResult<IEnumerable<UserRatingDto>>> GetAllInstructorsWithRating()
        {
            try
            {
                var usersWithRating = await _userServices.GetAllInstructorWithRating();
                return Ok(usersWithRating);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("user/with-ranking")]
        public async Task<IActionResult> GetUserRanking([FromQuery] string userId)
        {
            try
            {
                if (string.IsNullOrEmpty(userId))
                {
                    return BadRequest(new { message = "UserId is required." });
                }

                var sortedUsers = await _userServices.GetAllUsersWithRating();

                int ranking =  _userServices.GetUserRanking(userId, sortedUsers);

                if (ranking == -1)
                {
                    return NotFound(new { message = "User not found." });
                }

                return Ok(new { userId, ranking });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
   
    }
}
