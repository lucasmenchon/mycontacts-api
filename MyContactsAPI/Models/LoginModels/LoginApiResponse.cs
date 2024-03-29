using MyContactsAPI.SharedContext;

namespace MyContactsAPI.Models.LoginModels
{
    public class LoginApiResponse : Response
    {
        protected LoginApiResponse()
        {
        }

        public LoginApiResponse(
            string message,
            int status)
        {
            Message = message;
            Status = status;
        }

        public LoginApiResponse(string message, ResponseData data)
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
}
