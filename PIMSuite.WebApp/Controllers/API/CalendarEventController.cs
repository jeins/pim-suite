using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Web;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using PIMSuite.Persistence;
using PIMSuite.Persistence.Entities;
using PIMSuite.Persistence.Repositories;

namespace PIMSuite.WebApp.Controllers.API
{
    [DataContract]
    public class CreateCalendarEventModel
    {
        [DataMember(IsRequired = true)]
        [Required(ErrorMessage = "Title ist erforderlich!")]
        public string Title { get; set; }

        [DataMember(IsRequired = true)]
        [Required]
        public string Start { get; set; }

        [DataMember(IsRequired = true)]
        [Required]
        public string End { get; set; }
        
        public string Location { get; set; }
       
        public string Description { get; set; }
        public int CalendarId { get; set; }

        [DataMember(IsRequired = true)]
        [Required]
        public bool IsPrivate { get; set; }
    }

    [DataContract]
    public class InviteUserModel
    {
        [DataMember(IsRequired = true)]
        [Required]
        public string UsersId { get; set; }

        [DataMember(IsRequired = true)]
        [Required]
        public int EventId { get; set; }
    }

    public class CalendarEventController : ApiController
    {
        private readonly ICalendar_EventRepository _calendarEventRepository;
        private readonly ICalendar_SubscriptionRepository _calendarSubscriptionRepository;
        private readonly IEvent_InviteRepository _eventInviteRepository;
        private readonly INotificationRepository _notificationRepository;
        private DataContext dataContext;

        public CalendarEventController()
        {
            dataContext = new DataContext();
            _calendarEventRepository = new Calendar_EventRepository(dataContext);
            _calendarSubscriptionRepository = new Calendar_SubscriptionRepository(dataContext);
            _eventInviteRepository = new Event_InviteRepository(dataContext);
            _notificationRepository = new NotificationRepository(dataContext);
        }

        [HttpPost]
        public HttpResponseMessage CreateNewEvent(CreateCalendarEventModel model)
        {
            if (model != null && ModelState.IsValid)
            {
                var userId = Guid.Parse(HttpContext.Current.GetOwinContext().Authentication.User.Identity.GetUserId());
                var calendarEvent = new Calendar_Event()
                {
                    OwnerId = userId,
                    CalendarId = model.CalendarId,
                    Name = model.Title,
                    StartsAt = DateTime.Parse(model.Start),
                    EndsAt = DateTime.Parse(model.End),
                    Description = model.Description,
                    Location = model.Location,
                    IsPrivate = model.IsPrivate
                };

                _calendarEventRepository.InsertCalendar_Event(calendarEvent);
                _calendarEventRepository.Save();

                return Request.CreateResponse(HttpStatusCode.Accepted, calendarEvent.CalendarId);
            }
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "invalid data");
        }

        public class EditEventModel
        {
            [DataMember(IsRequired = true)]
            [Required]
            public int EventId { get; set; }

            [DataMember(IsRequired = true)]
            [Required(ErrorMessage = "Title ist erforderlich!")]
            public string Title { get; set; }

            [DataMember(IsRequired = true)]
            [Required]
            public string Start { get; set; }

            [DataMember(IsRequired = true)]
            [Required]
            public string End { get; set; }
            [DataMember(IsRequired = true)]
            [Required]
            public string Location { get; set; }

            [DataMember(IsRequired = true)]
            [Required]
            public string Description { get; set; }

            [DataMember(IsRequired = true)]
            [Required]
            public bool IsPrivate { get; set; }

            [DataMember(IsRequired = true)]
            [Required]
            public bool IsConfirmed { get; set; }
        }

