namespace MyContactsAPI.Models.LoginModels
{
    public class LoginResponse : SharedContext.Response
    {
        protected LoginResponse()
        {
        }

        public LoginResponse(
            string message,
            int status)
        {
            Message = message;
            Status = status;
        }

        public LoginResponse(string message, ResponseData data)
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
