using NewsletterAppMVC.Models;
using NewsletterAppMVC.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NewsletterAppMVC.Controllers
{
    public class HomeController : Controller
    {
        //private readonly string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;
                //Initial Catalog=Newsletter;Integrated Security=True;Connect Timeout=30;
                //Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;
                //MultiSubnetFailover=False";

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignUp(string firstName, string lastName, string emailAddress)
        {
            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName) || string.IsNullOrEmpty(emailAddress))
            {
                return View("~/Views/Shared/Error.cshtml");
            }
            else
            {
                using(NewsletterEntities1 db = new NewsletterEntities1())
                {
                    var signup = new SignUp();
                    signup.FirstName = firstName;
                    signup.LastName = lastName;
                    signup.EmailAddress = emailAddress;

                    db.SignUps.Add(signup);
                    db.SaveChanges();
                }

                ////create the query string which here is an insert statement
                //string queryString = @"INSERT INTO SignUps (FirstName, LastName, EmailAddress) VALUES (@FirstName, @LastName, @EmailAddress)";

                //using (SqlConnection connection = new SqlConnection(connectionString))
                //{
                //    SqlCommand command = new SqlCommand(queryString, connection);
                //    command.Parameters.Add("@FirstName", SqlDbType.VarChar);
                //    command.Parameters.Add("@LastName", SqlDbType.VarChar);
                //    command.Parameters.Add("@EmailAddress", SqlDbType.VarChar);

                //    command.Parameters["@FirstName"].Value = firstName;
                //    command.Parameters["@LastName"].Value = lastName;
                //    command.Parameters["@EmailAddress"].Value = emailAddress;

                //    connection.Open();
                //    command.ExecuteNonQuery();
                //    connection.Close();

                //}


                return View("Success");
            }
        }

        //public ActionResult Admin()
        //{
        //    using (NewsletterEntities1 db = new NewsletterEntities1())
        //    {
        //        var signups = db.SignUps; //db.SignUps is all of the records in the database
        //        var signupVms = new List<SignupVm>();
        //        foreach (var signup in signups)
        //        {
        //            var signupVm = new SignupVm();
        //            signupVm.FirstName = signup.FirstName;
        //            signupVm.LastName = signup.LastName;
        //            signupVm.EmailAddress = signup.EmailAddress;
        //            signupVms.Add(signupVm);

        //        }

        //        return View(signupVms);
        //    }
        //}
            //string queryString = @"SELECT Id, FirstName, LastName, EmailAddress, SocialSecurityNumber from Signups";
            //List<NewsletterSignUp> signUps = new List<NewsletterSignUp>();

            //using (SqlConnection connection = new SqlConnection(connectionString))
            //{
            //    SqlCommand command = new SqlCommand(queryString, connection);
            //    connection.Open();
            //    SqlDataReader reader = command.ExecuteReader();

            //    while (reader.Read())
            //    {
            //        var signup = new NewsletterSignUp();
            //        signup.Id = Convert.ToInt32(reader["Id"]);
            //        signup.FirstName = reader["FirstName"].ToString();
            //        signup.LastName = reader["LastName"].ToString();
            //        signup.EmailAddress = reader["EmailAddress"].ToString();
            //        signup.SocialSecurityNumber = reader["SocialSecurityNumber"].ToString();
            //        signUps.Add(signup);

            //    }



                
            //}
        //    var signupVms = new List<SignupVm>();
        //    foreach (var signup in signups)
        //    {
        //        var signupVm = new SignupVm();
        //        signupVm.FirstName = signup.FirstName;
        //        signupVm.LastName = signup.LastName;
        //        signupVm.EmailAddress = signup.EmailAddress;
        //        signupVms.Add(signupVm);

        //    }

        //    return View(signupVms);
        //}
    }

    //    public ActionResult About()
    //    {
    //        ViewBag.Message = "Your application description page.";

    //        return View();
    //    }

    //    public ActionResult Contact()
    //    {
    //        ViewBag.Message = "Your contact page.";

    //        return View();
    //    }
    //}
}