using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GimpySoftwareNewTemplate.Models;
using KnolSecurity;

namespace GimpySoftwareNewTemplate.Controllers
{
    public class CampaignController : Controller
    {
        private int Total;
        private int PageLength;
        private int TotalPages;
        private int PageAdddedLength;
        private int TotalAdded;
        private int TotalAddedPages;
        int start = 0, end = 0, curPage = 0;

        GimpyDataEntities GSEntity = new GimpyDataEntities();

        [HttpGet]
        public ActionResult AddCampaign()
        {
            return View();
        }

        public ActionResult CampaignDetails(int CurrentPage = 0, int? droppaging = 0)
        {
            if (Session["userid"] != null && Session["cid"] != null)
            {
                if (CurrentPage == 0)
                {
                    CurrentPage = 1;
                }

                PageLength = Convert.ToInt32(droppaging);

                if (PageLength == 0)
                {
                    PageLength = 5;
                }

                if (Session["PageLength"] == null)
                {
                    Session["PageLength"] = "5";
                }

                if (PageLength.ToString() != Session["PageLength"].ToString())
                {
                    Session["PageLength"] = PageLength.ToString();
                }

                int? userid = Convert.ToInt32(Session["userid"].ToString());
                int? cid = Convert.ToInt32(Session["cid"].ToString());

                ViewBag.GetCampaignresultByID = GSEntity.GetCampaignDataByID(userid, cid).ToList();
                List<GetCampaignByID_Result> addedComps = GSEntity.GetCampaignByID(cid).ToList();

                SearchCampaignCalculatePaging(addedComps.Count);

                CurrentPage = ((PageLength * CurrentPage) > Total) ? TotalPages : CurrentPage;
                ViewBag.CurrentPage = CurrentPage;
                int start = (PageLength * (CurrentPage - 1) + 1);
                int end = (PageLength * CurrentPage);

                if (start > 1)
                {
                    addedComps.RemoveRange(0, start - 1);
                }

                if (addedComps.Count - PageLength > 0)
                {
                    addedComps.RemoveRange(PageLength, addedComps.Count - PageLength);
                }

                ViewBag.addedcompanies = addedComps;
            }
            return View();
        }

