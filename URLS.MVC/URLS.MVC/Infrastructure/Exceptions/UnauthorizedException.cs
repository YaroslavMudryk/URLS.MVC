namespace URLS.MVC.Infrastructure.Exceptions
{
    public class UnauthorizedException : Exception
    {
        public UnauthorizedException(string error) : base(error) { }
    }
}
