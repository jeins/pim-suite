using PagedList;
using PIMSuite.Persistence;
using PIMSuite.Persistence.Entities;
using PIMSuite.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PIMSuite.WebApp.Controllers
{
    public class LocationController : BaseController
    {
        private ILocationRepository locationRepository;
        private DataContext _dataContext;

        public LocationController()
        {
            _dataContext = new DataContext();
        }

        // GET: Profile
        [AuthorizationFilter]
        public ViewResult Index(string sort, int? page, string searchString)
        {
            //TODO:: entities should be in english
            var locations = from u in _dataContext.Locations select u;
            var pageSize = 6;
            var pageNumber = (page ?? 1);

            if (!String.IsNullOrEmpty(searchString))
            {
                locations = SearchProcessor(locations, searchString);
            }

            locations = SortProcessor(locations, sort);

            return View(locations.ToPagedList(pageNumber, pageSize));
        }

        private IQueryable<Location> SearchProcessor(IQueryable<Location> location, string searchString)
        {
            ViewBag.SearchString = searchString;
            return location.Where(u =>
                u.Name.Contains(searchString)
            );
        }

        private IQueryable<Location> SortProcessor(IQueryable<Location> locations, string sort)
        {
            ViewBag.CurrentSortType = sort;
            switch (sort)
            {
                case "desc":
                    locations = locations.OrderByDescending(u => u.Name);
                    ViewBag.AvailableSortType = "asc";
                    break;
                default:
                    locations = locations.OrderBy(u => u.Name);
                    ViewBag.AvailableSortType = "desc";
                    break;
            }

            return locations;
        }
    }
}