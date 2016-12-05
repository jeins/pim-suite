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
        public const int passwordTokenSize = 12;

        public ResetPWController()
        {
            this.userRepository = new UserRepository(new DataContext());
        }

        public ActionResult ResetPW()
        {
            if (HttpContext.GetOwinContext().Authentication.User.Identity.IsAuthenticated)
            {
                Response.Redirect("/Dashboard/");
            }
            return View();
        }

        [HttpPost]
        public ActionResult ResetPW(User userEmail)
        {
            User user;
            using (DataContext context = new DataContext())
            {
                user = context.Users.SingleOrDefault(p => p.Email == userEmail.Email);
                if (user == null)
                {
                    //return new HttpStatusCodeResult(HttpStatusCode.Forbidden, "Wrong credentials!   Test");
                    ViewBag.Message = "No account was found with this email";
                }
                else 
                {
                    var hh = new HashHelper();                    
                    var tempPassword = TokenGenerator.GenerateValidationToken(passwordTokenSize);
                    user.Password = hh.Hash(tempPassword);
                    userRepository.UpdateUser(user);
                    userRepository.Save();
                    ViewBag.Message = "A new temporary password has been sent to your mail - address"; 
                    EmailHelper.SendMail("smtp.gmail.com", "PIMSuiteASP@gmail.com", "noreplyASP", user.Email, "Your password reset request for the PIM Suite", "Your new temporary password: " + tempPassword);
                    Response.Redirect("/?infoMessage=A new temporary password has been sent to your mail-address, please check your mails and use the new password!");
                }
            }
            return View();
        }
    }
}