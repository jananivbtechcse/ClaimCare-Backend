namespace ClaimCare.Responses
{
    public class ErrorResponse
    {
        public int StatusCode {get;set;}
        public string Title {get;set;}
        public string Message {get;set;}
        public DateTime TimeStamp {get;set;} = DateTime.UtcNow;
        
    }
}