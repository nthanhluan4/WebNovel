(function () {

    /*********************************************************************************************************
    * Define Variables
    *********************************************************************************************************/
    window.createNotify = {};
    var createNotify = window.createNotify;
    createNotify['messageDefault'] = {};
    //hrmUtil.functionEvent.getTranslate(["Hrm_Notification", "Success", "Hrm_Fail", 'Hrm_Change_Status_Succeed', 'HRM_Common_NotEmtpy', 'HRM_System_Resource_Sys_GroupLabel', 'HRM_Rec_JobCondition_SelectDiseases', 'HRM_Enter_Label_Find', 'HRM_Colspan_Filter', 'HRM_Advance_Filter'], function (data) {
    //    createNotify['messageDefault'] = data;
    //});
    createNotify['messageDefault'] = {
        "Hrm_Notification": "Thông báo",
        "Success": "Thao tác thành công",
        "Hrm_Fail": "Thao tác thất bại",
        "Hrm_Change_Status_Succeed": "Thay đổi trạng thái thành công!",
        "HRM_Common_NotEmtpy": " không thể bỏ trống.",
        "HRM_System_Resource_Sys_GroupLabel": "Đánh nhãn",
        "HRM_Rec_JobCondition_SelectDiseases": "Vui lòng chọn...",
        "HRM_Enter_Label_Find": "Nhập tên nhãn cần tìm...",
        "HRM_Colspan_Filter": "Thu gọn vùng tìm kiếm",
        "HRM_Advance_Filter": "Hiển thị tìm kiếm nâng cao"
    };

    createNotify['stack-topleft'] = { "dir1": "down", "dir2": "right", "push": "top" };
    createNotify['stack-bottomleft'] = { "dir1": "right", "dir2": "up", "push": "top" };
    createNotify['stack-custom'] = { "dir1": "right", "dir2": "down" };
    createNotify['stack-custom2'] = { "dir1": "left", "dir2": "up", "push": "top" };
    createNotify['stack-modal'] = { "dir1": "down", "dir2": "right", "push": "top", "modal": true, "overlay_close": false };
    createNotify['stack-bar_top'] = { "dir1": "down", "dir2": "right", "push": "top", "spacing1": 20, "spacing2": 20 };
    createNotify['stack-bar_bottom'] = { "dir1": "up", "dir2": "right", "spacing1": 0, "spacing2": 0 };
    createNotify['stack-bottomright'] = { "dir1": "up", "dir2": "left", "firstpos1": 25, "firstpos2": 25 };

    var defaultOptions = {
        title: 'Title is not define',
        text: 'Message is not define',
        stack: 'bar_top'
    };
    var enumNoticeStatus = {
        success: 'success',
        info: 'info',
        error: 'error',
        notice: 'notice'
    };

    /*********************************************************************************************************
    * Define Functions
    *********************************************************************************************************/
    /* Execute Notice
    */
    function executeNotice(options) {
        defaultOptions["title"] = createNotify['messageDefault']["Hrm_Notification"];
        var opts = $.extend(true, {}, defaultOptions, options);
        if (opts['stack']) {
            opts.addclass = ('stack-' + opts['stack']) || 'stack-bottomright';
            opts.stack = (createNotify['stack-' + opts['stack']]) || createNotify['stack-bottomright'];
        }

        return new PNotify(opts);
    }
    /* Execute Notice callBack
   */
    function executeNoticecallBack(options) {
        defaultOptions["title"] = createNotify['messageDefault']["Hrm_Notification"];
        var opts = $.extend(true, {}, defaultOptions, options);
        if (opts['stack']) {
            opts.addclass = ('stack-' + opts['stack']) || 'stack-bottomright';
            opts.stack = (createNotify['stack-' + opts['stack']]) || createNotify['stack-bottomright'];
        }
        return (new PNotify(opts)).get()
            .on('pnotify.confirm', function () {
                if (opts.yes && typeof opts.yes == "function") opts.yes();
            })
            .on('pnotify.cancel', function () {
                if (opts.no && typeof opts.no == "function") opts.no();
            });
    }

    /*********************************************************************************************************
    * Define Alert Type
    *********************************************************************************************************/
    var defaultAlertOptions = {
        title: createNotify.messageDefault["Hrm_Notification"],
        type: enumNoticeStatus.notice
    };

    function alert(userOptions) {
        var options = $.extend(true, {}, defaultAlertOptions, userOptions);
        return executeNotice(options);
    }

    /* Alert Info
    */
    alert.info = function (userOptions) {
        return alert($.extend(true, {}, { type: enumNoticeStatus.info, text: createNotify.messageDefault["Hrm_Change_Status_Succeed"] }, userOptions));
    }

    /* Alert Success
    */
    alert.success = function (userOptions) {
        return alert($.extend(true, {}, { type: enumNoticeStatus.success, text: createNotify.messageDefault["Success"] }, userOptions));
    }
    /* Alert Error
    */
    alert.error = function (userOptions) {
        return alert($.extend(true, {}, { type: enumNoticeStatus.error, text: createNotify.messageDefault["Hrm_Fail"] }, userOptions));
    }
    /*********************************************************************************************************
    * Define Sticky Type
    *********************************************************************************************************/
    var defaultStickyOptions = {
        title: createNotify.messageDefault["Hrm_Notification"],
        hide: false,
        type: enumNoticeStatus.notice,
        stack: 'bottomright'
    };

    /* sticky 
    */
    function sticky(userOptions) {
        var options = $.extend(true, {}, defaultOptions, defaultStickyOptions, userOptions);
        return executeNotice(options);
    }

    /* Sticky Info
    */
    sticky.info = function (userOptions) {
        var options = $.extend(true, {}, defaultOptions, { type: enumNoticeStatus.info, text: createNotify.messageDefault["Hrm_Change_Status_Succeed"] }, userOptions);
        return sticky(options);
    }

    /* Sticky Success
    */
    sticky.success = function (userOptions) {
        var options = $.extend(true, {}, defaultOptions, { type: enumNoticeStatus.success, text: createNotify.messageDefault["Success"] }, userOptions);
        return sticky(options);
    }

    /* Sticky error
    */
    sticky.error = function (userOptions) {
        var options = $.extend(true, {}, defaultOptions, { type: enumNoticeStatus.error, text: createNotify.messageDefault["Hrm_Fail"] }, userOptions);
        return sticky(options);
    }

    /*********************************************************************************************************
    * Define confirm
    *********************************************************************************************************/
    var defaultConfirmOptions = {
        type: enumNoticeStatus.notice,
        title: createNotify.messageDefault["Hrm_Notification"],
        hide: false,
        //stack: 'modal',
        confirm: {
            confirm: true
        },
        buttons: {
            closer: false,
            sticker: false
        },
        history: {
            history: false
        }
    };

    /* confirm
    */
    function confirm(userOptions, callBack) {
        var options = $.extend(true, {}, defaultConfirmOptions, userOptions);
        return executeNoticecallBack(options, callBack);
    }

    /* confirm Info
    */
    confirm.info = function (userOptions) {
        var options = $.extend(true, {}, defaultConfirmOptions, { type: enumNoticeStatus.info }, userOptions);
        return executeNoticecallBack(options);
    }

    /* confirm Success
    */
    confirm.success = function (userOptions) {
        var options = $.extend(true, {}, defaultConfirmOptions, { type: enumNoticeStatus.success, text: createNotify.messageDefault["Success"] }, userOptions);
        return executeNoticecallBack(options);
    }

    /* confirm error
    */
    confirm.error = function (userOptions) {
        var options = $.extend(true, {}, defaultConfirmOptions, { type: enumNoticeStatus.error, text: createNotify.messageDefault["Hrm_Fail"] }, userOptions);
        return executeNoticecallBack(options);
    }
    /*********************************************************************************************************
    * Define tooltip
    *********************************************************************************************************/
    var defaultTooltipOptions = {
        type: enumNoticeStatus.notice,
        title: createNotify.messageDefault["Hrm_Notification"],
        hide: false,
        confirm: {
            confirm: false
        },
        buttons: {
            closer: false,
            sticker: false
        },
        history: {
            history: false
        },
        animate_speed: "fast",
        icon: "fa fa-info-circle",
        stack: false,
        auto_display: false
    };

    function setTooltipToElement(selector, tooltipSet) {
        var name = selector;
        selector = $(selector);
        selector.on('mouseover', function (evt) {
            tooltipSet.open();
            //Reset when open Show/Hidden
            tooltipSet.get().css({ 'top': evt.clientY + 12, 'left': evt.clientX + 12 });
        });

        selector.on('mousemove', function (evt) {
            tooltipSet.get().css({ 'top': evt.clientY + 12, 'left': evt.clientX + 12 });
        });

        selector.on('mouseout', function () {
            tooltipSet.remove();
        });
    }

    function tooltip(userOptions) {
        var options = $.extend(true, {}, defaultTooltipOptions, userOptions);
        var tooltipSet = executeNotice(options);
        setTooltipToElement(options.selector, tooltipSet);
        return tooltipSet;
    }

    /* Display All Notices
    */
    function showAllNotices() {
        $("body").trigger('pnotify.history-all');
    }

    /* Display Last Notice
    */
    function showLastNotices() {
        $("body").trigger('pnotify.history-last');
    }
    /* Remove All Notice
    */
    function removeAll() {
        PNotify.removeAll();
    }
    /*********************************************************************************************************
    * Define notice
    *********************************************************************************************************/
    createNotify['status'] = enumNoticeStatus;
    createNotify['alert'] = alert;
    createNotify['sticky'] = sticky;
    createNotify['confirm'] = confirm;
    createNotify['tooltip'] = tooltip
    createNotify['showAllNotices'] = showAllNotices;
    createNotify['showLastNotices'] = showLastNotices;
    createNotify['removeAll'] = removeAll;
})();
