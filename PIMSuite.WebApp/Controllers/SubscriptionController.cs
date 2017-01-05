using Microsoft.AspNet.Identity;
using PIMSuite.Persistence;
using PIMSuite.Persistence.Entities;
using PIMSuite.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace PIMSuite.WebApp.Controllers
{
    public class SubscriptionModel
    {
        public string FullName { get; set; }
        public List<Calendar> Subscriptions { get; set; }

        public IEnumerable<SubscriptionModel> subs {get; set;}
    }

    [Authorize]
    public class SubscriptionController : BaseController
    {
        // Constructors

        public SubscriptionController()
        {
            _dataContext = new DataContext();
            _calendarRepository = new CalendarRepository(_dataContext);
            _userRepository = new UserRepository(_dataContext);
            _subscriptionRepository = new Calendar_SubscriptionRepository(_dataContext);
        }

        // Fields

        private readonly DataContext _dataContext;
        private readonly ICalendarRepository _calendarRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICalendar_SubscriptionRepository _subscriptionRepository;

        // Methods
        // GET: Subscription

        public ActionResult Index()
        {
            var userId = Guid.Parse(HttpContext.GetOwinContext().Authentication.User.Identity.GetUserId());
            var subs = _subscriptionRepository.GetAllSubscriptionsByUserId(userId);
            var subUsers = new List<User>();
            var subCalendars = new List<Calendar>();

            foreach(Calendar_Subscription cs in subs)
            {
                if (subUsers.Contains(_userRepository.GetUserByID(_calendarRepository.GetUserByCalendarId(cs.CalendarId))) == false)
                {
                    subUsers.Add(_userRepository.GetUserByID(_calendarRepository.GetUserByCalendarId(cs.CalendarId)));
                }
                subCalendars.Add(_calendarRepository.GetCalendarByCalendarId(cs.CalendarId));
            }
            ViewBag.SubsUsers = subUsers;
            var subsByUser = new List<SubscriptionModel>();
            foreach(User u in subUsers)
            {
                var userFullName = u.FirstName + " " + u.LastName;
                var calendars = new List<Calendar>();
                foreach (Calendar c in subCalendars)
                {
                    if (u.UserId == c.OwnerId)
                    {
                        calendars.Add(c);
                    }
                }
                subsByUser.Add(new SubscriptionModel
                {
                    FullName = userFullName,
                    Subscriptions = calendars
                });
            }
            
            ViewBag.Result = subsByUser;
            
            return View();
        }
    }
}