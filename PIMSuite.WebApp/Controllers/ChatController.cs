using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using PIMSuite.Persistence;
using PIMSuite.Persistence.Entities;
using PIMSuite.Persistence.Repositories;

namespace PIMSuite.WebApp.Controllers
{
    [Authorize]
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
           
            ViewBag.ChatToUserId = null;
            ViewBag.ChatHistories = new string[] {};

            if (toUserId != null)
            {
                ViewBag.ChatToUserId = toUserId;
                ViewBag.ChatHistories = _messageRepository.GetMessageHistories(currentUser.UserId, new Guid(toUserId));
            }

            ViewBag.UserList = GetUserWithUnReadMessage();

            return View();
        }

        private IEnumerable<string[]> GetUserWithUnReadMessage()
        {
            User currentUser = ViewBag.User;
            var users = _dataContext.Users.Where(u => u.UserId != currentUser.UserId);
            var usersWithUnReadMessages = new List<string[]>();

            foreach (var user in users)
            {
                var totalUnReadMessage = _dataContext.Messages.Count(m => m.SenderUserId.Equals(user.UserId) && m.ReceiverUserId.Equals(currentUser.UserId) && m.IsRead == false);

                usersWithUnReadMessages.Add(new[]
                {
                    user.UserId.ToString(),
                    user.LastName,
                    totalUnReadMessage.ToString()
                });
            }

            return usersWithUnReadMessages;
        }
    }
}