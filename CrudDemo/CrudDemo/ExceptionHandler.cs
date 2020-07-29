using CrudDemo.Exceptions;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace CrudDemo
{
    public class ExceptionHandler : IExceptionHandler
    {

        public ExceptionHandler()
        {
        }

        public async Task HandleAsync(HttpContext context, Exception exception)
        {
            // Unauthorized
            if (exception.GetType() == typeof(AuthorizationException))
            {
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
            }

            // NotFound
            if (exception.GetType() == typeof(NonExistingItemException))
            {
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            }

            // Conflict
            if (exception.GetType() == typeof(DuplicateRecordException))
            {
                context.Response.StatusCode = (int)HttpStatusCode.Conflict;
            }

            var result = JsonConvert.SerializeObject(new { error = exception.Message });
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(result);
        }
    }
}
