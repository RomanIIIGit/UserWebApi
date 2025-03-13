using Microsoft.EntityFrameworkCore;
using Serilog;
using UserWebApi.Data;
using UserWebApi.Middlewares;
using UserWebApi.Repositories;
using UserWebApi.Services.Services;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
	.WriteTo.Console()
	.WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
	.CreateLogger();

builder.Host.UseSerilog();

var useInMemory = builder.Configuration.GetValue<bool>("UseInMemoryDatabase");
if (useInMemory)
{
	builder.Services.AddDbContext<UserWebApiContext>(options =>
		options.UseInMemoryDatabase("InMemoryDb"));
}
else
{
	builder.Services.AddDbContext<UserWebApiContext>(options =>
		options.UseSqlServer(
			builder.Configuration.GetConnectionString("DefaultConnection"),
			b => b.MigrationsAssembly("UserWebApi.Data")));
}


builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddControllers();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseMiddleware<RequestResponseLoggingMiddleware>();
app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
	c.SwaggerEndpoint("/swagger/v1/swagger.json", "User Web API V1");
	c.RoutePrefix = string.Empty;
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
