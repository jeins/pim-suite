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
    public class DepartmentController : BaseController
    {
        private IDepartmentRepository departmentRepository;
        private DataContext _dataContext;

        public DepartmentController()
        {
            _dataContext = new DataContext();
        }

        // GET: Profile
        [AuthorizationFilter]
        public ViewResult Index(string sort, int? page, string searchString)
        {
            //TODO:: entities should be in english
            var departments = from u in _dataContext.Departments select u;
            var pageSize = 6;
            var pageNumber = (page ?? 1);

            if (!String.IsNullOrEmpty(searchString))
            {
                departments = SearchProcessor(departments, searchString);
            }

            departments = SortProcessor(departments, sort);

            return View(departments.ToPagedList(pageNumber, pageSize));
        }

        private IQueryable<Department> SearchProcessor(IQueryable<Department> department, string searchString)
        {
            ViewBag.SearchString = searchString;
            return department.Where(u =>
                u.Name.Contains(searchString)
            );
        }

        private IQueryable<Department> SortProcessor(IQueryable<Department> departments, string sort)
        {
            ViewBag.CurrentSortType = sort;
            switch (sort)
            {
                case "desc":
                    departments = departments.OrderByDescending(u => u.Name);
                    ViewBag.AvailableSortType = "asc";
                    break;
                default:
                    departments = departments.OrderBy(u => u.Name);
                    ViewBag.AvailableSortType = "desc";
                    break;
            }

            return departments;
        }
    }
}