using System.Text.RegularExpressions;

namespace WebNovel.Utils
{
    public static class TextUtils
    {
        /// <summary>
        /// Đếm số từ trong chuỗi (tính cả tiếng Việt, tiếng Anh)
        /// </summary>
        public static int CountWords(string content)
        {
            if (string.IsNullOrWhiteSpace(content))
                return 0;

            var matches = Regex.Matches(
                content,
                @"\b[\p{L}\p{N}]+(?:[-'][\p{L}\p{N}]+)?\b",
                RegexOptions.Multiline
            );

            return matches.Count;
        }

        public static string GetDisplayTime(DateTime? publicAt, DateTime createAt)
        {
            DateTime baseTime = publicAt ?? createAt;
            TimeSpan diff = DateTime.UtcNow - baseTime;

            if (diff.TotalMinutes < 1)
                return "Vừa đăng";
            if (diff.TotalMinutes < 60)
                return $"{(int)diff.TotalMinutes} phút trước";
            if (diff.TotalHours < 24)
                return $"{(int)diff.TotalHours} giờ trước";
            if (diff.TotalDays < 30)
                return $"{(int)diff.TotalDays} ngày trước";
            if (diff.TotalDays < 365)
                return $"{(int)(diff.TotalDays / 30)} tháng trước";

            // Nếu quá 12 tháng thì trả về định dạng ngày cụ thể
            return baseTime.ToLocalTime().ToString("dd/MM/yyyy");
        }
    }
}
