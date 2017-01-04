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
        [DataMember(IsRequired = true)]
        [Required]
        public string Location { get; set; }
        [DataMember(IsRequired = true)]
        [Required]
        public string Description { get; set; }
        public int CalendarId { get; set; }

        [DataMember(IsRequired = true)]
        [Required]
        public bool IsPrivate { get; set; }
    }

    public class CalendarEventController : ApiController
    {
        private readonly ICalendar_EventRepository _calendarEventRepository;
        private readonly ICalendar_SubscriptionRepository _calendarSubscriptionRepository;
        private readonly INotificationRepository _notificationRepository;

        public CalendarEventController()
        {
            var dataContext = new DataContext();
            _calendarEventRepository = new Calendar_EventRepository(dataContext);
            _calendarSubscriptionRepository = new Calendar_SubscriptionRepository(dataContext);
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

        public HttpResponseMessage GetEvents(string userId, int calendarId)
        {
            var displayAllEvent = DisplayAllEvent(new Guid(userId));
            var eventList = _calendarEventRepository.GetAllCalendar_EventByUserIdAndCalendarId(new Guid(userId), calendarId)
                .Where(c => c.IsPrivate == displayAllEvent || c.IsPrivate == false)
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
                    allday = false
                }
            );

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