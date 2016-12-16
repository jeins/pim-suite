using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using PIMSuite.Persistence;
using PIMSuite.Persistence.Repositories;

namespace PIMSuite.WebApp.Controllers
{
    [Authorize]
    public class CalendarController : BaseController
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

        public ActionResult Index()
        {
            ViewBag.CalendarList = _calendarRepository.GetAllUserCalendars(ViewBag.User.UserId);

            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Show(int calendarId)
        {
            var calendar = _calendarRepository.GetCalendarByCalendarId(calendarId);

            ViewBag.CalendarName = calendar.Name;

            return View();
        }
    }
}