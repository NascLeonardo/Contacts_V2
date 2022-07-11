using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Contacts_V2.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using Contacts_V2.Data;

namespace Contacts_V2.Controllers
{
    [Route("[controller]")]
    public class AuthController : Controller
    {
        private readonly ILogger<AuthController> _logger;

        public AuthController(ILogger<AuthController> logger)
        {
            _logger = logger;
        }
        [HttpGet("Index")]
        public IActionResult Index()
        {
            if (!HttpContext.Session.GetInt32("ID").HasValue)
            {
                return LocalRedirect("/");
            }
            return View();
        }


        [HttpGet("Login")]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost("Login")]
        public IActionResult Login([Bind] User user){
            ModelState.ClearValidationState("Id");
            ModelState.ClearValidationState("Name");            
            ModelState.ClearValidationState("ConfirmationPassword");
            if (!UserDAL.CheckEmailUsed(user.Email))
            {
                ModelState.AddModelError("Email", "This email don't match our records!");
            }
            int id = UserDAL.Login(user);
            if( id > 0){
                HttpContext.Session.SetInt32("ID",id);
                return LocalRedirectPermanent("/");
            }else if(id == -2){
                ModelState.AddModelError("Email","This email don't match our records");
            }else{
                ModelState.AddModelError("Password", "This password don't match our records!");
            }
            return View();
        }

        [HttpGet("Register")]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost("Register")]
        public IActionResult Register([Bind] User user)
        {
            if (user.Password != user.ConfirmationPassword)
            {
                ModelState.AddModelError("ConfirmationPassword", "Passwords don't match!");
            }
            if (user.Password == "" || user.Password == null)
            {
                ModelState.ClearValidationState("ConfirmationPassword");
            }
            if (UserDAL.CheckEmailUsed(user.Email))
            {
                ModelState.AddModelError("Email", "Email already in use!");
            }
            if (user.Email != null)
            {
                if (!Regex.IsMatch(user.Email.Trim(), @"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*@((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))\z"))
                {
                    ModelState.AddModelError("Email", "Is it even an email?");
                }
            }
            if (ModelState.IsValid)
            {
                if(UserDAL.Register(user)){
                    return LocalRedirectPermanent("/");
                }
            }
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet("Logout")]
        public IActionResult Logout(){
            HttpContext.Session.Clear();
            return LocalRedirectPermanent("/");
        }
    }
}