using System;
using System.Collections.Generic;
using System.Linq;
using PIMSuite.Persistence.Entities;

namespace PIMSuite.Persistence.Repositories
{
    public class Event_InviteRepository : IEvent_InviteRepository
    {
        // Constructors

        public Event_InviteRepository(DataContext context)
        {
            _context = context;
        }

        // Fields

        private readonly DataContext _context;

        // Methods

        public void DeleteEvent_Invite(int inviteId)
        {
            _context.EventInvites.Remove(_context.EventInvites.SingleOrDefault(c => c.InviteId == inviteId));
        }

        public Event_Invite GetInvite(int inviteId)
        {
            return _context.EventInvites.SingleOrDefault(c => c.InviteId == inviteId);
        }

        public void InsertEvent_Invite(Event_Invite ei)
        {
            _context.EventInvites.Add(ei);
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}