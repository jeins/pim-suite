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
            var calendar = _calendarRepository.GetCalendarByCalendarId(calendarId);

            ViewBag.CalendarName = calendar.Name;
            ViewBag.CalendarId = calendar.CalendarId;
            ViewBag.UserId = calendar.OwnerId.ToString();
            ViewBag.OwnerName= _userRepository.GetUserByID(calendar.OwnerId).FirstName+" "+_userRepository.GetUserByID(calendar.OwnerId).LastName;
            ViewBag.DisplayAll = User.Identity.GetUserId().Equals(calendar.OwnerId.ToString());
            
            var flag=_subscriptionRepository.SubscrptionContainsinUserList(calendarId, Guid.Parse(HttpContext.GetOwinContext().Authentication.User.Identity.GetUserId()));
            if (flag == true)
            {
                ViewBag.Flag = "nicht mehr folgen";
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
            
            var _sub = new Calendar_Subscription
            {
                CalendarId = calendarId,
                SubscriberId = Guid.Parse(HttpContext.GetOwinContext().Authentication.User.Identity.GetUserId())
            };

            _subscriptionRepository.Insert(_sub);
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
            ViewBag.CalendarList = _calendarRepository.GetAllCalendarsByUserId(userId);
            User user = _userRepository.GetUserByID(userId);
            ViewBag.UserName = user.FirstName + " " + user.LastName;
            return View();
        }

        

       
    }
}