using Microsoft.AspNet.Identity;
using PIMSuite.Persistence;
using PIMSuite.Persistence.Entities;
using PIMSuite.Persistence.Repositories;
using PIMSuite.Utilities.Auth;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PIMSuite.WebApp.Controllers
{
    public class SubscriptionModel
    {
        public string FullName { get; set; }
        public List<Calendar> Subscriptions { get; set; }

        IEnumerable <SubscriptionModel> subs {get; set;}
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
            var _subs = _subscriptionRepository.GetAllSubscriptionsByUserId(Guid.Parse(HttpContext.GetOwinContext().Authentication.User.Identity.GetUserId()));
            var _subUsers = new List<User>();
            var _subCalendars = new List<Calendar>();

            foreach(Calendar_Subscription cs in _subs)
            {
                if (_subUsers.Contains(_userRepository.GetUserByID
                    (_calendarRepository.GetUserByCalendarId
                    (cs.CalendarId))) == false)
                {
                    _subUsers.Add(_userRepository.GetUserByID(_calendarRepository.GetUserByCalendarId(cs.CalendarId)));
                }
                _subCalendars.Add(_calendarRepository.GetCalendarByCalendarId(cs.CalendarId));
            }
            ViewBag.SubsUsers = _subUsers;
            var _subsByUser = new List<SubscriptionModel>();
            foreach(User u in _subUsers)
            {
                var _userFullName = u.FirstName + " " + u.LastName;
                var _calendars = new List<Calendar>();
                foreach (Calendar c in _subCalendars)
                {
                    if (u.UserId == c.OwnerId)
                    {
                        _calendars.Add(c);
                    }
                    
                }
                _subsByUser.Add(new SubscriptionModel { FullName=_userFullName, Subscriptions=_calendars});
            }
            
            ViewBag.Result = _subsByUser;

           

            return View();
        }

        


    }
}