namespace API.Errors
{
    public class ApiException: ApiErrorResponse
    {

        public int StatusCode { get; set; }
        public string Message { get; set; }
        public string Detail { get; set; }

        public ApiException(int statusCode, string message=null, string detail= null): base(statusCode, message) 
        {
         
            Detail = detail;
        }
    }
}
