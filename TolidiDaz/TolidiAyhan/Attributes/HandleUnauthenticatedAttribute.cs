using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TolidiAyhan.Exceptions;

namespace TolidiAyhan.Attributes
{
    public class HandleUnauthenticatedFilter : IAsyncExceptionFilter
    {
        public Task OnExceptionAsync(ExceptionContext context)
        {
            if (context.Exception is UnauthenticatedException)
            {
                context.Result = new RedirectResult("/Login");
                context.ExceptionHandled = true;
            }

            return Task.CompletedTask;
        }
    }
}
