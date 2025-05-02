namespace WebNovel.Utils
{
    public class SystemVariable
    {
        public static readonly string htmlSearchToolbar = @"<div class='k-toolbar-item' style='padding-left:10px;padding-right: 3px;'>
    <span class='k-searchbox k-input k-input-md k-rounded-md k-input-solid k-grid-search'>
        <span class='k-svg-icon k-svg-i-search k-input-icon'>
            <svg viewBox='0 0 512 512' focusable='false' xmlns='http://www.w3.org/2000/svg'>
                <path d='M365.3 320h-22.7l-26.7-26.7C338.5 265.7 352 230.4 352 192c0-88.4-71.6-160-160-160S32 103.6 32 192s71.6 160 160 160c38.4 0 73.7-13.5 101.3-36.1l26.7 26.7v22.7L434.7 480l45.3-45.3L365.3 320zM64 192c0-70.7 57.3-128 128-128s128 57.3 128 128-57.3 128-128 128S64 262.7 64 192z'></path>
            </svg>
        </span>
        <input placeholder='Tìm kiếm nhanh...' title='Tìm kiếm nhanh...' aria-label='Tìm kiếm nhanh...' class='k-input-inner' />
    </span>
</div>";
        public static readonly string placeholderRequired = "Dữ liệu bắt buộc";
        public static readonly string placeholderTextbox = "Nhập chữ...";
        public static readonly string placeholderSelect = "Nhập chữ để tìm kiếm...";
        public static readonly string placeholderNumber = "Nhập số...";
        public static readonly string placeholderDateTime = "Nhập ngày/tháng/năm giờ:phút...";
        public static readonly string placeholderDate = "Nhập ngày/tháng/năm...";
        public static readonly string placeholderDay = "Chọn ngày...";
        public static readonly string placeholderMonth = "Chọn tháng...";
        public static readonly string placeholderYear = "Nhập năm...";
        public static readonly int minYear = 1900;
        public static readonly int maxYear = 2999;

        public static readonly string formatDate = "{0:dd/MM/yyyy}";
        public static readonly string formatDateTime = "{0:dd/MM/yyyy HH:mm}";
        public static readonly string formatTimeCtr = "HH:mm";
        public static readonly string formatTime = "{0:dd/MM/yyyy HH:mm}";
        public static readonly string formatNumber2 = "{0:n2}";
        public static readonly string formatNumber0 = "{0:n0}";
        public static readonly string formatNumberYear = "####";


        public static readonly string errorNotFoundItem = "Đối tượng không tồn tại hoặc đã bị xóa.";


        //Ví dụ: Cấu trúc Group item:
        // Nhóm Item 1: Combobox||Multiselect
        // Nhóm Item 2: Combobox||Multiselect
        //.........
        // ==> Giá trị lưu vào hệ thống: 1||1__2__3__4&&2||5__6__7__8
        public static readonly string splitGroup = "&&"; //Phân cách giá trị mỗi nhóm item
        public static readonly string splitItem = "||"; //Phân cách giá trị mỗi item trong 1 nhóm
        public static readonly string splitChildItem = "__"; //Phân cách giá trị dạng multiselect

        public static readonly string[] validationUpdateFileBidding = new string[] {
             ".doc", ".docx", ".pdf"
        };
        public static readonly string[] validationUpdateImage = new string[] {
             ".jpg", ".jepg", ".png", ".webp"
        };
        public static readonly string[] validationUpdateDefault = new string[] {
            ".jpg", ".jepg", ".png",
            ".doc", ".docx", ".xls", ".xlsx", ".ppt", ".pptx",
            ".pdf", ".txt",
            ".zip", ".rar", ".7z"
        };
        public static readonly string[] validationUpdateImport = new string[] {
            ".xls", ".xlsx"
        };
    }

}
