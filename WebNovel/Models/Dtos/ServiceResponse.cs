namespace WebNovel.Models.Dtos
{
    public class ServiceResponse<T>
    {
        public bool Success { get; set; } = true;
        public string Message { get; set; } = "Success";
        public T? Data { get; set; }

        public static ServiceResponse<T> Ok(T data, string? message = null) =>
            new() { Data = data, Message = message ?? "Success" };

        public static ServiceResponse<T> Fail(string message) =>
            new() { Success = false, Message = message };
    }

}
