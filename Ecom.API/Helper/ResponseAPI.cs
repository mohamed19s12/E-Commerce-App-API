namespace Ecom.API.Helper
{
    public class ResponseAPI
    {
        public ResponseAPI(int statusCode, string message=null)
        {
            StatusCode = statusCode;
            Message = message ?? GetMessageFromStatusCode(statusCode);
        }

        private string GetMessageFromStatusCode(int statusCode) => statusCode switch
        {
            200 => "OK",
            201 => "Created",   
            400 => "Bad Request",
            401 => "Unauthorized",
            403 => "Forbidden",
            404 => "Not Found",
            500 => "Internal Server Error",
            _ => "Unknown Status"
        };

        public int StatusCode { get; set; }
        public string Message { get; set; }
    }
}
