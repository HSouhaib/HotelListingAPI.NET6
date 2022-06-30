﻿using HotelListing.API.Core.CoreExceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net;

namespace HotelListing.API.Core.CoreMiddleware
{
   public class ExceptionMiddleware
   {
      private readonly RequestDelegate _next;
      private readonly ILogger<ExceptionMiddleware> _logger;

      public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
      {
         _next = next;
         _logger = logger;
      }

      public async Task InvokeAsync(HttpContext context)
      {
         try
         {
            await _next(context);
         }
         catch (Exception ex)
         {
            _logger.LogError(ex, $"Something Went Wrong while processing {context.Request.Path}");
            await HandleExceptionAsync(context, ex);
         }
      }

      private Task HandleExceptionAsync(HttpContext context, Exception ex)
      {
         context.Response.ContentType = "application/json";
         HttpStatusCode statusCode = HttpStatusCode.InternalServerError;

         var errorDetails = new ErrorDetails
         {
            ErrorType = "FAILURE",
            ErrorMessage = ex.Message,
         };
         switch (ex)
         {
            case NotFoundException notFoundException:
               statusCode = HttpStatusCode.NotFound;
               errorDetails.ErrorType = "NOT FOUND";
               break;
            default:
               break;
         }

         string response = JsonConvert.SerializeObject(errorDetails);
         context.Response.StatusCode = (int)statusCode;
         return context.Response.WriteAsync(response);
      }

   }

   public class ErrorDetails
   {
      public string ErrorMessage { get; set; }
      public string ErrorType { get; set; }
   }
}
