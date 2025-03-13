using Microsoft.EntityFrameworkCore;
using UserWebApi.Data.Models;

namespace UserWebApi.Data
{
    public class UserWebApiContext : DbContext
	{
		public UserWebApiContext(DbContextOptions<UserWebApiContext> options) 
			: base(options) { }

		public DbSet<User> Users { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<User>()
				.Property(u => u.Email)
				.IsRequired();

			modelBuilder.Entity<User>()
				.HasIndex(u => u.Email)
				.IsUnique();
		}
	}
}
