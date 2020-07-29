﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrudDemo
{
    public interface IExceptionHandler
    {
        public Task HandleAsync(HttpContext context, Exception exception);
    }
}
