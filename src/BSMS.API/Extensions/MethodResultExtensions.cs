using BSMS.Application.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace BSMS.API.Extensions;

internal static class MethodResultExtensions
{
    public static ObjectResult DecideWhatToReturn<T>(this MethodResult<T> result)
    {
        return result.IsError 
            ? SetErrorResultToReturn(result)
            : new OkObjectResult(result.Data);
    }
    
    private static ObjectResult SetErrorResultToReturn<T>(MethodResult<T> result)
    {
        return new ObjectResult(result.Error) { StatusCode = (int)result.ErrorStatusCode };
    }
}