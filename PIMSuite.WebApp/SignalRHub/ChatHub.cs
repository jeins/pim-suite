
using System;
using System.Collections.Generic;
using System.Linq;
using PIMSuite.Persistence;
using PIMSuite.Persistence.Entities;

namespace PIMSuite.WebApp.SignalRHub
{
    public class ChatHub : Microsoft.AspNet.SignalR.Hub
    {
        private DataContext _dataContext;

        public ChatHub()
        {
            _dataContext = new DataContext();
        }

        public void SendMessage(string toUserId, string message)
        {
            var receiver = _dataContext.Connections.FirstOrDefault(c => c.UserId.ToString().Equals(toUserId));
            var sender = _dataContext.Connections.FirstOrDefault(c => c.ConnectionId == Context.ConnectionId);
            var dbMessage = new Message
            {
                ReceiverUserId = new Guid(toUserId),
                SenderUserId = sender.UserId,
                MessageBody = message,
                IsRead = false
            };

            _dataContext.Messages.Add(dbMessage);
            _dataContext.SaveChanges();

            Clients.Client(Context.ConnectionId).onSendMessage(sender.User.Lastname, message);

            if (receiver != null) Clients.Client(receiver.ConnectionId).onSendMessage(receiver.User.Lastname, message);
        }

        public void UserConnect(string userId)
        {
            var connectionId = Context.ConnectionId;
            var userGuid = new Guid(userId);
            var connectedUsers = _dataContext.Connections.Where(c => c.UserId != userGuid);
            var currentUser = _dataContext.Users.FirstOrDefault(u => u.UserId.Equals(userGuid));

            if (!IsUserIdExistOnConnection(userGuid))
            {
                var connection = new Connection
                {
                    ConnectionId = connectionId,
                    UserId = userGuid
                };
                _dataContext.Connections.Add(connection);
                _dataContext.SaveChanges();
            }
            else
            {
                var connection = _dataContext.Connections.SingleOrDefault(c => c.UserId.Equals(userGuid));
                if (connection != null)
                {
                    connection.ConnectionId = connectionId;
                    _dataContext.SaveChanges();
                }
            }

            Clients.Caller.onConnected(connectedUsers.ToArray());
            Clients.AllExcept(connectionId).onNewUserConnected(userId, currentUser);
        }

        //TODO: needed to move MessageRepository
        private List<string[]> GetChatHistory(Guid senderUserId, Guid receiverUserId)
        {
            var chatHistory = new List<string[]>();
            var messages =
                _dataContext.Messages
                            .Where(m => m.ReceiverUserId.Equals(receiverUserId) && m.SenderUserId.Equals(senderUserId))
                            .ToList();
            return null;
        }

        private bool IsUserIdExistOnConnection(Guid userId)
        {
            var count = _dataContext.Connections.Count(u => u.UserId == userId);
            if (count > 0) return true;

            return false;
        }
    }
}