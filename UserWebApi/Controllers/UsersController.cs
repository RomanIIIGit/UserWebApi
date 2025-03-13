using Microsoft.AspNetCore.Mvc;
using UserWebApi.Services.Dtos;
using UserWebApi.Services.Services;

namespace UserWebApi.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class UsersController : ControllerBase
	{
		private readonly IUserService _userService;

		public UsersController(IUserService userService)
		{
			_userService = userService;
		}

		[HttpGet]
		public async Task<IActionResult> GetUsers()
		{
			var users = await _userService.GetUsersAsync();
			return Ok(users);
		}

		[HttpGet("{email}")]
		public async Task<IActionResult> GetUser([FromQuery] string email)
		{
			var user = await _userService.GetUserByEmailAsync(email);
			if (user == null) 
				return NotFound();

			return Ok(user);
		}

		[HttpPost]
		public async Task<IActionResult> CreateUser(UserRequestDto userRequestDto)
		{
			if (!ModelState.IsValid) return BadRequest(ModelState);
			try
			{
				await _userService.CreateUserAsync(userRequestDto);
			}
			catch (ArgumentException ex)
			{
				return BadRequest(ex.Message);
			}
			return CreatedAtAction(nameof(GetUser), new { email = userRequestDto.Email }, userRequestDto);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateUser(int id, UserRequestDto userRequestDto)
		{
			if (!ModelState.IsValid) return BadRequest(ModelState);
			try
			{
				await _userService.UpdateUserAsync(id, userRequestDto);
			}
			catch (ArgumentException ex)
			{
				return BadRequest(ex.Message);
			}
			return NoContent();
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteUser(int id)
		{
			await _userService.DeleteUserAsync(id);
			return NoContent();
		}
	}
}
