using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PIMSuite.Persistence.Entities;

namespace PIMSuite.Persistence.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        // Constructors

        public MessageRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        // Fields

        private readonly DataContext _dataContext;

        // Methods

        public IEnumerable<string[]> GetMessageHistories(Guid senderUserGuid, Guid receiverUserGuid)
        {
            var chatHistory = new List<string[]>();
            var messages =
                _dataContext.Messages
                .Where(m => 
                        m.ReceiverUserId.Equals(receiverUserGuid) && m.SenderUserId.Equals(senderUserGuid) ||
                        m.SenderUserId.Equals(receiverUserGuid) && m.ReceiverUserId.Equals(senderUserGuid)
                )
                .OrderBy(m=>m.CreatedAt)
                .ToList();
            var sender = _dataContext.Users.FirstOrDefault(u => u.UserId.Equals(senderUserGuid));
            var receiver = _dataContext.Users.FirstOrDefault(u => u.UserId.Equals(receiverUserGuid));

            foreach (var message in messages)
            {
                var senderOrReceiverLabel = message.SenderUserId.Equals(senderUserGuid) ? "sender" : "receiver";
                var userLastName = (senderOrReceiverLabel == "sender") ? "sender_" + sender.LastName : "receiver_" + receiver.LastName;
                var tmpArr = new[]
                {
                    senderOrReceiverLabel,
                    message.MessageBody,
                    message.CreatedAt.ToString("g"),
                    userLastName
                };

                chatHistory.Add(tmpArr);
            }

            return chatHistory;
        }

        public IEnumerable<string[]> GetGroupMessageHistories(Guid currentUserGuid, Guid groupGuid)
        {
            var chatHistory = new List<string[]>();
            var messages = _dataContext.Messages.Where(m => m.ReceiverUserId.Equals(groupGuid)).OrderBy(m => m.CreatedAt).ToList();

            foreach (var message in messages)
            {
                var senderOrReceiverLabel = message.SenderUserId.Equals(currentUserGuid) ? "sender" : "receiver";
                var user = _dataContext.Users.FirstOrDefault(u => u.UserId.Equals(message.SenderUserId));
                var userLastName = (user.UserId.Equals(currentUserGuid)) ? "sender_" + user.LastName : "receiver_" + user.LastName;
                var tmpArr = new[]
                {
                    senderOrReceiverLabel,
                    message.MessageBody,
                    message.CreatedAt.ToString("g"),
                    userLastName
                };

                chatHistory.Add(tmpArr);
            }

            return chatHistory;
        }

        public int InsertMessage(Guid senderUserGuid, Guid receiverUserGuid, string messageBody)
        {
            var message = new Message
            {
                ReceiverUserId = receiverUserGuid,
                SenderUserId = senderUserGuid,
                MessageBody = messageBody,
                IsRead = true
            };

            _dataContext.Messages.Add(message);
            _dataContext.SaveChanges();

            return message.MessageId;
        }

        public void UpdateMessageStatus(int messageGuid, bool isRead)
        {
            var message = _dataContext.Messages.FirstOrDefault(m => m.MessageId == messageGuid);

            if (message != null)
            {
                message.IsRead = isRead;
                _dataContext.SaveChanges();
            }
        }

        public void SetMessageStatusToRead(Guid receiverUserGuid, Guid senderUserGuid)
        {
            var messages =
                _dataContext.Messages.Where(m => m.ReceiverUserId.Equals(receiverUserGuid) && m.SenderUserId.Equals(senderUserGuid) && m.IsRead == false);

            foreach (var message in messages)
            {
                message.IsRead = true;
            }

            _dataContext.SaveChanges();
        }

        public int GetTotalUnReadMessages(Guid receiverUserGuid, Guid senderUserGuid)
        {
            var totalUnReadMessages =
                _dataContext.Messages
                    .Count(m => m.ReceiverUserId.Equals(receiverUserGuid) && m.SenderUserId.Equals(senderUserGuid) && m.IsRead == false);


            return totalUnReadMessages;
        }

        public IEnumerable<Dictionary<string, string>> GetNotificationOfUnReadMessage(Guid currentUserGuid)
        {
            var unReadMessages = new List<Dictionary<string, string>>();
            var messages =
                _dataContext.Messages
                    .Where(m => m.ReceiverUserId.Equals(currentUserGuid) && m.IsRead == false)
                    .OrderBy(m => m.CreatedAt)
                    .ToList();

            if (messages.Count > 0)
            {
                foreach(var message in messages)
                {
                    var senderId = message.SenderUserId;
                    var user = _dataContext.Users.FirstOrDefault(u => u.UserId.Equals(senderId));
                    var tmpArr = new Dictionary<string, string>
                {
                    {"senderId",  senderId.ToString()},
                    {"senderLastName", user.LastName },
                    {"messageBody",  message.MessageBody},
                    {"createdAt",  message.CreatedAt.ToString("D")}
                };

                    unReadMessages.Add(tmpArr);
                }
            }

            return unReadMessages;
        }
    }
}