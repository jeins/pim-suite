using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PIMSuite.Persistence;
using PIMSuite.Persistence.Entities;
using PIMSuite.Persistence.Repositories;

namespace PIMSuite.WebApp.Controllers
{
    public class ChatController : BaseController
    {

        private DataContext _dataContext;
        private IMessageRepository _messageRepository;

        public ChatController()
        {
            _dataContext = new DataContext();
            _messageRepository = new MessageRepository(_dataContext);
        }

        // GET: Chat
        public ActionResult Index(string toUserId)
        {
            User currentUser = ViewBag.User; 
            var users = _dataContext.Users.Where(u => u.UserId != currentUser.UserId);

            ViewBag.ChatToUserId = null;
            ViewBag.ChatHistories = new string[] {};

            if (toUserId != null)
            {
                ViewBag.ChatToUserId = toUserId;
                ViewBag.ChatHistories = _messageRepository.GetMessageHistories(currentUser.UserId, new Guid(toUserId));

            }

            return View(users);
        }
    }
}