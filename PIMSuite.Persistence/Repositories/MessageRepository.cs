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

        private DataContext _dataContext;

        public MessageRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public IEnumerable<string[]> GetMessageHistories(Guid senderUserGuid, Guid receiverUserGuid)
        {
            var chatHistory = new List<string[]>();
            var messages =
                _dataContext.Messages
                .Where(m => 
                        m.ReceiverUserId.Equals(receiverUserGuid) && m.SenderUserId.Equals(senderUserGuid) ||
                        m.SenderUserId.Equals(receiverUserGuid) && m.ReceiverUserId.Equals(senderUserGuid)
                )
                .ToList();

            foreach (var message in messages)
            {
                var senderOrReceiverLabel = message.SenderUserId.Equals(senderUserGuid) ? "sender" : "receiver";
                var tmpArr = new[]
                {
                    senderOrReceiverLabel,
                    message.MessageBody,
                    message.CreatedAt.ToString("g")
                };

                chatHistory.Add(tmpArr);
            }

            return chatHistory;
        }

        public Guid InsertMessage(Guid senderUserGuid, Guid receiverUserGuid, string messageBody)
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

        public void UpdateMessageStatus(Guid messageGuid, bool isRead)
        {
            var message = _dataContext.Messages.FirstOrDefault(m => m.MessageId == messageGuid);

            if (message != null)
            {
                message.IsRead = isRead;
                _dataContext.SaveChanges();
            }
        }

        public IEnumerable<string[]> GetUnReadMessages(Guid receiverUserGuid)
        {
            var unReadMessages = new List<string[]>();
            var messages =
                _dataContext.Messages
                    .Where(m => m.ReceiverUserId.Equals(receiverUserGuid) && m.IsRead == false)
                    .ToList();

            foreach (var message in messages)
            {
                var tmpArr = new[]
                {
                    message.MessageBody,
                    message.CreatedAt.ToString("g")
                };

                unReadMessages.Add(tmpArr);
            }

            return unReadMessages;
        }
    }
}
