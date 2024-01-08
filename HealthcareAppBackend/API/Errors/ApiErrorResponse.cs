namespace API.Errors
{
    public class ApiErrorResponse
    {


        public int StatusCode { get; set; }

        public string Message { get; set; }

        public ApiErrorResponse(int statusCode, string message=null)
        {
            StatusCode = statusCode;
            Message = message ?? GetMessageStatusCode(statusCode);
        }


        private string GetMessageStatusCode(int statusCode)
        {
            return statusCode switch
            {
                400 => "No valid request",
                401 => "You are not authorized for this resource",
                404 => "Not found resource",
                500 => "Internal error of the server",
                _ => null
            };
        }


    }
}
