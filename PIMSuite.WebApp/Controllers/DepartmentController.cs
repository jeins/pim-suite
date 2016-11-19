﻿using PIMSuite.Persistence;
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

        public DepartmentController()
        {
            this.departmentRepository = new DepartmentRepository(new DataContext());
        }


        // GET: Department
        [AuthorizationFilter]
        public ActionResult Index()
        {
            

            return View(departmentRepository.GetDepartments());
        }

       
    }
}