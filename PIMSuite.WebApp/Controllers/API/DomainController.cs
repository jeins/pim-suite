using PIMSuite.Persistence;
using PIMSuite.Persistence.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System.Security.Claims;
using System.Web;
using PIMSuite.Utilities.Auth;
using PIMSuite.Persistence.Repositories;

namespace PIMSuite.WebApp.Controllers.API
{
    public class DomainController : ApiController
    {
        private readonly DataContext _dataContext;
        private readonly IDomainRepository _domainRepository;

        public DomainController()
        {
            _dataContext = new DataContext();
            _domainRepository = new DomainRepository(_dataContext);
        }

        [DataContract]
        public sealed class DomainModel
        {
            [DataMember(IsRequired = true)]
            [Required]
            public string NewDomain { get; set; }
        }

        [HttpPost]
        public HttpResponseMessage Add(DomainModel model)
        {
            if (model != null && ModelState.IsValid)
            {
                using (DataContext context = new DataContext())
                {
                    _domainRepository.InsertDomain(model.NewDomain);
                    _domainRepository.Save();
                    return Request.CreateResponse(HttpStatusCode.Accepted, true);
                }
            }
            return Request.CreateResponse(HttpStatusCode.Forbidden, "Wrong Data!");
        }

    }
}
