using System;
using System.Linq;
using System.ComponentModel.DataAnnotations;
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
    public class CreateCalendarModel
    {
        [DataMember(IsRequired = true)]
        [Required]
        public string Name { get; set; }
    }

    public class CalendarController : ApiController
    {
        // Constructors

        public CalendarController()
        {
            _dataContext = new DataContext();
            _calendarRepository = new CalendarRepository(_dataContext);
            _calendarEventRepository = new Calendar_EventRepository(_dataContext);
        }

        // Fields

        private readonly DataContext _dataContext;
        private readonly ICalendarRepository _calendarRepository;
        private readonly ICalendar_EventRepository _calendarEventRepository;

        // Methods

        [HttpPost]
        public HttpResponseMessage CreateCalendar(CreateCalendarModel model)
        {
            if (model != null && ModelState.IsValid)
            {
                var userId = Guid.Parse(HttpContext.Current.GetOwinContext().Authentication.User.Identity.GetUserId());
                var calendar = new Calendar
                {
                    Name = model.Name,
                    OwnerId = userId
                };

                _calendarRepository.InsertCalendar(calendar);
                _calendarRepository.Save();

                return Request.CreateResponse(HttpStatusCode.Accepted, calendar.CalendarId);
            }
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "invalid data");
        }

        public HttpResponseMessage GetCalendarEvents(string userId, int calendarId)
        {
            var eventList = _calendarEventRepository.GetAllCalendar_EventByUserIdAndCalendarId(new Guid(userId), calendarId)
                .Select(c => new
                {
                    id = c.EventId,
                    title = c.Name,
                    description = c.Description,
                    location = c.Location,
                    start = c.StartsAt.ToString(("s")),
                    end  = c.EndsAt.ToString("s"),
                    allday = false
                }
            );

            return Request.CreateResponse(HttpStatusCode.OK, eventList, Configuration.Formatters.JsonFormatter);
        }
    }
}