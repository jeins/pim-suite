using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using PIMSuite.Persistence;
using PIMSuite.Persistence.Repositories;
using PIMSuite.Persistence.Entities;
using Microsoft.AspNet.Identity;

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
            _calendarEventRepository = new Calendar_EventRepository(_dataContext);
        }

        // Fields

        private readonly DataContext _dataContext;
        private readonly ICalendarRepository _calendarRepository;
        private readonly ICalendar_EventRepository _calendarEventRepository;

        // Methods

        public ActionResult Index()
        {
            ViewBag.CalendarList = _calendarRepository.GetAllCalendarsByUserId(ViewBag.User.UserId);

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
            ViewBag.CalendarId = calendar.CalendarId;

            return View();
        }

        public ActionResult CreateEvent(int  CalendarId)
        {


            Calendar_Event _event = new Calendar_Event();
            var userId = Guid.Parse(User.Identity.GetUserId());

            _event.OwnerId = userId;
            _event.CalendarId = CalendarId;

            return View(_event);

        }

        [HttpPost]
        public ActionResult CreateEvent(Calendar_Event _event)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _calendarEventRepository.InsertCalendar_Event(_event);
                    _calendarEventRepository.Save();
                    return RedirectToAction("Index");
                }

            }
            catch (Exception ex)
            {
                ModelState.AddModelError(String.Empty, ex);
            }
            return View(_event);

        }
    }
}