using System.Net;

namespace Infrastructure.Responses;

public class ApiResponse<T>
{
    public int StatusCode { get; set; }
    public T? Data { get; set; }
    public string? Message { get; set; }

    public ApiResponse(T? data)
    {
        StatusCode = 200;
        Data = data;
        Message = "Success";
    }

    public ApiResponse(HttpStatusCode statusCode, string message)
    {
        StatusCode = (int)statusCode;
        Message = message;
    }
}