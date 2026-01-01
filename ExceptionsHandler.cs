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
		catch (PortfolioSizeIsZero)
		{
			context.Response.StatusCode = 400;
			await context.Response.WriteAsJsonAsync(new { error = "The provided sum of amounts of assets is equal to 0." });
			//await context.Response.WriteAsJsonAsync(new { error = e.Message });
		}
		catch (UnexpectedMoexResponce e)
		{
			context.Response.StatusCode = 400;
			await context.Response.WriteAsJsonAsync(new { error = e.Message });
		}
	}
}

class PortfolioSizeIsZero : Exception {}
class UnexpectedMoexResponce(string message) : Exception(message) {}