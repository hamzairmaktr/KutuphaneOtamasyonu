namespace IKitaplik.BlazorUI.Responses
{
    public class Response<T> where T : class, new()
    {
        public Response()
        {
            Success = false;
            Message = string.Empty;
            Data = new T();
        }

        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
    }

    public class Response
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
