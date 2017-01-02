using Microsoft.AspNet.Identity;
using PIMSuite.Persistence;
using PIMSuite.Persistence.Entities;
using PIMSuite.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace PIMSuite.WebApp.Controllers.API
{
   

    public class SubscriptionController : ApiController
    {
        // GET: Subscription
        // Constructors

        public SubscriptionController()
        {
            _dataContext = new DataContext();
            _subscriptionRepository = new Calendar_SubscriptionRepository(_dataContext);
            
        }

        // Fields

        private readonly DataContext _dataContext;
        private readonly ICalendar_SubscriptionRepository  _subscriptionRepository;
        

        [System.Web.Mvc.HttpPost]
        public void CreateSubscription(int CalendarId)
        {
           
            var _sub = new Calendar_Subscription {
                CalendarId = CalendarId,
                SubscriberId = Guid.Parse(HttpContext.Current.GetOwinContext().Authentication.User.Identity.GetUserId())
        };
            _subscriptionRepository.Insert(_sub);
            _subscriptionRepository.Save();

            
        }

       


    }
}