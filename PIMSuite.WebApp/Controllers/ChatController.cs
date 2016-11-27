using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PIMSuite.Persistence;
using PIMSuite.Persistence.Entities;

namespace PIMSuite.WebApp.Controllers
{
    public class ChatController : BaseController
    {

        private DataContext _dataContext;

        public ChatController()
        {
            _dataContext = new DataContext();
        }

        // GET: Chat
        public ActionResult Index()
        {
            User currentUser = ViewBag.User; 
            var users = _dataContext.Users.Where(u => u.UserId != currentUser.UserId);

            //ViewBag.UserList = users.ToList();

            return View(users);
        }
    }
}