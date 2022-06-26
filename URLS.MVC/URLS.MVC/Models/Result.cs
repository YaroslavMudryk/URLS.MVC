namespace URLS.MVC.Models
{
    public class Result<T>
    {
        public bool Ok { get; set; }
        public string Message { get; set; }
        public string Description { get; set; }
        public T Data { get; set; }
        public Meta Meta { get; set; }

        public bool IsSuccess() => Ok;
        public string GetError() => Message;
    }
}
