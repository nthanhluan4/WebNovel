namespace WebNovel.Utils
{
    public static class GetDataHelper
    {
        public static string ConvertIdsToNames(string? ids, Dictionary<int, string> lookupDict)
        {
            if (string.IsNullOrWhiteSpace(ids))
                return string.Empty;

            var idList = ids.Split(',')
                            .Select(id => int.TryParse(id, out var parsedId) ? parsedId : 0)
                            .Where(id => id != 0 && lookupDict.ContainsKey(id))
                            .Select(id => lookupDict[id])
                            .ToList();

            return string.Join(", ", idList);
        }
        public static string ConvertStoryStatusToNames(string value)
        {
            if (string.IsNullOrEmpty(value)) return string.Empty;
            switch (value)
            {
                default:
                    return "";
                case "1":
                    return "Đang ra";
                case "2":
                    return "Hoàn thành";
                case "3":
                    return "Tạm ngưng";
            }
        }
    }
}
