
var WebAppNotification = WebAppNotification || {};
WebAppNotification = (function () {
    startConnection = function (root, userId) {
        var notifyMewithGeneralNotification = function (id, groupId, type, title, msg, creator, isPhoto) {
            try {
                //prepare content
                var notifyMeContent = "";

                if (!isPhoto) {
                    notifyMeContent = msg;
                } else {
                    notifyMeContent = " add an attachment ";
                }

                var icon = "";
                if (creator != '') {
                    icon = root + '/api/Profile/getUserProfilePhoto/' + creator + ' ';
                } else {
                    icon = root + '/images/default-image.svg ';
                }

                if (!Notification) {
                    alert('Thông báo trên màn hình không khả dụng trong trình duyệt của bạn. Hãy thiết lập lại trình duyệt của bạn.');
                    return;
                }

                if (Notification.permission !== "granted")
                    Notification.requestPermission();
                else {
                    var notification = new Notification(title, {
                        icon: icon,
                        body: notifyMeContent,
                    });

                    notification.onclick = function () {
                        window.open(root + "/comments/Comment/" + id + "/" + groupId + "/" + type);
                    };
                }
            } catch (e) { }
        };
        var notifyMe = function (groupName, title, msg, groupId, creator, isPhoto, type) {
            try {
                //prepare content
                var notifyMeContent = "";

                if (!isPhoto) {
                    notifyMeContent = msg;
                } else {
                    notifyMeContent = " gửi một tập tin ";
                }

                var icon = "";
                if (creator != '') {
                    icon = root + '/api/Profile/getUserProfilePhoto/' + creator + ' ';
                } else {
                    icon = root + '/images/default-image.svg ';
                }

                if (!Notification) {
                    alert('Thông báo trên màn hình không khả dụng trong trình duyệt của bạn. Hãy thiết lập lại trình duyệt của bạn.');
                    return;
                }

                if (Notification.permission !== "granted")
                    Notification.requestPermission();
                else {
                    var notification = new Notification(title, {
                        icon: icon,
                        body: notifyMeContent,
                    });

                    notification.onclick = function () {
                        //register_popup(groupId, groupName, root, userId, type);
                    };
                }
            } catch (e) { }
        };

        notificationConnection = new signalR.HubConnectionBuilder()
            .withUrl(root + "/notificationHub")
            .build();

        notificationConnection.on("RefreshChatNotificationNum", function (num, data) {
            //  console.clear();
            $("#main-chat-notification-num").text(num);

            if (data.commentGroupTypeId == "5")// Profile case
            {
                var title = data.creatorName + " đã gửi một tin nhắn đến bạn.";
                notifyMe(data.creatorName, title, data.content, data.groupId, data.creator, data.fileURL != null, 5);
            }
            else if (data.commentGroupTypeId == "4")//Group case
            {
                var title = data.creatorName + " đã gửi một tin nhắn đến nhóm " + data.groupName;

                notifyMe(data.groupName, title, data.content, data.groupId, data.creator, data.fileURL != null, 4);
            }

            window["profile-comments-container-" + data.groupId].appendNewComment(data);
            $("#chat-profilechat-conversation").scrollTop($("#chat-profilechat-conversation")[0].scrollHeight + 200);

            //set current group notification as seen by current user
            $.ajax({
                type: 'put',
                url: root + '/api/Notifications/setNotificationAsRead/' + data.groupId + '/1',
                success: function () {
                    //refresh the notification-num
                    $.ajax({
                        type: 'get',
                        url: root + '/api/Notifications/notificationsNo/1',
                        success: function (num) {
                            $("#main-chat-notification-num").text(num);
                        }
                    });
                }
            });

            if (jQuery.inArray(data.groupId) !== -1) {
                //window["popup-comment-" + data.groupId].appendNewComment(data);


                //$("#popup-comment-" + data.groupId).scrollTop($("#popup-comment-" + data.groupId)[0].scrollHeight);

                window["profile-comments-container-" + data.groupId].appendNewComment(data);
                $("#chat-profilechat-conversation").scrollTop($("#chat-profilechat-conversation")[0].scrollHeight + 200);

                //set current group notification as seen by current user
                $.ajax({
                    type: 'put',
                    url: root + '/api/Notifications/setNotificationAsRead/' + data.groupId + '/1',
                    success: function () {
                        //refresh the notification-num
                        $.ajax({
                            type: 'get',
                            url: root + '/api/Notifications/notificationsNo/1',
                            success: function (num) {
                                $("#main-chat-notification-num").text(num);
                            }
                        });
                    }
                });
            }
        });

        //General notification case

        notificationConnection.on("RefreshGeneralNotificationNum", function (num, data) {
            // consoler.clear();
            $("#main-notification-num").text(num);

            if (data != null) {
                if (data.parent == null) {
                    if (data.commentGroupTypeId == "1")// Profile case
                    {
                        if (window["profile-comments-container-" + data.groupId] !== undefined) {
                            window["profile-comments-container-" + data.groupId].prependNewComment(data);
                        }
                        var title = data.creatorName + " add an post";
                        notifyMewithGeneralNotification(data.id, data.groupId, 1, title, data.content, data.creator, data.fileURL != null);
                    }
                    else if (data.commentGroupTypeId == "2")//Group case
                    {
                        if (window["group-comments-container-" + data.groupId] !== undefined) {
                            window["group-comments-container-" + data.groupId].prependNewComment(data);
                        }

                        var title = data.creatorName + " add an post to group " + data.groupName;

                        notifyMewithGeneralNotification(data.id, data.groupId, 2, title, data.content, data.creator, data.fileURL != null);
                    }
                }
                else {
                    if (data.commentGroupTypeId == "1")// Profile case
                    {
                        if (window["profile-comments-container-" + data.groupId] !== undefined) {
                            window["profile-comments-container-" + data.groupId].prependNewComment(data);
                        }
                        var title = data.creatorName + " add an reply";
                        notifyMewithGeneralNotification(data.id, data.groupId, 1, title, data.content, data.creator, data.fileURL != null);
                    }
                    else if (data.commentGroupTypeId == "2")//Group case
                    {
                        if (window["group-comments-container-" + data.groupId] !== undefined) {
                            window["group-comments-container-" + data.groupId].prependNewComment(data);
                        }
                        var title = data.creatorName + " thêm một tin nhắn ở nhóm " + data.groupName + " bài đăng ";

                        notifyMewithGeneralNotification(data.id, data.groupId, 2, title, data.content, data.creator, data.fileURL != null);
                    }
                }
            }
        });

        notificationConnection.on("RefreshFollowersNotificationNum", function (num) {
            $("#main-follower-notification-num").text(num);
        });

        notificationConnection.start();
    }
    return {
        startConnection: startConnection
    };
})();