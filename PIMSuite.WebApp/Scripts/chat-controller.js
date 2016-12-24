var chat = $.connection.chatHub;

$.connection.hub.start().done(function () {
    chat.server.userConnect($('#currUserId').val());
});

$('#sendMessage').click(function () {
    chat.server.sendMessage($('#userId').val(), $('#messageBody').val());
    $('#messageBody').val('').focus();
});

$('#chat-rooms').on('click', 'li#chat-room', function () {
    var sender = $('#currUserId').val();

    var userIdAttr = $(this).attr('user-id');
    var userNameAttr = $(this).attr('user-lastname');
    var groupIdAttr = $(this).attr('group-id');
    var groupNameAttr = $(this).attr('group-name');
    var isGroup = false;

    $('.new_message_head').removeClass('pull-right');
    if (typeof groupIdAttr !== typeof undefined && groupIdAttr !== false) {
        var html = '<div class="pull-right">'+
                        '<button class="btn btn-success" id="add-user" type="button" onclick="addUserToGroupChat()" style="background-color: #5cb85c">' +
                            '<i class="fa fa-plus" aria-hidden="true"></i>'+
                        '</button>'+
                    '</div>';
        $('.new_message_head').append(html);

        isGroup = true;
    }

    var receiverId = isGroup ? groupIdAttr : userIdAttr;
    var receiverName = isGroup ? groupNameAttr : userNameAttr;

    $('.message_section').show();
    $('#chat-rooms li').removeClass('selectedUser');
    $('#messageColumn li').remove();
    $('#userId').val(receiverId);
    $('#chatWith').text('chat with: ' + receiverName);
    $(this).toggleClass('selectedUser');

    if (!isGroup) {
        chat.server.loadChatHistories(sender, receiverId);
        chat.server.readMessage(sender, receiverId);
    }
});

chat.client.onNewUserConnected = function (userId) {
    updateUserStatus(userId, 'Online');
}

chat.client.onUserDisconnected = function(userId) {
    updateUserStatus(userId, 'Offline');
}

chat.client.loadConnectedUser = function(users) {
    $.each(users, function (i, user) {
        updateUserStatus(user.UserId, 'Online');
    });
}

chat.client.onSendMessageToSender = function (lastName, messageBody, dateTime, senderOrReceiver) {
    var html = getChatTemplate(senderOrReceiver, messageBody, dateTime);
    $('#messageColumn').append(html);
}

chat.client.onSendMessageToReceiver = function (messageId, messageBody, senderUserId, receiverUserId, dateTime, senderOrReceiver) {
    if (senderOrReceiver === 'receiver') {
        var currentChatWithUser = $('#userId').val();
        if (currentChatWithUser !== senderUserId) {
            chat.server.sendNotification(receiverUserId, senderUserId, messageId);
        } else {
            var html = getChatTemplate(senderOrReceiver, messageBody, dateTime);
            $('#messageColumn').append(html);
        }
    }

    chat.server.readMessage(senderUserId, receiverUserId);
}

chat.client.sendNotification = function (totalUnReadMessages, senderUserId) {
    $('#chat-rooms li').each(function (i, value) {
        var userId = $(value).attr('user-id');
        if (userId === senderUserId) {
            $(value).find('.badge').text(totalUnReadMessages);
        }
    });
}

chat.client.readMessage = function (senderUserId) {
    $('#chat-rooms li').each(function (i, value) {
        var userId = $(value).attr('user-id');
        if (userId === senderUserId) {
            $(value).find('.badge').text('');
        }
    });
}

chat.client.loadChatHistories = function (chatHistories) {
    var html = '';

    $.each(chatHistories, function (key, chatHistory) {
        html += getChatTemplate(chatHistory[0], chatHistory[1], chatHistory[2]);
    });

    $('#messageColumn').append(html);
}

function getChatTemplate(senderOrReceiver, messageBody, dateTime) {
    var html = '<li class="left clearfix';
    if (senderOrReceiver === 'sender') {
        html += ' admin_chat"><span class="chat-img1 pull-right"><img src="/Content/images/no_propic.png" alt="User Avatar" class="img-circle"> </span>';
        html += '<div class="chat-body1 clearfix sender">';
    } else {
        html += '"><span class="chat-img1 pull-left"><img src="/Content/images/no_propic.png" alt="User Avatar" class="img-circle"> </span>';
        html += '<div class="chat-body1 clearfix receiver">';
    }

    html += '<p>' + messageBody + '</p>';
//    html += '<div class="chat_time pull-left">' + dateTime + '</div>';
    html += '</div></li>';
    return html;
}

function updateUserStatus(userId, onlineOrOffline) {
    $('div').find('#statusUserId').each(function (i, statusUserId) {
        if ($(statusUserId).val() === userId) {
            var userStatus = $(statusUserId).parent();

            if (onlineOrOffline === 'Online') {
                $(userStatus).removeClass('btn-danger');
                $(userStatus).addClass('btn-success');
            } else {
                $(userStatus).removeClass('btn-success');
                $(userStatus).addClass('btn-danger');
            }
            $(userStatus).find('#onlineOffline').text(onlineOrOffline);
        }
    });
}