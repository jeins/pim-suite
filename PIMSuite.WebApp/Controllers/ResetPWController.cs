using PIMSuite.Persistence;
using PIMSuite.Persistence.Entities;
using PIMSuite.Persistence.Repositories;
using PIMSuite.Persistence.Validators;
using PIMSuite.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System.Security.Claims;
using PIMSuite.Utilities.Auth;
using System.Net;
using System.Web.Mvc;
using System.Text;
using System.Security.Cryptography;
using PIMSuite.Utilities.Mail;

namespace PIMSuite.WebApp.Controllers
{
    public class ResetPWController : BaseController
    {
        public const int passwordTokenSize = 24;

        public ResetPWController()
        {

        }

        [HttpGet]
        public ActionResult ResetPassword(String email)
        {
            User user;
            using (DataContext context = new DataContext())
            {
                user = context.Users.SingleOrDefault(p => p.Email == email);
                if (user == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden, "Wrong credentials!");
                }
                else 
                {
                    var hh = new HashHelper();                    
                    var tempPassword = TokenGenerator.GenerateValidationToken(passwordTokenSize);
                    user.Password = hh.Hash(tempPassword);
                    userRepository.UpdateUser(user);                                
                    ViewBag.Message = "A new temporary password has been sent to your mail - address"; 
                    EmailHelper.SendMail("smtp.gmail.com", "PIMSuiteASP@gmail.com", "noreplyASP", user.Email, "Your password reset request for the PIM Suite", "Your new temporary password: " + tempPassword);
                    Response.Redirect("/?infoMessage=A new temporary password has been sent to your mail-address, please check your mails and use the new password!");
                }
            }
            return View();
        }
    }
}