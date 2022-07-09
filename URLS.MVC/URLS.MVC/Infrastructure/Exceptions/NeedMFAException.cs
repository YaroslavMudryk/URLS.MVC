namespace URLS.MVC.Infrastructure.Exceptions
{
    public class NeedMFAException : Exception
    {
        public string SessionId { get; set; }
        public NeedMFAException(string sessionId)
        {
            SessionId = sessionId;
        }
    }
}