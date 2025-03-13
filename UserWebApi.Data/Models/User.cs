using System.ComponentModel.DataAnnotations;

namespace UserWebApi.Data.Models
{
	public class User
	{
		public int Id { get; set; }
		public string? FirstName { get; set; }
		public string? LastName { get; set; }

		[Required]
		[EmailAddress]
		public required string Email { get; set; }
	}
}
