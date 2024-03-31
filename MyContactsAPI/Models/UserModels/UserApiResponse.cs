using MyContactsAPI.SharedContext;

namespace MyContactsAPI.Models.UserModels;

public class UserApiResponse : Response
{
    protected UserApiResponse()
    {
    }

    public UserApiResponse(
        string message,
        int status)
    {
        Message = message;
        Status = status;
    }

    public UserApiResponse(string message, ResponseData data)
    {
        Message = message;
        Status = 201;
        Data = data;
    }

    public ResponseData? Data { get; set; }
}

public record ResponseData(Guid Id, string Name, string Email);