        [HttpPost]
        public ActionResult AddCampaign(string btnclick, string name, string description)
        {
            if (Session["userid"] != null)
            {

                int? userid = Convert.ToInt32(Session["userid"].ToString());
                if (name != "")
                {
                    GSEntity.AddNewCompaign(name, description, userid);
                }

                if (btnclick != "Btnsaveandanother")
                    return RedirectToAction("AllCampaigns", "Campaign");
                else
                    return RedirectToAction("AddCampaign", "Campaign");
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        [HttpGet]
        public ActionResult AllCampaigns(int? ID, string status = "")
        {
            
            if (Session["userid"] != null)
            {
                int? userid = Convert.ToInt32(Session["userid"].ToString());
                if (userid != null)
                {
                    if (status != "")
                    {
                        if (status == "inactive")
                        {
                            ViewBag.allcampaign = GSEntity.GetAllInActiveCampaigns(userid).ToList();
                            Session["status"] = "inactive";
                        }
                        else
                        {
                            ViewBag.allcampaign = GSEntity.GetAllActiveCampaigns(userid).ToList();
                            Session["status"] = "active";
                        }
                    }
                    else
                    {
                        ViewBag.allcampaign = GSEntity.GetAllActiveCampaigns(userid).ToList();
                    }
                }
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }

            return View();

        }

        public ActionResult CampaignStatus(int? ID, int CurrentPage = 0, string status = "")
        {
            if (CurrentPage == 0)
            {
                CurrentPage = 1;
            }
            if (Session["PageLength"] == null)
            {
                Session["PageLength"] = "5";
                PageLength = 5;
            }
            else if (Session["PageLength"] != null)
            {
                PageLength = Convert.ToInt32(Session["PageLength"].ToString());
            }
            if (Session["userid"] != null)
            {
                int? userid = Convert.ToInt32(Session["userid"].ToString());
                if (userid != null)
                {
                    CalculatePagination(status, CurrentPage);
                    ViewBag.allcategory = GSEntity.GetCategory().ToList();
                    if (status != null)
                    {
                        if (status == "inactive")
                        {
                            ViewBag.allcompanies = GSEntity.GetAllInActiveCompany(userid, start, end).ToList();
                        }
                        else
                        {
                            ViewBag.allcompanies = GSEntity.GetAllCompanyByUserID(userid, start, end).ToList();
                        }
                    }
                    else
                    {
                        ViewBag.allcompanies = GSEntity.GetAllCompanyByUserID(userid, start, end).ToList();
                    }

                    ViewBag.allgenres = GSEntity.GetAllGenresByUserID(userid).ToList();
                }
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
            return View();
        }

        private int GetTotalContactsInCampaign(int campaignID, int? userid)
        {
            List<FindCampaignsCompanies_Result> comps = GSEntity.FindCampaignsCompanies("%%", "%%", "%%", "", "", userid).ToList();
            List<int?> compsID = GSEntity.FindCompaniesByCampaigns(campaignID).ToList();

            int k = 0;

            for (int i = 0; i < comps.Count; i++)
            {
                if (compsID.Contains(comps[i].id))
                {
                    comps.RemoveAt(i);
                    i--;
                    k++;
                    if (compsID.Count == k)
                    {
                        break;
                    }
                }
            }

            return comps.Count;
        }

        [HttpGet]
        public ActionResult AddContactToCampaign(int cid=0, int CurrentPage = 0, int CurrentAddedPage = 0, string status = "")
        {
            if (CurrentPage == 0)
            {
                CurrentPage = 1;
            }
            if (CurrentAddedPage == 0)
            {
                CurrentAddedPage = 1;
            }

            if (Session["PageLength"] == null)
            {
                Session["PageLength"] = "5";
                PageLength = 5;
            }

            if (Session["PageAddedLength"] == null)
            {
                Session["PageAddedLength"] = "5";
                PageAdddedLength = 5;
            }

            if (Session["userid"] != null)
            {
                int? userid = Convert.ToInt32(Session["userid"].ToString());
                if (userid != null)
                {
                    Session["cid"] = cid;
                    ViewBag.CurrentPage = CurrentPage;
                    ViewBag.CurrentAddedPage = CurrentAddedPage;
                    ViewBag.allcategory = GSEntity.GetCategory().ToList();
                    ViewBag.allgenres = GSEntity.GetAllGenresByUserID(userid).ToList();
                    ViewBag.allcompanies = null;
                    ViewBag.country = GSEntity.GetCountryByUser(userid).ToList();
                    int campaignID = Convert.ToInt32(Session["cid"].ToString());
                    List<FindCampaignsCompanies_Result> comps = GSEntity.FindCampaignsCompanies("%%", "%%", "%%", "", "", userid).ToList();
                    List<int?> compsID = GSEntity.FindCompaniesByCampaigns(campaignID).ToList();

                    int k = 0;

                    for (int i = 0; i < comps.Count; i++)
                    {
                        if (compsID.Contains(comps[i].id))
                        {
                            comps.RemoveAt(i);
                            i--;
                            k++;
                            if (compsID.Count == k)
                            {
                                break;
                            }
                        }
                    }

                    ViewBag.campContactsCount = comps.Count;

                    SearchCampaignCalculatePaging(comps.Count);
                    CurrentPage = ((PageLength * CurrentPage) > Total) ? TotalPages : CurrentPage;
                    ViewBag.CurrentPage = CurrentPage;
                    int start = (PageLength * (CurrentPage - 1) + 1);
                    int end = (PageLength * CurrentPage);

                    if (start > 1)
                    {
                        comps.RemoveRange(0, start - 1);
                    }

                    if (comps.Count - PageLength > 0)
                    {
                        comps.RemoveRange(PageLength, comps.Count - PageLength);
                    }

                    ViewBag.allcategory = GSEntity.GetCategory().ToList();
                    ViewBag.allgenres = GSEntity.GetAllGenresByUserID(userid).ToList();
                    ViewBag.allcompanies = comps;
                    List<int?> countComps = GSEntity.GetCampaignCompaniesCount(campaignID).ToList();

                    AddedCompaniesCalculatePaging(Convert.ToInt32(countComps[0]));
                    CurrentAddedPage = ((PageAdddedLength * CurrentAddedPage) > TotalAdded) ? TotalAddedPages : CurrentAddedPage;
                    ViewBag.CurrentAddedPage = CurrentAddedPage;
                    int startAdded = (PageAdddedLength * (CurrentAddedPage - 1) + 1);
                    int endAdded = (PageAdddedLength * CurrentAddedPage);

                    ViewBag.addedcompanies = GSEntity.GetCampaignCompanies(campaignID, startAdded, endAdded).ToList();
                    ViewBag.searchData = new CompanyParameters();
                }
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
            return View();
        }

        [HttpPost]
        public ActionResult AddContactToCampaign(string btnclick, List<int> chkgen, List<int> chkcat, int? droppaging, int? droppagingAdded, int? ID, string country, string region, string company, int CurrentAddedPage = 0, int CurrentPage = 0)
        {
            string genres = "", category = "";
            int? userid = Convert.ToInt32(Session["userid"].ToString());
            if (userid != null)
            {
                if (CurrentPage == 0)
                {
                    CurrentPage = 1;
                }
                if (CurrentAddedPage == 0)
                {
                    CurrentAddedPage = 1;
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
                }
                if (category != "")
                {
                    category = category.Remove(category.Length - 1);
                }

                if (country == "" || country.Contains("---------------------"))
                {
                    country = "";
                }
                if (region == "Select State")
                {
                    region = "";
                }

                PageLength = Convert.ToInt32(droppaging);

                if (Session["PageLength"] == null)
                {
                    Session["PageLength"] = 5;
                }

                if (PageLength.ToString() != Session["PageLength"].ToString())
                {
                    Session["PageLength"] = PageLength.ToString();
                }

                PageAdddedLength = Convert.ToInt32(droppagingAdded);

                if (Session["PageAddedLength"] == null)
                {
                    Session["PageAddedLength"] = 5;
                }

                if (PageAdddedLength.ToString() != Session["PageAddedLength"].ToString())
                {
                    Session["PageAddedLength"] = PageAdddedLength.ToString();
                }

                int campaignID = Convert.ToInt32(Session["cid"].ToString());

                ViewBag.campContactsCount = GetTotalContactsInCampaign(campaignID, userid);

                List<FindCampaignsCompanies_Result> comps = GSEntity.FindCampaignsCompanies("%" + company + "%", "%" + country + "%", "%" + region + "%", category, genres, userid).ToList();
                List<int?> compsID = GSEntity.FindCompaniesByCampaigns(campaignID).ToList();

                int k = 0;

                for (int i = 0; i < comps.Count; i++)
                {
                    if (compsID.Contains(comps[i].id))
                    {
                        comps.RemoveAt(i);
                        i--;
                        k++;
                        if (compsID.Count == k)
                        {
                            break;
                        }
                    }
                }

                SearchCampaignCalculatePaging(comps.Count);
                CurrentPage = ((PageLength * CurrentPage) > Total) ? TotalPages : CurrentPage;
                ViewBag.CurrentPage = CurrentPage;
                int start = (PageLength * (CurrentPage - 1) + 1);
                int end = (PageLength * CurrentPage);

                if (start > 1)
                {
                    comps.RemoveRange(0, start - 1);
                }

                if (comps.Count - PageLength > 0)
                {
                    comps.RemoveRange(PageLength, comps.Count - PageLength);
                }

                ViewBag.allcategory = GSEntity.GetCategory().ToList();
                ViewBag.allgenres = GSEntity.GetAllGenresByUserID(userid).ToList();

                ViewBag.allcompanies = comps;
                List<int?> countComps = GSEntity.GetCampaignCompaniesCount(campaignID).ToList();

                AddedCompaniesCalculatePaging(Convert.ToInt32(countComps[0]));
                CurrentAddedPage = ((PageAdddedLength * CurrentAddedPage) > TotalAdded) ? TotalAddedPages : CurrentAddedPage;
                ViewBag.CurrentAddedPage = CurrentAddedPage;
                int startAdded = (PageAdddedLength * (CurrentAddedPage - 1) + 1);
                int endAdded = (PageAdddedLength * CurrentAddedPage);

                ViewBag.addedcompanies = GSEntity.GetCampaignCompanies(campaignID, startAdded, endAdded).ToList();
                ViewBag.searchData = searchData;
            }
            return View();
        }

        [HttpPost]
        public ActionResult AllCampaigns(int? ID, string btnclick, string name,string chkactives)
        {

            if (Session["userid"] != null)
            {
                   //int userid = Convert.ToInt32(Session["userid"].ToString());
                   // if (chkactives == "true")
                   // {
                        
                   //     //ViewBag.allcompanies = GSEntity.GetAllCompaign("%" + name + "%",  userid).ToList();
                   //     //ViewBag.allcompanies = GSEntity.GetAllInActiveContacts(userid);
                   //     ViewBag.finddata = "no";
                   //     Session["status"] = "inactive";
                   // }
                   // else
                   // {
                        
                   //     var searchparameter = (CompanyParameters)Session["searchparameter"];
                   //     ViewBag.allcompanies = GSEntity.FindActive_Contacts("%" + searchparameter.Name + "%", searchparameter.Country, searchparameter.Region, searchparameter.Category, searchparameter.Genre, userid).ToList();
                   //     //ViewBag.allcompanies = GSEntity.GetAllActiveContacts(userid);
                   //     ViewBag.finddata = "no";
                   //     Session["status"] = "active";
                   // }
                    
                   
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
            return View();
        }

        void CalculatePagination(string status = "", int CurrentPage = 0)
        {
            int? userid = Convert.ToInt32(Session["userid"].ToString());
            Total = (Int32)GSEntity.CampaignCount(userid, status).FirstOrDefault();
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

            CurrentPage = ((PageLength * CurrentPage) > Total) ? TotalPages : CurrentPage;
            ViewBag.CurrentPage = CurrentPage;
            start = (PageLength * (CurrentPage - 1) + 1);
            end = (PageLength * CurrentPage);
        }

        private void SearchCalculatePaging(string name, int? userid)
        {
            //Total = GSEntity.FindCampaign("%" + name + "%",userid).ToList().Count();
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

        private void SearchCampaignCalculatePaging(int total)
        {
            Total = total;
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

        private void AddedCompaniesCalculatePaging(int total)
        {
            TotalAdded = total;
            PageAdddedLength = Convert.ToInt32(Session["PageAddedLength"]);
            TotalAddedPages = 0;
            if (PageAdddedLength != 0)
            {
                if (TotalAdded % PageAdddedLength == 0)
                {
                    TotalAddedPages = TotalAdded / PageAdddedLength;
                }
                else
                {
                    TotalAddedPages = (TotalAdded / PageAdddedLength) + 1;
                }
            }
            ViewBag.TotalAdded = TotalAdded;
            ViewBag.PageAddedLength = PageAdddedLength;
            ViewBag.TotalAddedPages = TotalAddedPages;
        }

        [HttpGet]
        public JsonResult CheckCampaignExists(string name)
        {
            int count = Convert.ToInt32(GSEntity.CheckExists(name).FirstOrDefault());
            if (count != 0)
            {
                return Json(count, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult DeleteCampaign(string chkall)
        {
            if (chkall != "")
            {
                chkall = chkall.Remove(chkall.Length - 1);
                string[] chkedall = chkall.Split(',');
                foreach (string item in chkedall)
                {
                    GSEntity.DeleteCampaign(Convert.ToInt32(item));
                }
            }
            return Json("AllCampaign", JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCampaignID(int? cid)
        {
            if (cid != 0)
            {
                Session["cid"] = cid;
                //if (Session["userid"] != null)
                //{
                //    int? userid = Convert.ToInt32(Session["userid"].ToString());
                //   gtcomp.result = GSEntity.GetCampaignByID(userid, cid).ToList();
                //}
                return Json("AllCampaign", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult AddUpdate()
        {
            ViewBag.allstatus = GSEntity.GetAllCampaignStatus().ToList();
            return View();
        }

        public JsonResult AddContact(string companyIDs)
        {
            int campaignID = Convert.ToInt32(Session["cid"].ToString());
            string[] companies = companyIDs.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var CID in companies)
            {
                GSEntity.InsertCompaniesToCampaign(campaignID, Convert.ToInt32(CID));
                GSEntity.AddCampaignActivity(Convert.ToInt32(CID), campaignID, 0, "Added to campaign", DateTime.Now);
            }
            return Json("AddContact", JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteContact(string companyIDs)
        {
            int campaignID = Convert.ToInt32(Session["cid"].ToString());
            string[] companies = companyIDs.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var CID in companies)
            {
                GSEntity.DeleteCompaniesFromCampaign(campaignID, Convert.ToInt32(CID));
            }
            return Json("DeleteContact", JsonRequestBehavior.AllowGet);
        }

        void Calculate_Pagination(string status = "", string searchstatus = "", string searchname = "", int CurrentPage = 0)
        {
            int? userid = Convert.ToInt32(Session["userid"].ToString());
            if (searchstatus != "yes")
            {
                Total = (Int32)GSEntity.CampaignCount(userid, status).FirstOrDefault();
            }

            else
            {
                Total = (Int32)GSEntity.FindCampaignCount("%" + searchname + "%", userid, status).FirstOrDefault();
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

            ViewBag.Total = Total;
            ViewBag.PageLength = PageLength;
            ViewBag.TotalPages = TotalPages;
        }

        public JsonResult GetInactiveResult(string status = "", int? pagesize = 0, int CurrentPage = 0, string sortby = "", string sorttype = "", string searchstatus = "", string searchname = "", string allrecords = "")
        {
            if (CurrentPage == 0)
            {
                CurrentPage = 1;
            }
            if (searchstatus == "yes")
            {
                allrecords = "";
            }
            if (pagesize != null)
            {
                PageLength = Convert.ToInt32(pagesize);
            }

            if (Session["userid"] != null)
            {
                int? userid = Convert.ToInt32(Session["userid"].ToString());
                if (userid != null)
                {
                    if (allrecords == "yes")
                    {
                        #region all
                        searchname = "";
                        searchstatus = "";
                        if (sortby != "")
                        {
                            Calculate_Pagination(status, "", "", CurrentPage);
                            var data = GSEntity.SortCampaign(userid, sortby, sorttype, status, start, end).ToList();
                            var recordset = data;
                            var result = new { recordset, Total, PageLength, TotalPages, status, curPage };
                            return Json(result, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            sortby = "name";
                            sorttype = "asc";
                            Calculate_Pagination(status, "", "", CurrentPage);
                            var data = GSEntity.SortCampaign(userid, sortby, sorttype, status, start, end).ToList();
                            var recordset = data;
                            var result = new { recordset, Total, PageLength, TotalPages, status, curPage };
                            return Json(result, JsonRequestBehavior.AllowGet);
                        }

                        #endregion


                    }
                    else
                    {
                        #region other
                        if (status != "")
                        {
                            #region other
                            if (sortby == "" && searchstatus == "yes")
                            {
                                sortby = "name";
                                sorttype = "asc";
                                Calculate_Pagination(status, searchstatus, searchname, CurrentPage);
                                var data = GSEntity.FindCampaign('%' + searchname + '%', userid, status, start, end).ToList();
                                var recordset = data;
                                var result = new { recordset, Total, PageLength, TotalPages, status, curPage };
                                return Json(result, JsonRequestBehavior.AllowGet);
                            }
                            else if (sortby != "" && searchstatus == "yes")
                            {
                                Calculate_Pagination(status, searchstatus, searchname, CurrentPage);
                                var data = GSEntity.FindCampaignSort('%' + searchname + '%', userid, status, sortby, sorttype, start, end).ToList();
                                var recordset = data;
                                var result = new { recordset, Total, PageLength, TotalPages, status, curPage };
                                return Json(result, JsonRequestBehavior.AllowGet);
                            }
                            else if (sortby != "")
                            {
                                Calculate_Pagination(status, "", "", CurrentPage);
                                var data = GSEntity.SortCampaign(userid, sortby, sorttype, status, start, end).ToList();
                                var recordset = data;
                                var result = new { recordset, Total, PageLength, TotalPages, status, curPage };
                                return Json(result, JsonRequestBehavior.AllowGet);
                            }
                            else
                            {
                                sortby = "name";
                                sorttype = "asc";
                                Calculate_Pagination(status, "", "", CurrentPage);
                                var data = GSEntity.SortCampaign(userid, sortby, sorttype, status, start, end).ToList();
                                var recordset = data;
                                var result = new { recordset, Total, PageLength, TotalPages, status, curPage };
                                return Json(result, JsonRequestBehavior.AllowGet);
                            }

                            #endregion
                        }
                        else
                        {
                            var data = GSEntity.GetAllCampaignByUserID(userid, start, end).ToList();
                            var recordset = data;
                            var result = new { recordset, Total, PageLength, TotalPages, status, curPage };
                            return Json(result, JsonRequestBehavior.AllowGet);
                        }
                        #endregion
                    }

                }
                else
                {
                    return Json("Login/Account", JsonRequestBehavior.AllowGet);

                }
            }
            else
            {
                return Json("Login/Account", JsonRequestBehavior.AllowGet);
            }
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

        public ActionResult GetCampaign()
        {
            return View();
        }

    }
}