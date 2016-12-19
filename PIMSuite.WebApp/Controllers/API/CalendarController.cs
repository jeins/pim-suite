using System;
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
        }

        // Fields

        private readonly DataContext _dataContext;
        private readonly ICalendarRepository _calendarRepository;

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
    }
}