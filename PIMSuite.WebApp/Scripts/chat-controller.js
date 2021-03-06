﻿var chat = $.connection.chatHub;

$.connection.hub.start().done(function () {
    chat.server.userConnect($('#currUserId').val());
});

$('#messageBody').keypress(function (e) {
    if ($('#messageBody').val() !== "" && e.which === 13) {
        e.preventDefault();
        $('#sendMessage').click();
    }
});

$('#sendMessage').click(function () {
    if ($(this).attr('is-group') === "true") {
        chat.server.sendGroupMessage($('#userId').val(), $('#messageBody').val());
    } else {
        chat.server.sendMessage($('#userId').val(), $('#messageBody').val());
    }

    $('#messageBody').val('');
    $('#messageBody')[0].focus();
});

$('#chat-rooms').on('click', 'li#chat-room', function () {
    var sender = $('#currUserId').val();

    var userIdAttr = $(this).attr('user-id');
    var userNameAttr = $(this).attr('user-lastname');
    var groupIdAttr = $(this).attr('group-id');
    var groupNameAttr = $(this).attr('group-name');
    var isGroup = false;

    $('#adduserbtn').remove();
    if (typeof groupIdAttr !== typeof undefined && groupIdAttr !== false) {
        var html = '<div id="adduserbtn" class="pull-right">' +
                        '<button class="btn btn-success" id="view-user" type="button" onclick="viewUserInGroupChat()" style="background-color: blue; margin-right: 10px">' +
                            '<i class="fa fa-users" aria-hidden="true"></i>' +
                        '</button>' +
                        '<button class="btn btn-success" id="add-user" type="button" onclick="addUserToGroupChat()" style="background-color: #5cb85c">' +
                            '<i class="fa fa-plus" aria-hidden="true"></i>'+
                        '</button>' +
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
    $('#chatWith').text('Chat mit: ' + receiverName);
    $(this).toggleClass('selectedUser');
    $('#sendMessage').attr('is-group', isGroup);

    chat.server.loadChatHistories(sender, receiverId, isGroup);
    chat.server.readMessage(sender, receiverId);
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
    var html = getChatTemplate(senderOrReceiver, messageBody, dateTime, "", false);
    $('#messageColumn').append(html);
}

chat.client.onSendMessageToReceiver = function (messageId, messageBody, senderUserId, receiverUserId, dateTime, senderOrReceiver) {
    if (senderOrReceiver === 'receiver') {
        var currentChatWithUser = $('#userId').val();
        if (currentChatWithUser !== senderUserId) {
            chat.server.sendNotification(receiverUserId, senderUserId, messageId);
        } else {
            var html = getChatTemplate(senderOrReceiver, messageBody, dateTime, "", false);
            $('#messageColumn').append(html);
        }
    }

    chat.server.readMessage(senderUserId, receiverUserId);
}

chat.client.onSendMessageToGroup = function (messageBody, senderUserId, dateTime, senderLastName, groupId) {
    var tagetChatId = $('#userId').val();

    if (groupId === tagetChatId) {
        var html = getChatTemplate("receiver", messageBody, dateTime, senderLastName, true);
        $('#messageColumn').append(html);
    } else {
        var currentUserId = $('#currUserId').val();
        chat.server.sendNotification(currentUserId, groupId, 0);
    }
}

chat.client.sendNotification = function (totalUnReadMessages, senderId) {
    $('#chat-rooms li').each(function (i, value) {
        var userId = $(value).attr('user-id');
        var groupId = $(value).attr('group-id');
        if (userId === senderId || groupId === senderId) {
            $(value).find('.badge').text(totalUnReadMessages);
        }
    });
}

chat.client.readMessage = function (senderId) {
    $('#chat-rooms li').each(function (i, value) {
        var userId = $(value).attr('user-id');
        var groupId = $(value).attr('group-id');
        if (userId === senderId || groupId === senderId) {
            $(value).find('.badge').text('');
        }
    });
}

chat.client.loadChatHistories = function (chatHistories) {
    var html = '';

    $.each(chatHistories, function (key, chatHistory) {
        var userLastName = chatHistory[3].includes("receiver") ? chatHistory[3].split('_')[1] : '';
        var isGroupChat = chatHistory[4] === 'group' ? true : false;
        html += getChatTemplate(chatHistory[0], chatHistory[1], chatHistory[2], userLastName, isGroupChat);
    });

    $('#messageColumn').append(html);
}

function getChatTemplate(senderOrReceiver, messageBody, dateTime, userLastName, isGroupChat) {
    var html = '<li class="left clearfix';
    if (senderOrReceiver === 'sender') {
        html += ' admin_chat"><span class="chat-img1 pull-right"><img src="/Content/images/no_propic.png" alt="User Avatar" class="img-circle"> </span>';
        html += '<div class="chat-body1 clearfix sender">';
    } else {
        html += '"><span class="chat-img1 pull-left"><img src="/Content/images/no_propic.png" alt="User Avatar" class="img-circle"> </span>';
        html += '<div class="chat-body1 clearfix receiver">';
    }

    html += '<p>' + messageBody + '</p>';
    if (userLastName && isGroupChat) html += '<div class="chat_time pull-left">From: <span style="font-style: oblique; font-size: 12px; text-decoration: underline;">' + userLastName + '</span></div>';
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