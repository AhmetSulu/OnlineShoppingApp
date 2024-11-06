namespace OnlineShoppingApp.Business.Types
{
    public class ServiceMessage
    {
        public string Message { get; set; }
        public bool IsSuccess { get; set; }
    }

    public class ServiceMessage<T>
    {
        public string Message { get; set; }
        public bool IsSuccess { get; set; }
        public T? Data { get; set; }
    }
}
