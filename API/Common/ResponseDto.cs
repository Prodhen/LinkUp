using System;

namespace API.Common;

public class ResponseDto
{
    internal int StatusCode { get; set; }
    public string? Message { get; set; }
    public object? Data { get; set; }
}
