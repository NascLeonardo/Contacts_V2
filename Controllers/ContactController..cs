using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Contacts_V2.Data;
using Contacts_V2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Contacts_V2.Controllers
{
    [Route("{controller}")]
    public class ContactController : Controller
    {

        private readonly ILogger<ContactController> _logger;

        public ContactController(ILogger<ContactController> logger)
        {
            _logger = logger;
        }
        [HttpGet]
        [HttpGet("Index")]
        public ActionResult Index()
        {
            if (!HttpContext.Session.GetInt32("ID").HasValue)
            {
                return LocalRedirect("/");
            }
            return View(ContactDAL.GetAll(HttpContext.Session.GetInt32("ID").Value).OrderBy(X => !X.isFavorite).ThenBy(X => X.Firstname).ThenBy(X => X.Surname));
        }

        [HttpGet("Search")]
        public ActionResult Index(string Sentence)
        {
            if (!HttpContext.Session.GetInt32("ID").HasValue)
            {
                return LocalRedirect("/");
            }
            if (Sentence != null)
            {
                return View(ContactDAL.Search(Sentence, HttpContext.Session.GetInt32("ID").Value).OrderBy(X => !X.isFavorite).ThenBy(X => X.Firstname).ThenBy(X => X.Surname));
            }
            else
            {
                return RedirectToAction("Index");

            }
        }

        [HttpGet("Create")]
        public IActionResult Create()
        {
            if (!HttpContext.Session.GetInt32("ID").HasValue)
            {
                return LocalRedirect("/");
            }
            return View();
        }

        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind] Contact contact)
        {
            if (!HttpContext.Session.GetInt32("ID").HasValue)
            {
                return LocalRedirect("/");
            }
            ModelState.ClearValidationState("User");
            contact.UserId = HttpContext.Session.GetInt32("ID").Value;
            ModelState.MarkFieldValid("User");

            if (ModelState.IsValid)
            {
                if (ContactDAL.Create(contact))
                {
                    return RedirectToAction("Index");

                }
            }
            return View(contact);
        }

        [HttpGet("Favorite/{id}")]
        public IActionResult Favorite(int id)
        {
            if (!HttpContext.Session.GetInt32("ID").HasValue)
            {
                return LocalRedirect("/");
            }
            if (ContactDAL.ChangeFavoriteStatus(id, HttpContext.Session.GetInt32("ID").Value))
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        [HttpGet("Delete/{id}")]
        public IActionResult Delete(int id)
        {
            if (!HttpContext.Session.GetInt32("ID").HasValue)
            {
                return LocalRedirect("/");
            }
            if (ContactDAL.DeleteById(id, HttpContext.Session.GetInt32("ID").Value))
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
        
        [HttpGet("Edit/{id}")]
        public IActionResult Edit(int id)
        {
            if (!HttpContext.Session.GetInt32("ID").HasValue)
            {
                return LocalRedirect("/");
            }
            Contact contact = ContactDAL.GetById(id);
            if (contact != null && contact.UserId == HttpContext.Session.GetInt32("ID"))
            {
                return View(contact);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind] Contact contact)
        {
            if (!HttpContext.Session.GetInt32("ID").HasValue)
            {
                return LocalRedirect("/");
            }
            
            ModelState.ClearValidationState("User");
            contact.UserId = HttpContext.Session.GetInt32("ID").Value;
            ModelState.MarkFieldValid("User");
            var thiss = ModelState.ValidationState;
            if (ModelState.IsValid)
            {
                if (ContactDAL.Edit(contact, HttpContext.Session.GetInt32("ID").Value))
                {
                    return RedirectToAction("Index");
                }
            }

            return View(contact);

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [HttpGet("/Error")]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}