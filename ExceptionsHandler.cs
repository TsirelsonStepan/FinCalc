class ExceptionsHandlerMiddleWare
{
	private readonly RequestDelegate _next;

	public ExceptionsHandlerMiddleWare(RequestDelegate next)
	{
		_next = next;
	}

	public async Task InvokeAsync(HttpContext context)
	{
		try
		{
			await _next(context);
		}
		catch (PortfolioSizeIsZero e)
		{
			context.Response.StatusCode = 400;
			await context.Response.WriteAsJsonAsync(new { error = e.Message });
		}
		catch (UnexpectedMoexResponce e)
		{
			context.Response.StatusCode = 400;
			await context.Response.WriteAsJsonAsync(new { error = e.Message });
		}
		catch (Exception e)
		{
			context.Response.StatusCode = 500;
			await context.Response.WriteAsJsonAsync(new { error = e.Message });
		}
	}
}

class PortfolioSizeIsZero(string message) : Exception(message) {}
class UnexpectedMoexResponce(string message) : Exception(message) {}