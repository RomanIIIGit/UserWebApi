using Microsoft.EntityFrameworkCore;
using UserWebApi.Data;
using UserWebApi.Data.Models;
using UserWebApi.Repositories.Base;

namespace UserWebApi.Repositories
{
	public interface IUserRepository : IBaseRepository<User>
	{
		Task<User?> GetByEmailAsync(string email);
		Task<bool> IsEmailUniqueAsync(string email);
	}

	public class UserRepository : BaseRepository<User>, IUserRepository
	{
		public UserRepository(UserWebApiContext context) 
			: base(context) { }

		public async Task<User?> GetByEmailAsync(string email)
		{
			return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
		}

		public async Task<bool> IsEmailUniqueAsync(string email)
		{
			return await _context.Users.AllAsync(u => u.Email != email);
		}
	}
}
