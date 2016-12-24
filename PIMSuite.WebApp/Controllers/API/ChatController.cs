
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Web.Http;
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
        public string UserId { get; set; }

        public bool IsOwner { get; set; }
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
                    UserId = new Guid(model.UserId),
                    IsOwner = model.IsOwner
                };

                _dataContext.ChatGroups.Add(chatGroup);
                _dataContext.SaveChanges();
                
                return Request.CreateResponse(HttpStatusCode.Accepted, chatGroup.GroupId);
            }

            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "invalid data");
        }

        public HttpResponseMessage GetUserNotInChatGroup(string groupId)
        {
            var userList = _dataContext.Users.Select(u => new {u.UserId, u.FirstName, u.LastName}).ToList();
            var userFromGroup = _dataContext.ChatGroups.Where(g => g.GroupId.Equals(new Guid(groupId))).Select(g => g.UserId).ToList();

            userList.RemoveAll(u => userFromGroup.Contains(u.UserId));

            return Request.CreateResponse(HttpStatusCode.OK, userList, Configuration.Formatters.JsonFormatter);
        }
    }
}