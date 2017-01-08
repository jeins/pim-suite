using PIMSuite.Persistence.Entities;
using System;
using System.Collections.Generic;

namespace PIMSuite.Persistence.Repositories
{
    public interface IEvent_InviteRepository
    {
        void InsertEvent_Invite(Event_Invite ei);
        void DeleteEvent_Invite(int inviteId);
        Event_Invite GetInvite(int inviteId);
        Event_Invite GetInviteByEventAndReceiver(int eventId, Guid receiverId);
        void Save(); 
    }
}