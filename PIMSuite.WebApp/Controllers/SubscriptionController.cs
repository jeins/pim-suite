using Microsoft.AspNet.Identity;
using PIMSuite.Persistence;
using PIMSuite.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PIMSuite.WebApp.Controllers
{
    [Authorize]
    public class SubscriptionController : BaseController
    {
        // Constructors

        public SubscriptionController()
        {
            _dataContext = new DataContext();
            _calendarRepository = new CalendarRepository(_dataContext);
            _userRepository = new UserRepository(_dataContext);
        }

        // Fields

        private readonly DataContext _dataContext;
        private readonly ICalendarRepository _calendarRepository;
        private readonly IUserRepository _userRepository;

        // Methods
        // GET: Subscription

        public ActionResult Index()
        {

            return View();
        }

        public ActionResult CreateFormSubscription(string searchString)
        {
            var users = _userRepository.GetUsers();
            users = users.Where(u => u.UserId != Guid.Parse(User.Identity.GetUserId())).ToList();

            if (!String.IsNullOrEmpty(searchString))
            {
                users = users.Where(u => u.LastName.Contains(searchString)
                                       || u.FirstName.Contains(searchString));
            }
            ViewData["users"] = users;

            return View(users);
        }

        public ActionResult GetCalendars(Guid UserId)
        {

            ViewBag.UserCalendars = "Der Benutzer hat folgende Kalender";
            var calendars = _calendarRepository.GetAllUserCalendars(UserId);

            ViewData["calendars"] = calendars;
            return PartialView("searchResults", calendars);

        }
        //Abonnement 
        public ActionResult CreateSubscription()
        {
            //todo
            return View();
        }


    }
}