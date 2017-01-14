
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Web.Http;
using Newtonsoft.Json.Linq;
using PIMSuite.Persistence;
using PIMSuite.Persistence.Entities;

namespace PIMSuite.WebApp.Controllers.API
{
    [DataContract]
    public class CreateChatGroupModel
    {
        [DataMember(IsRequired = true)]
        [Required]
        public string GroupName { get; set; }

        [DataMember(IsRequired = true)]
        [Required]
        public string OwnerId { get; set; }
    }

    [DataContract]
    public class AddUserToChatGroupModel
    {
        [DataMember(IsRequired = true)]
        [Required]
        public string GroupId { get; set; }

        [DataMember(IsRequired = true)]
        [Required]
        public string UsersId { get; set; }
    }

    public class ChatController : ApiController
    {
        private readonly DataContext _dataContext;

        public ChatController()
        {
            _dataContext = new DataContext();
        }
        
        [HttpPost]
        public HttpResponseMessage CreateNewGroup(CreateChatGroupModel model)
        {
            if (model != null && ModelState.IsValid)
            {
                var chatGroup = new ChatGroup
                {
                    GroupName = model.GroupName,
                    OwnerId = new Guid(model.OwnerId)
                };

                var userChatGroup = new UserChatGroup
                {
                    GroupId = new Guid(chatGroup.GroupId.ToString()),
                    UserId = new Guid(model.OwnerId)
                };

                _dataContext.ChatGroups.Add(chatGroup);
                _dataContext.UserChatGroups.Add(userChatGroup);
                _dataContext.SaveChanges();
                
                return Request.CreateResponse(HttpStatusCode.Accepted, chatGroup.GroupId);
            }

            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "invalid data");
        }

        public HttpResponseMessage GetUserNotInChatGroup(string groupId)
        {
            var userList = _dataContext.Users.ToList();
            var userListFromGroup = _dataContext.UserChatGroups.Where(g => g.GroupId.Equals(new Guid(groupId))).ToList();
            
            foreach (var userFromGroup in userListFromGroup)
            {
                userList.RemoveAll(u => u.UserId.Equals(userFromGroup.UserId));
            }

            return Request.CreateResponse(HttpStatusCode.OK, userList, Configuration.Formatters.JsonFormatter);
        }

        [HttpPost]
        public HttpResponseMessage AddUserToChatGroup(AddUserToChatGroupModel model)
        {
            if (model != null && ModelState.IsValid)
            {
                var users = model.UsersId.Split('_');
                foreach (var userId in users)
                {
                    var userChatGroup = new UserChatGroup
                    {
                        GroupId = new Guid(model.GroupId),
                        UserId = new Guid(userId)
                    };

                    _dataContext.UserChatGroups.Add(userChatGroup);
                }

                _dataContext.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.Accepted, "");
            }

            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "invalid data");
        }

        public HttpResponseMessage GetUserInChatGroup(string groupId)
        {
            var userListFromGroup = _dataContext.UserChatGroups.Where(g => g.GroupId.Equals(new Guid(groupId))).ToList();

            return Request.CreateResponse(HttpStatusCode.OK, userListFromGroup, Configuration.Formatters.JsonFormatter);
        }
    }
}