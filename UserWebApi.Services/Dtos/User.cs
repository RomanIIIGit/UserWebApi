using System.ComponentModel.DataAnnotations;

namespace UserWebApi.Services.Dtos
{
    public class UserBaseDto
    {
		public string? FirstName { get; set; }
		public string? LastName { get; set; }

		[Required(ErrorMessage = "Email is Required")]
		[EmailAddress(ErrorMessage = "Invalid Email")]
		public required string Email { get; set; }
	}

	public class UserRequestDto: UserBaseDto
	{
	}

	public class UserResponseDto : UserBaseDto
	{
		public int Id { get; set; }
	}
}
