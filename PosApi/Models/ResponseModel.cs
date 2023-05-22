namespace PosApi.Models
{
    public enum StatusCode
    {
        successReturn = 200,
        created= 200,
        successNoReturn =200,
        error = 500,
        notFound = 404,
    }
    public class ResponseObject<T>
    {
        
        public StatusCode statusCode { get; set; }
        public T data { get; set; } 
    }
    public class ResponseObject
    {

        public StatusCode statusCode { get; set; }
        public object data { get; set; }
    }
    public class Error
    {
        public string message { get; set;}
    }
}
