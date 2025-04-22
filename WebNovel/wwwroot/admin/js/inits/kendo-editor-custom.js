$("#editor").kendoEditor({
    tools: [
        "bold", "italic", "underline", "strikethrough", "justifyLeft", "justifyCenter",
        "justifyRight", "justifyFull", "insertUnorderedList", "insertOrderedList",
        "indent", "outdent", "createLink", "unlink", "insertImage", "viewHtml"
    ],
    image: {
        saveAsBase64: false,  // Không lưu chuỗi base64
        saveUrl: "/api/uploadImage" // API xử lý upload hình ảnh
    },
    paste: function (e) {
        // Kiểm tra nếu người dùng dán hình ảnh
        var images = e.clipboardData.items;
        for (var i = 0; i < images.length; i++) {
            if (images[i].kind === 'file' && images[i].type.startsWith('image/')) {
                var file = images[i].getAsFile();
                uploadImage(file);
            }
        }
    }
});

function uploadImage(file) {
    var formData = new FormData();
    formData.append("file", file);

    $.ajax({
        url: "/api/uploadImage", // API xử lý upload hình ảnh
        type: "POST",
        data: formData,
        contentType: false,
        processData: false,
        success: function (response) {
            // Chèn hình ảnh với URL trả về
            var editor = $("#editor").data("kendoEditor");
            editor.exec("inserthtml", { value: '<img src="' + response.location + '" />' });
        },
        error: function () {
            alert("Image upload failed");
        }
    });
}