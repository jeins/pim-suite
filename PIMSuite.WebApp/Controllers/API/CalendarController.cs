﻿using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using PIMSuite.Persistence;
using PIMSuite.Persistence.Entities;
using PIMSuite.Persistence.Repositories;

namespace PIMSuite.WebApp.Controllers.API
{
    public class CreateCalendarModel
    {
        public string Name { get; set; }
        public bool IsPrivate { get; set; }
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
            var calendar = new Calendar()
            {
                
            }

            return Request.CreateResponse(HttpStatusCode.Accepted);
        }
    }
}