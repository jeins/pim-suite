﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PimSuite.Models;

namespace PimSuite.Controllers
{
    public class DashboardController : Controller
    {
        private PimSuiteDatabaseEntities _db;
        public DashboardController()
        {
            _db = new PimSuiteDatabaseEntities();
        }
        // GET: Dashboard
        public ActionResult Index()
        {
            var allUsers = _db.Users.ToList();
            var b = "";
            return View();
        }
    }
}