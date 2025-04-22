var currentEditedRow = null;

function onEditGridRow(e) {
    currentEditedRow = e.container.closest("tr");
}

function onPICChange(e) {
    var selectedValue = this.value(); // ValueId của PIC mới
    var selectedText = this.text(); // Tên PIC mới (Text)
    if (!currentEditedRow) {
        return;
    }
    // Lấy item của grid đang chọn để cập nhật thông tin
    var grid = $("#grid_TaskInfo_Index").data("kendoGrid");
    var dataItem = grid.dataItem(currentEditedRow);
    dataItem.set("PICQC", selectedValue);
    dataItem.set("PICQCName", selectedText);

    if (!dataItem) {
        alert("Please select a row in the grid first.");
        return;
    }

    // Gọi API PUT để cập nhật thông tin PIC
    //$.ajax({
    //    url: 'api/TaskInfo/UpdateQCPIC',
    //    type: 'PUT',
    //    contentType: 'application/json',
    //    data: JSON.stringify({
    //        TaskId: dataItem.TaskId, // Truyền ID hoặc thông tin cần thiết từ item
    //        QCPIC: selectedValue
    //    }),
    //    success: function (response) {
    //        // Nếu thành công, cập nhật item trong grid
    //        dataItem.set("QCPIC", selectedValue);
    //        dataItem.set("QCPICName", selectedText);

    //        // Refresh lại lưới nếu cần thiết
    //        grid.refresh();
    //    },
    //    error: function (xhr, status, error) {
    //        console.error("Error while updating PIC:", error);
    //        alert("Có lỗi xảy ra khi cập nhật PIC.");
    //    }
    //});
}

function onPICClose(e) {
    if (!currentEditedRow) {
        return;
    }
    var grid = $("#grid_TaskInfo_Index").data("kendoGrid");
    var dataItem = grid.dataItem(currentEditedRow);

    // Xóa lớp k-dirty-cell và các phần tử không cần thiết
    $(currentEditedRow).find("td").removeClass("k-dirty-cell");
    $(currentEditedRow).find("span.k-dirty").remove();
}

function onPICFiltering(e) {
    var filterValue = e.filter != null ? e.filter.value : "";
    e.preventDefault();
    // Thực hiện filter custom trên client
    e.sender.dataSource.filter({
        logic: "or", // Lọc theo nhiều field
        filters: [
            { field: "Text", operator: "contains", value: filterValue },
            { field: "Email", operator: "contains", value: filterValue },
            { field: "Department", operator: "contains", value: filterValue }
        ]
    });
    e.sender.refresh();
}
