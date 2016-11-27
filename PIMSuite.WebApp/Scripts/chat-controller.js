var chat = $.connection.chatHub;

$.connection.hub.start().done(function () {
    chat.server.userConnect($('#currUserId').val());
});

$('#sendMessage').click(function () {
    chat.server.sendMessage($('#userId').val(), $('#messageBody').val());
    $('#messageBody').val('').focus();
});

$('#users').on('click', 'li#userList', function () {
    var sender = $('#currUserId').val();
    var receiver = $(this).attr('user-id');

    $('.message_section').show();
    $('#users li').removeClass('selectedUser');
    $('#messageColumn li').remove();
    $('#userId').val(receiver);
    $('#chatWith').text('chat with: ' + $(this).attr('user-lastname'));
    $(this).toggleClass('selectedUser');

    chat.server.loadChatHistories(sender, receiver);
    chat.server.readMessage(sender, receiver);
});


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
    $('#users li').each(function (i, value) {
        var userId = $(value).attr('user-id');
        if (userId === senderUserId) {
            $(value).find('.badge').text(totalUnReadMessages);
        }
    });
}

chat.client.readMessage = function (senderUserId) {
    $('#users li').each(function (i, value) {
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
    html += '<div class="chat_time pull-left">' + dateTime + '</div>';
    html += '</div></li>';
    return html;
}