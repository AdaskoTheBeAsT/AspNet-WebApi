using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;

namespace ReferenceProject
{
    public static class CacheControlMiddleware
    {
        public static Task MiddlewareAsync(IOwinContext context, Func<Task> next)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (next is null)
            {
                throw new ArgumentNullException(nameof(next));
            }

            if (context.Request.Method == "GET")
            {
                context.Response.Headers.Append("Cache-Control", "no-cache, no-store, must-revalidate");
            }

            return next();
        }

        public static void PreventResponseCaching(this IAppBuilder app)
        {
            app.Use((context, func) => MiddlewareAsync(context, func));
        }
    }
}