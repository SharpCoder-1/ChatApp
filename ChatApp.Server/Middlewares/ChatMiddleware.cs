
namespace ChatApp.Server.Middlewares
{
    public class ChatMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (context.Request.Path.StartsWithSegments("/hubs/chat"))
            {   
                context.Request.Headers.Add("Authorization", context.Request.Query
                    .FirstOrDefault(p=>p.Key=="access_token").Value);
            }
            await next(context);
        }
    }
}
