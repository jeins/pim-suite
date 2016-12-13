using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PIMSuite.Persistence;
using PIMSuite.Persistence.Entities;
using PagedList;
using PIMSuite.Utilities.Auth;
using Microsoft.AspNet.SignalR;
using PIMSuite.Persistence.Repositories;
using PIMSuite.WebApp.SignalRHub;

namespace PIMSuite.WebApp.Controllers
{
    public class ProfileController : BaseController
    {
        private DataContext _dataContext;

        public ProfileController()
        {
            _dataContext = new DataContext();
        }

        // GET: Profile
        [AuthorizationFilter]
        public ViewResult Index(string sort, int? page, string searchString)
        {
            var users = from u in _dataContext.Users select u;
            var pageSize = 6;
            var pageNumber = (page ?? 1);

            if (!String.IsNullOrEmpty(searchString))
            {
                users = SearchProcessor(users, searchString);
            }

            users = SortProcessor(users, sort);

            return View(users.ToPagedList(pageNumber, pageSize));
        }

        // GET: Profile/Show
        [AuthorizationFilter]
        public ActionResult Show(string userId)
        {
            Guid guid;

            if (String.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out guid))
            {
                return RedirectToAction("Index", "Profile");
            }
            
            var user = _dataContext.Users.Find(guid);

            if (user == null)
            {
                return RedirectToAction("Index", "Profile");
            }
            
            ViewBag.EnableEditProfile = CheckCurrentUser(guid);
            ViewBag.CurrentUser = user;

            return View(user);
        }

        [AuthorizationFilter]
        public ActionResult Edit(string userId)
        {
            ViewBag.Departments = new SelectList(new DataContext().Departments, "Name", "Name");
            ViewBag.Locations = new SelectList(new DataContext().Locations, "Name", "Name");
            Guid guid;

            if (String.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out guid))
            {
                return RedirectToAction("Index", "Profile");
            }

            var user = _dataContext.Users.Find(guid);

            if (user == null || !CheckCurrentUser(guid))
            {
                return RedirectToAction("Index", "Profile");
            }

            user.Password = null;
            return View(user);
        }

        [AuthorizationFilter]
        [HttpPost]
        public ActionResult Edit(User user)
        {
            ViewBag.Departments = new SelectList(new DataContext().Departments, "Name", "Name");
            ViewBag.Locations = new SelectList(new DataContext().Locations, "Name", "Name");
            var username = HttpContext.GetOwinContext().Authentication.User.Identity.Name;
            var edituser = userRepository.GetUserByUsername(username);
            Guid guid=edituser.UserId;
            var hashhelper = new HashHelper();

            user.UserId = guid;

            if (user == null || edituser == null || !CheckCurrentUser(guid))
            {
                return RedirectToAction("Index", "Profile");
            }

            if (user.Password == null || user.Password.Equals(""))
            {
                user.Password = edituser.Password;
            }
            else
            {
                user.Password = hashhelper.Hash(user.Password);
            }
            userRepository.UpdateUser(user);
            userRepository.Save();

            if (!user.Username.Equals(username))
            {
                var hub = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
                IConnectionRepository connectionRepository = new ConnectionRepository(new DataContext());
                connectionRepository.RemoveUser(guid, null);
                hub.Clients.All.onUserDisconnected(guid);

                HttpContext.GetOwinContext().Authentication.SignOut();
                return RedirectToAction("Index", "Home", new { successMessage = "Your Username has been successfully updated, please login again!" });
            }
            else
            {
                return RedirectToAction("Show", "Profile", new { userId = guid.ToString() });
            }
        }

        private IQueryable<User> SearchProcessor(IQueryable<User> user, string searchString)
        {
            ViewBag.SearchString = searchString;
            return user.Where(u => 
                u.LastName.Contains(searchString) ||
                u.FirstName.Contains(searchString) ||
                u.DepartmentName.Contains(searchString) ||
                u.Email.Contains(searchString)
            );
        }

        private IQueryable<User> SortProcessor(IQueryable<User> users, string sort)
        {
            ViewBag.CurrentSortType = sort;
            switch (sort)
            {
                case "desc":
                    users = users.OrderByDescending(u => u.LastName);
                    ViewBag.AvailableSortType = "asc";
                    break;
                default:
                    users = users.OrderBy(u => u.LastName);
                    ViewBag.AvailableSortType = "desc";
                    break;
            }

            return users;
        }

        private bool CheckCurrentUser(Guid userId)
        {
            User user = ViewBag.User;
            if (user != null && user.UserId.Equals(userId))
            {
                return true;
            }
            return false;
        }
    }
}