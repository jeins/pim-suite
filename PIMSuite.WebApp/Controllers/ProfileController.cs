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
    public class ProfileController : Controller
    {
        private DataContext _dataContext;

        public ProfileController()
        {
            _dataContext = new DataContext();;
        }

        // GET: Profile
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

        private IQueryable<User> SearchProcessor(IQueryable<User> user, string searchString)
        {
            return user.Where(u => 
                u.Nachname.Contains(searchString) ||
                u.Vorname.Contains(searchString) ||
                u.Abteilung.Contains(searchString) ||
                u.Email.Contains(searchString)
            );
        }

        private IQueryable<User> SortProcessor(IQueryable<User> users, string sort)
        {
            ViewBag.CurrentSortType = sort;
            switch (sort)
            {
                case "desc":
                    users = users.OrderByDescending(u => u.Nachname);
                    ViewBag.AvailableSortType = "asc";
                    break;
                default:
                    users = users.OrderBy(u => u.Nachname);
                    ViewBag.AvailableSortType = "desc";
                    break;
            }

            return users;
        }
    }
}