namespace WebNovel.ViewModels
{
    public class ResponseViewModel<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public List<T> Data { get; set; }
        public string Code { get; set; } = string.Empty;
    }
}
