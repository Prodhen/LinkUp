using System;
using API.Common;

namespace API.Helper;

public static class Utilities
{

    public static ResponseDto SuccessResponseForAdd(object? data = null)
    {
        return new ResponseDto
        {
            StatusCode = StatusCodes.Status201Created,
            Message = "SavedSuccessfully",
            Data = data
        };
    }

    public static ResponseDto SuccessResponseForUpdate(object? data = null)
    {
        return new ResponseDto
        {
            StatusCode = StatusCodes.Status200OK,
            Message = "UpdatedSuccessfully",
            Data = data
        };
    }

    public static ResponseDto SuccessResponseForDelete()
    {
        return new ResponseDto
        {
            StatusCode = StatusCodes.Status200OK,
            Message = "DeletedSuccessfully",
            Data = null
        };
    }

    public static ResponseDto SuccessResponseForGet(object? data = null)
    {
        return new ResponseDto
        {
            StatusCode = StatusCodes.Status200OK,
            Message = null,
            Data = data
        };
    }

    public static ResponseDto NoContentResponse(object? data = null)
    {
        return new ResponseDto
        {
            StatusCode = StatusCodes.Status204NoContent,
            Message = "NoContent",
            Data = null
        };
    }


    public static ResponseDto ValidationErrorResponse(string? message = null)
    {
        return new ResponseDto
        {
            StatusCode = StatusCodes.Status400BadRequest,
            Message = message ?? "ValidationError",
            Data = null
        };
    }
    public static ResponseDto ValidationErrorResponse(List<string>? messageList)
    {
        return new ResponseDto
        {
            StatusCode = StatusCodes.Status400BadRequest,
            Message = "Validation Error",
            Data = messageList
        };
    }


    public static ResponseDto UnAuthorizedResponse()
    {
        return new ResponseDto
        {
            StatusCode = StatusCodes.Status401Unauthorized,
            Message = "UnAuthorized",
            Data = null
        };
    }


    public static ResponseDto ForbiddenResponse()
    {
        return new ResponseDto
        {
            StatusCode = StatusCodes.Status403Forbidden,
            Message = "ForbiddenResponse",
            Data = null
        };
    }




}
