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

        public CalendarEventController()
        {
            var dataContext = new DataContext();
            _calendarEventRepository = new Calendar_EventRepository(dataContext);
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
                    isPrivate = model.IsPrivate
                };

                _calendarEventRepository.InsertCalendar_Event(calendarEvent);
                _calendarEventRepository.Save();


                return Request.CreateResponse(HttpStatusCode.Accepted, calendarEvent.CalendarId);
            }
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "invalid data");
        }

        [HttpPost]
        public HttpResponseMessage DeleteEvent(int eventId)
        {
            var userId = Guid.Parse(HttpContext.Current.GetOwinContext().Authentication.User.Identity.GetUserId());
            if (_calendarEventRepository.GetEvent(eventId).OwnerId==userId)
            {
                _calendarEventRepository.DeleteCalendar_Event(eventId);
                _calendarEventRepository.Save();
                return Request.CreateResponse(HttpStatusCode.Accepted, eventId);
            }
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "invalid data");
        }

        public HttpResponseMessage GetEvents(string userId, int calendarId)
        {
            var displayAllEvent = DisplayAllEvent(new Guid(userId));
            var eventList = _calendarEventRepository.GetAllCalendar_EventByUserIdAndCalendarId(new Guid(userId), calendarId)
                .Where(c => c.isPrivate == displayAllEvent || c.isPrivate == false)
                .Select(c => new
                {
                    id = c.EventId,
                    title = c.Name,
                    description = c.Description,
                    location = c.Location,
                    start = c.StartsAt.ToString(("s")),
                    end = c.EndsAt.ToString("s"),
                    isPrivateEvent = c.isPrivate,
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