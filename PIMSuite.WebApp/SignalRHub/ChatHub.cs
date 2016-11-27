
using System;
using System.Collections.Generic;
using System.Linq;
using PIMSuite.Persistence;
using PIMSuite.Persistence.Entities;
using PIMSuite.Persistence.Repositories;

namespace PIMSuite.WebApp.SignalRHub
{
    public class ChatHub : Microsoft.AspNet.SignalR.Hub
    {
        private DataContext _dataContext;
        private IConnectionRepository _connectionRepository;
        private IMessageRepository _messageRepository;

        public ChatHub()
        {
            _dataContext = new DataContext();
            _connectionRepository = new ConnectionRepository(_dataContext);
            _messageRepository = new MessageRepository(_dataContext);
        }

        public void SendMessage(string toUserId, string message)
        {
            var receiver = _dataContext.Connections.FirstOrDefault(c => c.UserId.ToString().Equals(toUserId));
            var sender = _dataContext.Connections.FirstOrDefault(c => c.ConnectionId == Context.ConnectionId);

            var messageId = _messageRepository.InsertMessage(sender.UserId, new Guid(toUserId), message);

            Clients.Client(Context.ConnectionId).onSendMessageToSender(sender.User.Lastname, message, "sender");

            if (receiver != null) Clients.Client(receiver.ConnectionId).onSendMessageToReceiver(messageId, message, sender.UserId, receiver.UserId, "receiver");
        }

        public void UserConnect(string userId)
        {
            var connectionId = Context.ConnectionId;
            var userGuid = new Guid(userId);
            var connectedUsers = _connectionRepository.GetConnectedUsers(userGuid);
            var currentUser = _dataContext.Users.FirstOrDefault(u => u.UserId.Equals(userGuid));

            if (!IsUserIdExistOnConnection(userGuid))
            {
                _connectionRepository.InsertConnection(userGuid, connectionId);
            }
            else
            {
                _connectionRepository.UpdateConnection(userGuid, connectionId);
            }

            Clients.Caller.onConnected(connectedUsers.ToArray());
            Clients.AllExcept(connectionId).onNewUserConnected(userId, currentUser);
        }

        public void LoadChatHistories(string senderUserId, string receiverUserId)
        {
            var chatHistories = _messageRepository.GetMessageHistories(new Guid(senderUserId), new Guid(receiverUserId));

            Clients.Caller.loadChatHistories(chatHistories);
        }

        public void SendNotification(string receiverUserId, string senderUserId, string messageId)
        {
            _messageRepository.UpdateMessageStatus(new Guid(messageId), false);
            var receiverConnection = _dataContext.Connections.FirstOrDefault(c => c.UserId == new Guid(receiverUserId));

            if (receiverConnection != null)
            {
                var unReadMessages = _messageRepository.GetUnReadMessages(new Guid(receiverUserId));

                Clients.Client(receiverConnection.ConnectionId).sendNotification(unReadMessages, senderUserId);
            }
        }

        private bool IsUserIdExistOnConnection(Guid userId)
        {
            var count = _dataContext.Connections.Count(u => u.UserId == userId);
            if (count > 0) return true;

            return false;
        }
    }
}