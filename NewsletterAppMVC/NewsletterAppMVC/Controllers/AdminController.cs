using NewsletterAppMVC.Models;
using NewsletterAppMVC.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NewsletterAppMVC.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            using (NewsletterEntities1 db = new NewsletterEntities1())
            {
                //var signups = db.SignUps.Where(x => x.Removed == null).ToList(); //db.SignUps is all of the records in the database
                var signups = (from c in db.SignUps
                               where c.Removed == null
                               select c).ToList();
                var signupVms = new List<SignupVm>();
                foreach (var signup in signups)
                {
                    var signupVm = new SignupVm();
                    signupVm.Id = signup.Id;
                    signupVm.FirstName = signup.FirstName;
                    signupVm.LastName = signup.LastName;
                    signupVm.EmailAddress = signup.EmailAddress;
                    signupVms.Add(signupVm);

                }

                return View(signupVms);
            }
 
        }

        public ActionResult Unsubscribe(int Id)
        {
            //establish db context, which automatically creates a connection to the database
            using(NewsletterEntities1 db = new NewsletterEntities1())
            {
                //find the record that we want
                var signup = db.SignUps.Find(Id);
                //update the field Removed
                signup.Removed = DateTime.Now;
                //update the change to the database
                db.SaveChanges();
               
            }
            return RedirectToAction("Index");
        }
    }
}