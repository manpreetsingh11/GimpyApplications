using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GimpySoftwareNewTemplate.Models;
using KnolSecurity;

namespace GimpySoftwareNewTemplate.Controllers
{
    public class ContactsController : Controller
    {
        private int Total;
        private int PageLength;
        private int TotalPages;
        int start = 0, end = 0, curPage = 0, userid = 0;

        GimpyDataEntities GSEntity = new GimpyDataEntities();

        [HttpGet]
        public ActionResult AddContact()
        {

            if (Session["userid"] != null)
            {
                userid = Convert.ToInt32(Session["userid"]);
                ViewBag.genres = GSEntity.getgenrebyuserid(userid).ToList();
                //ViewBag.country = GSEntity.GetCountryByUser(userid).ToList();
            }
            else
            {
                ViewBag.genres = null;
                ViewBag.country = null;
            }

            ViewBag.categories = GSEntity.GetCategory().ToList();

            return View();

        }

        [HttpPost]
        public ActionResult AddContact(int? category, string btnclick, string company, string contactname, string phone, string email, string website, string country, string region, string addressline1, string addressline2, string towncity, string PostalCode, List<int> chkgenre, string notes)
        {
            if (Session["userid"] != null)
            {
                int? userid = Convert.ToInt32(Session["userid"]);
                if (country == "" || country.Contains("---------------------") || country.Contains("Select Country"))
                {
                    country = "0";
                }
                if (region == "Select Region")
                {
                    region = "0";
                }

                GSEntity.AddNewCompany(company, category, email, phone, contactname, addressline1, addressline2, towncity, country, PostalCode, country, notes, true, userid, website, region);
                int companyid = Convert.ToInt32(GSEntity.GetCompanyID().First());

                if (companyid != null)
                {
                    if (chkgenre != null)
                    {
                        foreach (int item in chkgenre)
                        {
                            GSEntity.AddCompanyGenre(companyid, item);
                        }
                    }
                }
            }

            if (btnclick != "Btnsaveandanother")
                return RedirectToAction("AllContacts", "Contacts");
            else
                return RedirectToAction("AddContact", "Contacts");
        }

        [HttpGet]
        public ActionResult ContactDetail(string cid = "")
        {
            int ID;
            int? userid;
            string genres = "";
            if (Session["userid"] != null)
            {
                if (cid != "")
                {
                    ID = Convert.ToInt32(cid);
                    userid = Convert.ToInt32(Session["userid"]);
                    ViewBag.allgenre = GSEntity.GetAllCompanyGenresByUserID(userid, ID).ToList();
                    ViewBag.result = GSEntity.GetCompanyDetailByID(ID, userid).ToList();

                    foreach (var item in ViewBag.allgenre)
                    {
                        genres += item.genre + ",";
                    }

                    if (genres.Length > 0)
                    {
                        genres = genres.Remove(genres.Length - 1, 1);
                    }
                    ViewBag.genres = genres;

                    return View();
                }
                else
                {
                    return RedirectToAction("AllContacts", "Contacts");
                }

            }
            else
            {
                return RedirectToAction("AllContacts", "Contacts");

            }

        }

