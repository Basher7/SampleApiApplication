using Domain.Abstraction;
using Domain.ResponseModels;
using Microsoft.IO;
using System.Net;
using System.Text;

namespace SampleApiApplication.Middlewares;

//public class GlobalExceptionMiddleware(RequestDelegate next)
//{
//    private readonly RequestDelegate _next = next;

//    public async Task InvokeAsync(HttpContext context)
//    {
//        string _method = context.Request.Path.Value!;

//        var originalBodyStream = context.Response.Body;

//        RecyclableMemoryStreamManager _recyclableMemoryStreamManager = new();
//        await using var responseBody = _recyclableMemoryStreamManager.GetStream();
//        context.Response.Body = responseBody;

//        try
//        {
//            await _next(context);
//            await responseBody.CopyToAsync(originalBodyStream);
//        }
//		catch (Exception ex)
//        {
//            string errMsg = ex.ExceptionMessage();
//            string errorResponse = GetErrorResponse(_method);
//            context.Response.StatusCode = (int)HttpStatusCode.OK;
//            await new MemoryStream(Encoding.UTF8.GetBytes(errorResponse)).CopyToAsync(originalBodyStream);
//		}
//    }

//    #region Private Methods

//    private static string GetErrorResponse(string methodName)
//    {
//        return methodName switch
//        {
//            _ => new GlobalApiResponse(false, MessageCollections.SomethingWentWrong).ToJsonString(),
//        };
//    }

//    #endregion
//}
