using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using KnolSecurity;
using GimpySoftwareNewTemplate.Models;
using System.Net.Mail;

namespace GimpySoftwareNewTemplate.Controllers
{
    public class AccountController : Controller
    {
        GimpyDataEntities GSEntity = new GimpyDataEntities();

        string pass = "6sm..y..ms6";
        [HttpGet]

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }


        public ActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            string encryppass = GSEntity.LoginCheckEncrypt(username).SingleOrDefault();
            if (encryppass != null)
            {
                string s = Crypter.aesDecrypt(encryppass);
                if (password.Equals(Crypter.aesDecrypt(encryppass)))
                {
                    Session["email"] = Crypter.aesEncrypt(username.ToString());
                    Session["UserInfo"] = Crypter.aesEncrypt(username);
                    var userinfo = GSEntity.GetUserIDByEmail(username).ToList();

                    foreach (var item in userinfo)
                    {
                        Session["userid"] = item.Id;
                        ViewBag.username = item.FirstName + " " + item.LastName;
                    }
                    ViewBag.LoginError = null;

                    return RedirectToAction("Dashboard", "AdminPanel");
                }
                {
                    ViewBag.LoginError = "Wrong credentials";
                }
            }
            else
            {
                ViewBag.LoginError = "Wrong credentials";
            }

            return View();
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(string email,  string password,string country,string fname,string lname,string bandartist,string company)
        {
            
                GSEntity.RegisterUser(email, Crypter.aesEncrypt(password), lname, fname, country, bandartist, company);
                int? userid = GSEntity.getcurrentuserid().First();
                if (userid != null)
                {
                    var result = GSEntity.getallgenres().ToList();
                    foreach (var item in result)
                    {
                        GSEntity.insertgenrebyuser(item, userid);
                    }
                }
            return RedirectToAction("Login", "Account");

        }

        [HttpGet]
        public ActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ResetPassword(string email)
        {
            if (email != null)
            {
                var result = GSEntity.checkemail(email).ToList();
                if (result.Count() > 0)
                {
                    DateTime expiretime = DateTime.Now.AddHours(2);
                    string rndmno = SendMail("resetlink", email, "http://" + Request.Url.Authority + "/Account/Sendpassword?recoverid=");
                    GSEntity.InsertIntoResetpassword(email, rndmno, expiretime);
                    ViewBag.message = "Reset password link sent to " + email;
                }
                else
                {
                    ViewBag.message = "This email does not exist";
                }
            }
            return View();
        }

        private string SendMail(string sendtype, string email, string data)
        {
            string randomno = Helper.GetRandomNumeric(10);
            string randomNo = Helper.EncryptData(randomno);
            MailMessage MMsg = new MailMessage();
            //SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            //MMsg.From = new MailAddress("manpreet6822@gmail.com");
            //MMsg.To.Add("manpreet6822@gmail.com");
            SmtpClient smtp = new SmtpClient("smtp.mail.yahoo.com", 587);
            MMsg.From = new MailAddress("manindersingh0640@yahoo.in");
            MMsg.To.Add("manindersingh0640@yahoo.in");
            MMsg.To.Add(email);
            if (sendtype == "resetlink")
            {
                MMsg.Subject = "Reset password";
                MMsg.Body = data + randomNo;
            }
            else
            {
                MMsg.Subject = "Your Credentials";
                MMsg.Body = data;
            }
            MMsg.Priority = MailPriority.High;
            //MMsg.IsBodyHtml = true;
            smtp.UseDefaultCredentials = false;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            //smtp.Credentials = new System.Net.NetworkCredential("manpreet6822@gmail.com", pass);
            smtp.Credentials = new System.Net.NetworkCredential("manindersingh0640@yahoo.in", pass);
            smtp.EnableSsl = true;
            smtp.Send(MMsg);
            return randomno;
        }

        public ActionResult Sendpassword(string recoverid)
        {
            if (recoverid != null)
            {
                string email = "", credencials = "";
                DateTime exptm = DateTime.Now;
                if (recoverid != null)
                {
                    recoverid = Helper.DecryptData(recoverid);
                    var result = GSEntity.CheckResetpassword(recoverid).ToList();

                    if (result.Count > 0)
                    {
                        foreach (var item in result)
                        {
                            exptm = Convert.ToDateTime(item.expiretime);
                            email = item.email;
                        }

                        if (exptm > DateTime.Now)
                        {
                            var record = GSEntity.Getpassword(email).ToList();
                            if (record.Count() > 0)
                            {
                                foreach (var item in record)
                                {
                                    credencials = "Your Password = " + Crypter.aesDecrypt(item.Password);
                                }

                                SendMail("credencials", email, credencials);
                                GSEntity.DeleteFromResetpassword(recoverid);
                                ViewBag.message = "Credentials sent to your account";
                                return View();
                            }
                        }
                    }
                }
            }
            return RedirectToAction("Login", "Account");
        }

        public ActionResult Logout()
        {
            Session["UserInfo"] = null;
            return RedirectToAction("Login", "Account");

        }

        [HttpGet]
        public JsonResult CheckEmailExists(string emailid)
        {
            var result = GSEntity.CheckEmailExists(emailid).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ResetPasswords(string email)
        {
            string msg = "";
            if (email != null)
            {
                var result = GSEntity.checkemail(email).ToList();
                if (result.Count() > 0)
                {
                    DateTime expiretime = DateTime.Now.AddHours(2);
                    string rndmno = SendMail("resetlink", email, "http://" + Request.Url.Authority + "/Account/Sendpassword?recoverid=");
                    GSEntity.InsertIntoResetpassword(email, rndmno, expiretime);
                     msg = "Reset password link sent to " + email;
                }
                else
                {
                    msg = "This email does not exist";
                }
            }
            return Json(msg,JsonRequestBehavior.AllowGet);
        }
    }
}