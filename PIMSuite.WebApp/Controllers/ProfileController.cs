using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PIMSuite.Persistence;
using PIMSuite.Persistence.Entities;
using PagedList;

namespace PIMSuite.WebApp.Controllers
{
    public class ProfileController : BaseController
    {
        private DataContext _dataContext;

        public ProfileController()
        {
            _dataContext = new DataContext();;
        }

        // GET: Profile
        [AuthorizationFilter]
        public ViewResult Index(string sort, int? page, string searchString)
        {
            //TODO:: entities should be in english
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

            //TODO:: check if userId equals userId in the cookie, then enable edit profile button
            ViewBag.EnableEditProfile = CheckCurrentUser(userId);

            return View(user);
        }

        private IQueryable<User> SearchProcessor(IQueryable<User> user, string searchString)
        {
            ViewBag.SearchString = searchString;
            return user.Where(u => 
                u.Lastname.Contains(searchString) ||
                u.Firstname.Contains(searchString) ||
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
                    users = users.OrderByDescending(u => u.Lastname);
                    ViewBag.AvailableSortType = "asc";
                    break;
                default:
                    users = users.OrderBy(u => u.Lastname);
                    ViewBag.AvailableSortType = "desc";
                    break;
            }

            return users;
        }

        private bool CheckCurrentUser(string userId)
        {
            return false;
        }
    }
}