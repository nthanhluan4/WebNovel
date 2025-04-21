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
                @"\\b[\\p{L}\\p{N}]+(?:[-'][\\p{L}\\p{N}]+)?\\b",
                RegexOptions.Multiline
            );

            return matches.Count;
        }
    }
}