        public JsonResult CompanyDetailbyID(int? cid)
        {
            if (cid != 0)
            {
                Session["cid"] = cid;
                return Json("AllContacts", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult AllContacts(int? ID,string status = "")
        {
            if (Session["userid"] != null)
            {
                ViewBag.searchcompany = null;
                ViewBag.chkcategories = null;
                ViewBag.chkgenres = null;
                int? userid = Convert.ToInt32(Session["userid"].ToString());
                if (userid != null)
                {
                    ViewBag.categories = GSEntity.GetCategoryByUser(userid).ToList();
                    ViewBag.country = GSEntity.GetCountryByUser(userid).ToList();
                    ViewBag.genres = GSEntity.GetAllGenresByUserID(userid).ToList();

                    if (Session["searchparameter"] != null && (status == "" || status == "active"))
                    {
                        var searchparameter = (CompanyParameters)Session["searchparameter"];
                        ViewBag.allcompanies = GSEntity.FindActive_Contacts("%" + searchparameter.Name + "%", searchparameter.Country, searchparameter.Region, searchparameter.Category, searchparameter.Genre, userid).ToList();
                        Session["status"] = status;
                        ViewBag.searchData = searchparameter;
                    }
                    else if (Session["searchparameter"] != null && status == "inactive")
                    {
                        var searchparameter = (CompanyParameters)Session["searchparameter"];
                        ViewBag.allcompanies = GSEntity.Find_Contacts("%" + searchparameter.Name + "%", searchparameter.Country, searchparameter.Region, searchparameter.Category, searchparameter.Genre, userid).ToList();
                        Session["status"] = status;
                        ViewBag.searchData = searchparameter;
                    }
                    else
                    {
                        Session["status"] = status;
                        if (status == "inactive")
                        {
                            ViewBag.allcompanies = GSEntity.GetAllInActiveContacts(userid).ToList();
                        }
                        else
                        {

                            ViewBag.allcompanies = GSEntity.GetAllActiveContacts(userid).ToList();
                        }
                    }

                }
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
            return View();
        }

        [HttpPost]
        public ActionResult AllContacts(string btnclick, List<int> chkgen, List<int> chkcat, int? droppaging, int? ID, string country, string region, string company, int? curPage, string chkactives, int CurrentPage = 0)
        {
            string genres = "", category = "";
            ViewBag.searchcompany = company;
            ViewBag.chkcategories = chkcat;
            ViewBag.chkgenres = chkgen;

            int? userid = Convert.ToInt32(Session["userid"].ToString());
            if (userid != null)
            {
                if (country == "" || country.Contains("---------------------") || country.Trim() == "Select Country")
                {
                    country = " ";
                }
                if (region.Trim() == "Select Region")
                {
                    region = " ";
                }

                CompanyParameters searchData = new CompanyParameters() { Name = company, Country = country, Region = region };

                if (chkgen != null)
                {
                    searchData.Genres = chkgen;
                    foreach (int gen in chkgen)
                    {
                        genres += gen.ToString() + ",";
                    }
                }
                if (chkcat != null)
                {
                    searchData.Categories = chkcat;
                    foreach (int cat in chkcat)
                    {
                        category += cat.ToString() + ",";
                    }
                }
                if (genres != "")
                {
                    genres = genres.Remove(genres.Length - 1);
                    searchData.Genre = genres;
                }
                else
                {
                    searchData.Genre = "";
                }
                if (category != "")
                {
                    category = category.Remove(category.Length - 1);
                    searchData.Category = category;
                }
                else
                {
                    searchData.Category = "";
                }

                if (country == "" || country.Contains("---------------------"))
                {
                    country = " ";
                }
                if (region == "Select State")
                {
                    region = " ";
                }

                ViewBag.categories = GSEntity.GetCategoryByUser(userid).ToList();
                ViewBag.genres = GSEntity.GetAllGenresByUserID(userid).ToList();
                ViewBag.country = GSEntity.GetCountryByUser(userid).ToList();
                ViewBag.searchData = searchData;

                if (btnclick != "Delete")
                {
                    Session["searchparameter"] = searchData;

                    if (chkactives == "true")
                    {
                        
                        var searchparameter = (CompanyParameters)Session["searchparameter"];
                        ViewBag.allcompanies = GSEntity.Find_Contacts("%" + searchparameter.Name + "%", searchparameter.Country, searchparameter.Region, searchparameter.Category, searchparameter.Genre, userid).ToList();
                        //ViewBag.allcompanies = GSEntity.GetAllInActiveContacts(userid);
                        ViewBag.finddata = "no";
                        Session["status"] = "inactive";
                    }
                    else
                    {
                        
                        var searchparameter = (CompanyParameters)Session["searchparameter"];
                        ViewBag.allcompanies = GSEntity.FindActive_Contacts("%" + searchparameter.Name + "%", searchparameter.Country, searchparameter.Region, searchparameter.Category, searchparameter.Genre, userid).ToList();
                        //ViewBag.allcompanies = GSEntity.GetAllActiveContacts(userid);
                        ViewBag.finddata = "no";
                        Session["status"] = "active";
                    }
                    
                }
                else
                {
                    ViewBag.allcompanies = GSEntity.GetAllInActiveContacts(userid).ToList();
                    Session["searchparameter"] = null;
                }


            }
            return View();
        }

        [HttpPost]
        public ActionResult EditContact(int? cid, int? category, string btnclick, string company, string contactname, string phone, string email, string website, string country, string region, string addressline1, string addressline2, string towncity, string PostalCode, List<int> chkgenre, string notes)
        {
            if (Session["userid"] != null)
            {
                int? userid = Convert.ToInt32(Session["userid"].ToString());
                GSEntity.UpdateCompanyByID(cid, company, category, email, phone, contactname, addressline1, addressline2, towncity, country, PostalCode, notes, true, website, region);
                if (chkgenre != null)
                {
                    GSEntity.DeleteCompanyGenre(cid);
                    foreach (int genreid in chkgenre)
                    {
                        GSEntity.updateCompanyGenre(cid, genreid);
                    }

                }
            }
            return RedirectToAction("AllContacts", "Contacts");
        }

        [HttpGet]
        public ActionResult EditContact(int? ID)
        {
            if (Session["userid"] != null)
            {
                int? userid = Convert.ToInt32(Session["userid"].ToString());
                //int? cid = 0;
                string country = "";
                if (userid != null)
                {
                    Session["cid"] = ID;
                    ViewBag.contactdetail = GSEntity.GetCompanyByID(ID).ToList();
                    foreach (var item in ViewBag.contactdetail)
                    {
                        country = item.Country;
                    }
                    ViewBag.regions = GSEntity.GetRegionByCountry(userid, country).ToList();
                    ViewBag.allcategory = GSEntity.GetCategoryByUser(userid).ToList();
                    ViewBag.allgenres = GSEntity.GetAllGenresByUserID(userid).ToList();
                    ViewBag.companygenres = GSEntity.GetAllCompanyGenresByUserID(userid, ID).ToList();
                    ViewBag.country = GSEntity.GetCountryByUser(userid).ToList();

                }
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        private void SearchCalculatePaging(string country, string region, string company, string genres, string category, int? userid)
        {
            if (country == "" || country.Contains("---------------------"))
            {
                country = " ";
            }
            if (region == "Select State")
            {
                region = " ";
            }
            Total = GSEntity.FindData_count("%" + company + "%", country, region, category, genres, userid).ToList().Count();
            TotalPages = 0;
            if (PageLength != 0)
            {
                if (Total % PageLength == 0)
                {
                    TotalPages = Total / PageLength;
                }
                else
                {
                    TotalPages = (Total / PageLength) + 1;
                }
            }
            ViewBag.Total = Total;
            ViewBag.PageLength = PageLength;
            ViewBag.TotalPages = TotalPages;
        }

        private void SearchCalculatePagingActive(string country, string region, string company, string genres, string category, int? userid)
        {
            if (country == "" || country.Contains("---------------------"))
            {
                country = " ";
            }

            if (region == "Select State")
            {
                region = " ";
            }

            Total = GSEntity.FindData_countActive("%" + company + "%", country, region, category, genres, userid).ToList().Count();
            TotalPages = 0;

            if (PageLength != 0)
            {
                if (Total % PageLength == 0)
                {
                    TotalPages = Total / PageLength;
                }
                else
                {
                    TotalPages = (Total / PageLength) + 1;
                }
            }

            ViewBag.Total = Total;
            ViewBag.PageLength = PageLength;
            ViewBag.TotalPages = TotalPages;
        }

        void CalculatePagination(string status)
        {
            int? userid = Convert.ToInt32(Session["userid"].ToString());
            Get_Inactive_Total(status, userid);
            PageLength = Convert.ToInt32(Session["PageLength"]);
            TotalPages = 0;
            if (PageLength != 0)
            {
                if (Total % PageLength == 0)
                {
                    TotalPages = Total / PageLength;
                }
                else
                {
                    TotalPages = (Total / PageLength) + 1;
                }
            }

            ViewBag.Total = Total;
            ViewBag.PageLength = PageLength;
            ViewBag.TotalPages = TotalPages;
        }

        private void Get_Inactive_Total(string status, int? userid)
        {

            if (Session["status"] != null)
            {
                if (status == "active")
                {
                    Total = GSEntity.GetAllCompanyByID(userid).Count();
                }
                else
                {
                    Total = GSEntity.GetAllInActiveCompanyByUserID(userid).Count();
                }
            }
            else
            {
                if (status == "inactive")
                {
                    Total = GSEntity.GetAllInActiveCompanyByUserID(userid).Count();
                }
                else
                {
                    Total = GSEntity.GetAllCompanyByID(userid).Count();
                }
            }
        }

        [HttpGet]
        public JsonResult CheckCompanyExists(string company)
        {
            int? userid = Convert.ToInt32(Session["userid"].ToString());
            int count = Convert.ToInt32(GSEntity.CheckExistingCompanyWithUser(company, userid).FirstOrDefault());
            if (count != 0)
            {
                return Json(count, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult CheckCompanyExistswithID(string company)
        {
            int? userid = Convert.ToInt32(Session["userid"].ToString());
            int cid = Convert.ToInt32(GSEntity.CheckExistingCompanyWithID(company, userid).FirstOrDefault());
            if (cid != 0)
            {
                return Json(cid, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult DeleteCompany(string chkall)
        {
            if (chkall != "")
            {
                chkall = chkall.Remove(chkall.Length - 1);
                string[] chkedall = chkall.Split(',');
                foreach (string item in chkedall)
                {
                    GSEntity.DeleteCompany(Convert.ToInt32(item));
                }
            }
            return Json("AllContacts", JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetRegion(string country)
        {
            if (country != "")
            {
                int? userid = Convert.ToInt32(Session["userid"].ToString());
                var result = GSEntity.GetRegionByCountry(userid, country).ToList();
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetInactiveResult(string status = "")
        {
            List<Object> allcontacts = new List<object>();

            if (Session["userid"] != null)
            {
                userid = Convert.ToInt32(Session["userid"]);
                if (status == "inactive")
                {
                    var data = GSEntity.GetAllInActiveContacts(userid).ToList();
                    foreach (var item in data)
                    {
                        Company cc = new Company()
                        {
                            Active = item.active,
                            ContactName = item.name
                            
                        };
                        allcontacts.Add(cc);
                    }
                }
                else
                {

                    ViewBag.allcompanies = GSEntity.GetAllActiveContacts(userid).ToList();
                }

                return Json(new { data = allcontacts }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("Login/Account", JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetResult()
        {


            if (Session["userid"] != null)
            {
                int? userid = Convert.ToInt32(Session["userid"].ToString());
                var data = GSEntity.GetAllInActiveCompany1(userid);
                var recordset = data;
                var result = new { recordset };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("Login/Account", JsonRequestBehavior.AllowGet);
            }
        }
        public void Calculate_Paging(string status, string searchstatus, int CurrentPage, string country, string region, string company, string genres, string category, int? userid)
        {
            if (country == "" || country.Contains("---------------------"))
            {
                country = " ";
            }

            if (region == "Select State")
            {
                region = " ";
            }
            if ((searchstatus == "yes" && searchstatus != "") && status == "active")
            {
                Total = GSEntity.FindData_countActive("%" + company + "%", country, region, category, genres, userid).ToList().Count();
            }
            else if ((searchstatus == "yes" && searchstatus != "") && status == "inactive")
            {
                Total = GSEntity.FindData_count("%" + company + "%", country, region, category, genres, userid).ToList().Count();
            }

            else if ((searchstatus == "no" || searchstatus == "") && status == "inactive")
            {
                Total = GSEntity.GetAllInActiveCompanyByUserID(userid).Count();
            }
            else
            {
                Total = GSEntity.GetAllCompanyByID(userid).Count();
            }

            TotalPages = 0;
            if (PageLength != 0)
            {
                if (Total % PageLength == 0)
                {
                    TotalPages = Total / PageLength;
                }
                else
                {
                    TotalPages = (Total / PageLength) + 1;
                }
            }

            curPage = ((PageLength * CurrentPage) > Total) ? TotalPages : CurrentPage;
            start = (PageLength * (curPage - 1) + 1);
            end = (PageLength * curPage);

            ViewBag.CurrentPage = CurrentPage;
            ViewBag.Total = Total;
            ViewBag.PageLength = PageLength;
            ViewBag.TotalPages = TotalPages;
        }
    }
}