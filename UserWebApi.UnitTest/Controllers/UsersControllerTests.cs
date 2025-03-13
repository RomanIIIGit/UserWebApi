using Moq;
using UserWebApi.Controllers;
using UserWebApi.Services.Dtos;
using UserWebApi.Services.Services;
using Microsoft.AspNetCore.Mvc;

namespace UserWebApi.UnitTest.Controllers
{
	[TestClass]
	public class UsersControllerTests
	{
		private UsersController _controller;
		private Mock<IUserService> _mockUserService;

		[TestInitialize]
		public void Setup()
		{
			_mockUserService = new Mock<IUserService>();
			_controller = new UsersController(_mockUserService.Object);
		}

		[TestMethod]
		public async Task GetUsers_ReturnsOkResult_WithMockData()
		{
			// Arrange
			var mockUsers = new List<UserResponseDto>
			{
				new UserResponseDto { Id = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com" },
				new UserResponseDto { Id = 2, FirstName = "Jane", LastName = "Smith", Email = "jane.smith@example.com" }
			};
			_mockUserService.Setup(service => service.GetUsersAsync()).ReturnsAsync(mockUsers);

			// Act
			var result = await _controller.GetUsers();

			// Assert
			Assert.IsInstanceOfType(result, typeof(OkObjectResult));
			var okResult = result as OkObjectResult;
			var users = okResult.Value as List<UserResponseDto>;
			Assert.AreEqual(2, users.Count);
		}

		[TestMethod]
		public async Task GetUser_ReturnsOkResult_ForExistingUser()
		{
			// Arrange
			var mockUser = new UserResponseDto { Id = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com" };
			_mockUserService.Setup(service => service.GetUserByEmailAsync("john.doe@example.com")).ReturnsAsync(mockUser);

			// Act
			var result = await _controller.GetUser("john.doe@example.com");

			// Assert
			Assert.IsInstanceOfType(result, typeof(OkObjectResult));
			var okResult = result as OkObjectResult;
			var user = okResult.Value as UserResponseDto;
			Assert.IsNotNull(user);
			Assert.AreEqual("John", user.FirstName);
		}

		[TestMethod]
		public async Task GetUser_ReturnsNotFound_ForNonExistingUser()
		{
			_mockUserService.Setup(service => service.GetUserByEmailAsync("nonexistent@example.com")).ReturnsAsync((UserResponseDto)null);

			var result = await _controller.GetUser("nonexistent@example.com");

			Assert.IsInstanceOfType(result, typeof(NotFoundResult));
		}

		[TestMethod]
		public async Task CreateUser_ReturnsCreatedAtActionResult()
		{
			var userRequestDto = new UserRequestDto { Email = "newuser@example.com", FirstName = "New", LastName = "User" };
			var result = await _controller.CreateUser(userRequestDto);

			Assert.IsInstanceOfType(result, typeof(CreatedAtActionResult));
			var createdAtActionResult = result as CreatedAtActionResult;
			Assert.AreEqual("GetUser", createdAtActionResult.ActionName);
		}

		[TestMethod]
		public async Task UpdateUser_ReturnsNoContentResult()
		{
			var userRequestDto = new UserRequestDto { Email = "john.doe@example.com", FirstName = "John", LastName = "Doe" };

			var result = await _controller.UpdateUser(1, userRequestDto);

			Assert.IsInstanceOfType(result, typeof(NoContentResult));
		}

		[TestMethod]
		public async Task DeleteUser_ReturnsNoContentResult()
		{
			// Act
			var result = await _controller.DeleteUser(1);

			// Assert
			Assert.IsInstanceOfType(result, typeof(NoContentResult));
		}
	}
}
