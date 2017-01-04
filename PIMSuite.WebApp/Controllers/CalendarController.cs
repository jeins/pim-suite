using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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
            _userRepository = new UserRepository(_dataContext);
            _calendarEventRepository = new Calendar_EventRepository(_dataContext);
            _subscriptionRepository = new Calendar_SubscriptionRepository(_dataContext);
        }

        // Fields

        private readonly DataContext _dataContext;
        private readonly ICalendarRepository _calendarRepository;
        private readonly ICalendar_EventRepository _calendarEventRepository;
        private readonly UserRepository _userRepository;
        private readonly ICalendar_SubscriptionRepository _subscriptionRepository;

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
            var userId = Guid.Parse(HttpContext.GetOwinContext().Authentication.User.Identity.GetUserId());
            var calendar = _calendarRepository.GetCalendarByCalendarId(calendarId);

            string check = "inherit";
            if (calendar.OwnerId == userId)
            {
                check = "none";
            }
            ViewBag.UserCheck = check;
            ViewBag.CalendarName = calendar.Name;
            ViewBag.CalendarId = calendar.CalendarId;
            ViewBag.UserId = calendar.OwnerId.ToString();
            ViewBag.OwnerName= _userRepository.GetUserByID(calendar.OwnerId).FirstName+" "+_userRepository.GetUserByID(calendar.OwnerId).LastName;
            ViewBag.DisplayAll = User.Identity.GetUserId().Equals(calendar.OwnerId.ToString());
            ViewBag.IsOwner = calendar.OwnerId == userId;

            var flag = _subscriptionRepository.SubscriptionContainsInUserList(calendarId, userId);
            if (flag == true)
            {
                ViewBag.Flag = "deabonnieren";
            }
            else
            {
                ViewBag.Flag = "abonnieren";
            }
            
            
            return View();
        }

        [HttpPost]
        public void CreateSubscription(int calendarId)
        {
            var sub = new Calendar_Subscription
            {
                CalendarId = calendarId,
                SubscriberId = Guid.Parse(HttpContext.GetOwinContext().Authentication.User.Identity.GetUserId())
            };

            _subscriptionRepository.Insert(sub);
            _subscriptionRepository.Save();
        }

        [HttpPost]
        public void RemoveSubscription(int calendarId)
        {
            _subscriptionRepository.Delete(calendarId, Guid.Parse(HttpContext.GetOwinContext().Authentication.User.Identity.GetUserId()));
            _subscriptionRepository.Save();
        }

        public ActionResult List(Guid userId)
        {
            var check = "";
            var subscByUser = _subscriptionRepository.GetAllSubscriptionsByUserId(Guid.Parse(HttpContext.GetOwinContext().Authentication.User.Identity.GetUserId()));
            var profileCalendars = _calendarRepository.GetAllCalendarsByUserId(userId);
            var calendarsBySubs = new List<Calendar>();
            foreach (Calendar_Subscription cs in subscByUser)
            {
                calendarsBySubs.Add(_calendarRepository.GetCalendarByCalendarId(cs.CalendarId));
            }
            var result = new List<Calendar>();
            foreach (Calendar c in calendarsBySubs)
            {
                foreach (Calendar cu in profileCalendars)
                {
                    if (calendarsBySubs.Contains(cu) == false)
                    {
                        if (result.Contains(cu)==false)
                            result.Add(cu);
                    }
                }
            }
            if (result.Count ==0 && profileCalendars.ToList().Count!=0)
            {
                check = "Sie abonnieren alle Kalendar des Benutzers";
            }
            if (result.Count == 0 && profileCalendars.ToList().Count == 0)
            {
                check = "Noch keine Kalender erstellt";
            }
            else
            {
                check = "Sie können noch folgende Kalender des Benutzers abonnieren:";
            }
            if (userId == Guid.Parse(HttpContext.GetOwinContext().Authentication.User.Identity.GetUserId()))
            {
                check = "Ihre Kalender";
            }
            ViewBag.CalendarList = result;
            ViewBag.FullCalendarList = profileCalendars;
            ViewBag.Check = check;
            User user = _userRepository.GetUserByID(userId);
            ViewBag.UserName = user.FirstName + " " + user.LastName;
            return View();
        }
    }
}