namespace MyContactsAPI.Models
{
    public class Response : MyContactsAPI.SharedContext.Response
    {
        protected Response()
        {
        }

        public Response(
            string message,
            int status)
        {
            Message = message;
            Status = status;
        }

        public Response(string message, ResponseData data)
        {
            Message = message;
            Status = 201;
            Data = data;
        }

        public ResponseData? Data { get; set; }
    }

    public record ResponseData(Guid Id, string Name, string Email);
}
