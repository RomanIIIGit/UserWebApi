using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using UserWebApi.Data;

public class UserWebApiContextFactory : IDesignTimeDbContextFactory<UserWebApiContext>
{
	public UserWebApiContext CreateDbContext(string[] args)
	{
		IConfigurationRoot configuration = new ConfigurationBuilder()
			.SetBasePath(Directory.GetCurrentDirectory())
			.AddJsonFile("appsettings.json")
			.Build();

		string connectionString = configuration.GetConnectionString("DefaultConnection");

		var optionsBuilder = new DbContextOptionsBuilder<UserWebApiContext>();
		optionsBuilder.UseSqlServer(connectionString, b => b.MigrationsAssembly("UserWebApi.Data"));

		return new UserWebApiContext(optionsBuilder.Options);
	}
}
