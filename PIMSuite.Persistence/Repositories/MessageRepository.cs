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
        private DataContext context;

        public MessageRepository(DataContext dataContext)
        {
            this.context = dataContext;
        }

        public void DeleteMessage(Guid MessageId)
        {
            context.Messages.Remove(context.Messages.Find(MessageId));
        }


        public IEnumerable<Message> GetConversationOfTwoUsers(Guid FirstUser, Guid SecondUser)
        {
            var conversation = from m in context.Messages
                               where (m.SenderId == FirstUser && m.ReceiverId == SecondUser) || (m.SenderId == SecondUser && m.ReceiverId == FirstUser)
                               orderby m.Creation descending
                               select m;

            return conversation.ToList();
        }

        public void InsertMessage(Message m)
        {
            context.Messages.Add(m);
        }

        public void Save()
        {
            context.SaveChanges();
        }
    }
}