        [HttpPost]
        public HttpResponseMessage EditEvent(EditEventModel model)
        {
            if (model != null && ModelState.IsValid)
            {
                var evt = _calendarEventRepository.GetEvent(model.EventId);

                if (evt == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "event not found");
                }

                var confirmationChanged = model.IsConfirmed && !evt.Confirmed;

                evt.StartsAt = DateTime.Parse(model.Start);
                evt.EndsAt = DateTime.Parse(model.End);
                evt.Name = model.Title;
                evt.Description = model.Description;
                evt.Location = model.Location;
                evt.IsPrivate = model.IsPrivate;
                evt.Confirmed = model.IsConfirmed;

                _calendarEventRepository.Save();

                // notification

                if (confirmationChanged)
                {
                    var msg = string.Format("Termin \"{0}\" findet wirklich statt.", evt.Name);
                    var subscriptions = _calendarSubscriptionRepository.GetAllSubscriptionsByCalendarId(evt.CalendarId);
                    foreach (var subscription in subscriptions)
                    {
                        var notification = new Notification
                        {
                            UserId = subscription.SubscriberId,
                            Message = msg
                        };
                        _notificationRepository.InsertNotification(notification);
                    }
                    _notificationRepository.Save();
                }
            }
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "invalid data");
        }

        [HttpPost]
        public HttpResponseMessage DeleteEvent(int eventId)
        {
            var userId = Guid.Parse(HttpContext.Current.GetOwinContext().Authentication.User.Identity.GetUserId());
            var evt = _calendarEventRepository.GetEvent(eventId);

            if (_calendarEventRepository.GetEvent(eventId).OwnerId == userId)
            {
                // delete event

                _calendarEventRepository.DeleteCalendar_Event(eventId);
                _calendarEventRepository.Save();

                // create notification

                var msg = string.Format("Termin \"{0}\" wurde gelöscht.", evt.Name);
                var subscriptions = _calendarSubscriptionRepository.GetAllSubscriptionsByCalendarId(evt.CalendarId);
                foreach (var subscription in subscriptions)
                {
                    var notification = new Notification
                    {
                        UserId = subscription.SubscriberId,
                        Message = msg
                    };
                    _notificationRepository.InsertNotification(notification);
                }
                _notificationRepository.Save();

                // return result

                return Request.CreateResponse(HttpStatusCode.Accepted, eventId);
            }
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "invalid data");
        }

        public HttpResponseMessage ProcessInvite(int eventId, int status)
        {
            var userId = Guid.Parse(HttpContext.Current.GetOwinContext().Authentication.User.Identity.GetUserId());
            var evt = _calendarEventRepository.GetEvent(eventId);
            var invt = _eventInviteRepository.GetInviteByEventAndReceiver(eventId, userId);

            if (invt.InviteReceiverId == userId)
            {
                invt.Status = status;
                _eventInviteRepository.Save();
                return Request.CreateResponse(HttpStatusCode.Accepted, invt.InviteId);
            }
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "invalid data");
        }

        public HttpResponseMessage GetUsersForInvite(int eventId)
        {
            var self = HttpContext.Current.GetOwinContext().Authentication.User.Identity.Name;
            var userList = dataContext.Users.Select(u => new { u.UserId, u.FirstName, u.LastName, u.Username }).ToList();
            var userAlreadyInvited = dataContext.EventInvites.Where(e => e.InviteEventId.Equals(eventId)).Select(e => e.InviteReceiverId).ToList();
            var ownUser = dataContext.Users.Where(u => u.Username.Equals(self)).Select(u => u.Username).ToList();

            userList.RemoveAll(u => ownUser.Contains(u.Username));
            userList.RemoveAll(u => userAlreadyInvited.Contains(u.UserId));

            return Request.CreateResponse(HttpStatusCode.OK, userList, Configuration.Formatters.JsonFormatter);
        }

        [HttpPost]
        public HttpResponseMessage InviteUser(InviteUserModel model)
        {
            if (model != null && ModelState.IsValid)
            {
                var self = HttpContext.Current.GetOwinContext().Authentication.User.Identity.Name;
                var selfUser = dataContext.Users.SingleOrDefault(u => u.Username == self);
                var targetevent = dataContext.CalendarEvents.SingleOrDefault(e => e.EventId == model.EventId);
                var users = model.UsersId.Split('_');
                foreach (var userId in users)
                {
                    var recvrGuid = Guid.Parse(userId);
                    var recvr = dataContext.Users.SingleOrDefault(u => u.UserId == recvrGuid);
                    var ei = new Event_Invite
                    {
                        InviteSender = selfUser,
                        InviteReceiver = recvr,
                        InviteEvent = targetevent,
                        Status = 0
                    };

                    dataContext.EventInvites.Add(ei);
                }

                dataContext.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.Accepted, "");
            }

            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "invalid data");
        }

        public HttpResponseMessage GetEvents(string userId, int calendarId)
        {
            var displayAllEvent = DisplayAllEvent(new Guid(userId));
            var eventList = _calendarEventRepository.GetAllCalendar_EventByUserIdAndCalendarId(new Guid(userId), calendarId)
                .Where(c => !c.IsPrivate || displayAllEvent)
                .Select(c => new
                {
                    id = c.EventId,
                    title = c.Name,
                    description = c.Description,
                    location = c.Location,
                    start = c.StartsAt.ToString(("s")),
                    end = c.EndsAt.ToString("s"),
                    isPrivateEvent = c.IsPrivate,
                    displayAllEvent = displayAllEvent,
                    allday = false,
                    isConfirmed = c.Confirmed,
                    invites = c.Invites.ToList(),
                    isInvite = false,
                    isInviteProcessed = false,
                    backgroundColor = "#3a87ad",
                    borderColor = "#3a87ad",
                    textColor = "#ffffff"
                }
            );
            var privateEventList = _calendarEventRepository.GetAllCalendar_EventByUserIdAndCalendarId(new Guid(userId), calendarId)
                .Where(c => c.IsPrivate && !displayAllEvent)
                .Select(c => new
                {
                    id = c.EventId,
                    title = "Privat",
                    description = "Privat",
                    location = "Privat",
                    start = c.StartsAt.ToString(("s")),
                    end = c.EndsAt.ToString("s"),
                    isPrivateEvent = c.IsPrivate,
                    displayAllEvent = displayAllEvent,
                    allday = false,
                    isConfirmed = false,
                    invites = c.Invites.ToList(),
                    isInvite = false,
                    isInviteProcessed = false,
                    backgroundColor = "#3a87ad",
                    borderColor = "#3a87ad",
                    textColor = "#ffffff"
                }
            );
            var acceptedInvites = _calendarEventRepository.GetProcessedInvites(new Guid(userId)).Select(c => new
            {
                id = c.EventId,
                title = c.Name,
                description = c.Description,
                location = c.Location,
                start = c.StartsAt.ToString(("s")),
                end = c.EndsAt.ToString("s"),
                isPrivateEvent = c.IsPrivate,
                displayAllEvent = false,
                allday = false,
                isConfirmed = c.Confirmed,
                invites = c.Invites.ToList(),
                isInvite = true,
                isInviteProcessed = true,
                backgroundColor = "#3a87ad",
                borderColor = "#3a87ad",
                textColor = "#ffffff"
            }
            );
            var invitedEvents = _calendarEventRepository.GetInvites(new Guid(userId)).Select(c => new
            {
                id = c.EventId,
                title = c.Name,
                description = c.Description,
                location = c.Location,
                start = c.StartsAt.ToString(("s")),
                end = c.EndsAt.ToString("s"),
                isPrivateEvent = c.IsPrivate,
                displayAllEvent = false,
                allday = false,
                isConfirmed = c.Confirmed,
                invites = c.Invites.ToList(),
                isInvite = true,
                isInviteProcessed = false,
                backgroundColor = "#666666",
                borderColor = "#000000",
                textColor = "#999999"
            }
            );
            eventList = eventList.Concat(privateEventList);
            eventList = eventList.Concat(acceptedInvites);
            eventList = eventList.Concat(invitedEvents);

            return Request.CreateResponse(HttpStatusCode.OK, eventList, Configuration.Formatters.JsonFormatter);
        }

        private bool DisplayAllEvent(Guid eventOwner)
        {
            var currUserId = Guid.Parse(HttpContext.Current.GetOwinContext().Authentication.User.Identity.GetUserId());

            if (currUserId.Equals(eventOwner))
            {
                return true;
            }

            return false;
        }
    }
}