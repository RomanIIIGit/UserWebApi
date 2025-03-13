namespace UserWebApi.Middlewares
{
	public class RequestResponseLoggingMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly ILogger<RequestResponseLoggingMiddleware> _logger;

		public RequestResponseLoggingMiddleware(RequestDelegate next, ILogger<RequestResponseLoggingMiddleware> logger)
		{
			_next = next;
			_logger = logger;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			await LogRequest(context);

			var originalBodyStream = context.Response.Body;
			using var responseBody = new MemoryStream();
			context.Response.Body = responseBody;

			try
			{
				await _next(context);
				await LogResponse(context);
			}
			finally
			{
				await responseBody.CopyToAsync(originalBodyStream);
			}
		}

		private async Task LogRequest(HttpContext context)
		{
			context.Request.EnableBuffering();
			var body = await new StreamReader(context.Request.Body).ReadToEndAsync();
			context.Request.Body.Position = 0;

			_logger.LogInformation($"Incoming Request: {context.Request.Method} {context.Request.Path}");
			_logger.LogInformation($"Request Body: {body}");
		}

		private async Task LogResponse(HttpContext context)
		{
			context.Response.Body.Seek(0, SeekOrigin.Begin);
			var body = await new StreamReader(context.Response.Body).ReadToEndAsync();
			context.Response.Body.Seek(0, SeekOrigin.Begin);

			_logger.LogInformation($"Outgoing Response: {context.Response.StatusCode}");
			_logger.LogInformation($"Response Body: {body}");
		}
	}

}
