using MyContactsAPI.SharedContext;

namespace MyContactsAPI.Models.PasswordModels
{
    public class UserPasswordResponse : Response
    {
            protected UserPasswordResponse()
            {
            }

            public UserPasswordResponse(
                string message,
                int status)
            {
                Message = message;
                Status = status;
            }

            public UserPasswordResponse(string message, ResponseData data)
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
