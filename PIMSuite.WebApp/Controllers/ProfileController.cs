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
        public ViewResult Index(string sort, int? page)
        {
            //TODO:: entities should be in english
            var users = from u in _dataContext.Users
                        select u;
            int pageSize = 6;
            int pageNumber = (page ?? 1);

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
            
            return View(users.ToPagedList(pageNumber, pageSize));
        }
    }
}