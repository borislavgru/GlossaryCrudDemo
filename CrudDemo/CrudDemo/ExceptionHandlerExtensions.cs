using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrudDemo
{
   public static class ExceptionHandlerExtensions
    {
        public static IApplicationBuilder UseProblemDetailsExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(configure =>
            {
                configure.Run(async context =>
                {
                    try
                    {
                        var exceptionHandler = app.ApplicationServices.GetRequiredService<IExceptionHandler>();
                        var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();
                        var exception = exceptionHandlerFeature.Error;
                        await exceptionHandler.HandleAsync(context, exception);
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                });
            });

            return app;
        }
    }
}
