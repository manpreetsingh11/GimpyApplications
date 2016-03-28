using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GimpySoftwareNewTemplate.Controllers
{
    public class AdminPanelController : Controller
    {
        public ActionResult Dashboard()
        {
            return View();
        }
        public ActionResult Logout()
        {
            Session["UserInfo"] = null;
            return RedirectToAction("Login","Account");
        }
    }
}