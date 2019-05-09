using MVC_CircloidTemplate.App_Classes;
using MVC_CircloidTemplate.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_CircloidTemplate.Controllers
{
    
    public class HomeController : Controller
    {
        NorthwindEntities db = new NorthwindEntities();
        // GET: Home
        public ActionResult Index()
        {
            ViewBag.ActiveUserCount = HttpContext.Application["ActiveUserCount"];
            ViewBag.TotalUserCount= HttpContext.Application["TotalUserCount"];

            return View();
        }
        public ActionResult AddCookie()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddCookie(string CookieName,string CookieValue)
        {
            HttpCookie hc = new HttpCookie(CookieName, CookieValue);
            hc.Expires = DateTime.Now.AddDays(2);
            Response.Cookies.Add(hc);
            return View();
        }
        public ActionResult RetrieveCookie()
        {
            string cookieVal = Request.Cookies["Sc201-Morning"].Value.ToString();
            ViewBag.CookieValue = cookieVal;
            return View();
        }
        public ActionResult MyCart()
        {
            return View();
        }

        //public ActionResult RemoveCart(int id)
        //{
        //    try
        //    {
        //        ((Cart)Session["CurrentCart"]).PrdList.RemoveAll(x=>x.ProductID==id);
        //        return RedirectToAction("MyCart");
        //    }
        //    catch (Exception)
        //    {
        //        return RedirectToAction("MyCart");

        //    }            
        //}
        [HttpPost]
        public string RemoveCart(int id)
        {
            try
            {
                ((Cart)Session["CurrentCart"]).PrdList.RemoveAll(x => x.ProductID == id);
                return "Başka Bişeyler";
            }
            catch (Exception)
            {
                return "Bişeyler";

            }
        }
        public ActionResult PartialCartListView()
        {
            if (Session["CurrentCart"] != null)
            {
                Cart c = (Cart)Session["CurrentCart"];
                int n = c.PrdList.Count();
                return PartialView(c.PrdList);
            }
            return PartialView();
            
        }
    }
}