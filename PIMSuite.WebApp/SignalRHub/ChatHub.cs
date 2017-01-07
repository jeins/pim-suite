
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PIMSuite.Persistence;
using PIMSuite.Persistence.Repositories;

namespace PIMSuite.WebApp.SignalRHub
{
    public class ChatHub : Microsoft.AspNet.SignalR.Hub
    {
        private readonly DataContext _dataContext;
        private readonly IConnectionRepository _connectionRepository;
        private readonly IMessageRepository _messageRepository;

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
            if (sender != null)
            {
                var messageId = _messageRepository.InsertMessage(sender.UserId, new Guid(toUserId), message);
                var dateTime = new DateTime().ToString("g");

                Clients.Client(Context.ConnectionId).onSendMessageToSender(sender.User.LastName, message, dateTime, "sender");

                if (receiver != null)
                {
                    Clients.Client(receiver.ConnectionId)
                        .onSendMessageToReceiver(messageId, message, sender.UserId, receiver.UserId, dateTime, "receiver");
                }
                else
                {
                    _messageRepository.UpdateMessageStatus(messageId, false);
                }
            }
        }

        public void SendGroupMessage(string groupId, string message)
        {
            var sender = _dataContext.Connections.FirstOrDefault(c => c.ConnectionId == Context.ConnectionId);
            var group = _dataContext.ChatGroups.FirstOrDefault(g => g.GroupId == new Guid(groupId));

            if (sender != null && group != null)
            {
                var messageId = _messageRepository.InsertMessage(sender.UserId, new Guid(groupId), message);
                var dateTime = new DateTime().ToString("g");
                
                Clients.Caller.onSendMessageToSender(sender.User.LastName, message, dateTime, "sender");
                Clients.OthersInGroup(group.GroupName).onSendMessageToGroup(message, sender.UserId, dateTime, sender.User.LastName, groupId);
            }
        }

        public void UserConnect(string userId)
        {
            var userGuid = new Guid(userId);
            var connectedUsers = _connectionRepository.GetConnectedUsers(userGuid);

            if (!IsUserIdExistOnConnection(userGuid))
            {
                _connectionRepository.InsertConnection(userGuid, Context.ConnectionId);
            }
            else
            {
                _connectionRepository.UpdateConnection(userGuid, Context.ConnectionId);

            }

            UpdateUserGroupConnection(userId, Context.ConnectionId);

            Clients.Caller.loadConnectedUser(connectedUsers.ToArray());
            Clients.AllExcept(Context.ConnectionId).onNewUserConnected(userId);
        }

        public void LoadChatHistories(string senderUserId, string receiverUserId, bool isGroup)
        {
            IEnumerable<string[]> chatHistories;

            if (!isGroup)
            {
                chatHistories = _messageRepository.GetMessageHistories(new Guid(senderUserId), new Guid(receiverUserId));
            }
            else
            {
                chatHistories = _messageRepository.GetGroupMessageHistories(new Guid(senderUserId), new Guid(receiverUserId));
            }

            Clients.Caller.loadChatHistories(chatHistories);
        }

        public void SendNotification(string receiverUserId, string senderUserId, int messageId)
        {
            _messageRepository.UpdateMessageStatus(messageId, false);
            var receiverConnection = _dataContext.Connections.FirstOrDefault(c => c.UserId == new Guid(receiverUserId));

            if (receiverConnection != null)
            {
                if (messageId != 0)
                {
                    var unReadMessages = _messageRepository.GetTotalUnReadMessages(new Guid(receiverUserId),
                        new Guid(senderUserId));

                    Clients.Client(receiverConnection.ConnectionId).sendNotification(unReadMessages, senderUserId);
                }
                else
                {
                    var userChatGroup = _dataContext.UserChatGroups.FirstOrDefault(u => u.UserId.Equals(new Guid(receiverUserId)) && u.GroupId.Equals(new Guid(senderUserId)));

                    if (userChatGroup != null)
                    {
                        userChatGroup.NumUnReadMessage++;
                        _dataContext.SaveChanges();

                        Clients.Client(receiverConnection.ConnectionId).sendNotification(userChatGroup.NumUnReadMessage, senderUserId);
                    }
                }
            }
        }

        public void ReadMessage(string receiverId, string senderId)
        {
            var chatGroup = _dataContext.ChatGroups.FirstOrDefault(c => c.GroupId.Equals(new Guid(senderId)));

            if (chatGroup != null)
            {
                var userChatGroup =
                    _dataContext.UserChatGroups.FirstOrDefault(
                        u => u.UserId.Equals(new Guid(receiverId)) && u.GroupId.Equals(new Guid(senderId)));
                if (userChatGroup != null)
                {
                    userChatGroup.NumUnReadMessage = 0;
                    _dataContext.SaveChanges();
                }
            }
            else
            {
                _messageRepository.SetMessageStatusToRead(new Guid(receiverId), new Guid(senderId));
            }

            Clients.Client(Context.ConnectionId).readMessage(senderId);
        }

        public void AddUserToGroup(string groupName, string userId)
        {
            var userConnectionId = _dataContext.Connections.FirstOrDefault(c => c.UserId.Equals(new Guid(userId)));

            if (userConnectionId != null)
            {
                Groups.Add(userConnectionId.ConnectionId, groupName);
            }
        }

        private void UpdateUserGroupConnection(string userId, string connectionId)
        {
            var userGroups = _dataContext.UserChatGroups.Where(u => u.UserId.Equals(new Guid(userId)));

            foreach (var userGroup in userGroups)
            {
                Groups.Add(connectionId, userGroup.ChatGroup.GroupName);
            }
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            var userId = _connectionRepository.RemoveUser(Guid.Empty, Context.ConnectionId);

            if (userId == Guid.Empty) return base.OnDisconnected(stopCalled);

            var userGroups = _dataContext.UserChatGroups.Where(u => u.UserId.Equals(userId));
            foreach (var userGroup in userGroups)
            {
                Groups.Remove(Context.ConnectionId, userGroup.ChatGroup.GroupName);
            }

            Clients.All.onUserDisconnected(userId.ToString());

            return base.OnDisconnected(stopCalled);
        }

        private bool IsUserIdExistOnConnection(Guid userId)
        {
            var count = _dataContext.Connections.Count(u => u.UserId == userId);

            return count > 0;
        }
    }
}