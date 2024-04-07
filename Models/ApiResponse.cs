using MyContactsAPI.SharedContext;

namespace MyContactsAPI.Models;

public class ApiResponse : Response
{
    protected ApiResponse()
    {
    }

    public ApiResponse(
        string message,
        int status)
    {
        Message = message;
        Status = status;
    }

    public ApiResponse(string message, ResponseData data)
    {
        Message = message;
        Status = 201;
        Data = data;
    }

    public ResponseData? Data { get; set; }
}

public class ResponseData
{
    public string Token { get; set; } = string.Empty;
}