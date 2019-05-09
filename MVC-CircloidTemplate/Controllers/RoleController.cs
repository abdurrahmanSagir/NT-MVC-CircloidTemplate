using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MVC_CircloidTemplate.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {
        // GET: Role
        public RoleController()
        {
            ViewBag.RoleSelected = "selected";
        }
        public ActionResult Index()
        {
            List<String> roleList= Roles.GetAllRoles().ToList();
            return View(roleList);
        }
        public ActionResult AddRole(string message="merhaba")
        {
            ViewBag.Message = message;
            return View();
        }

        [HttpPost]
        [ActionName("AddRole")]
        public ActionResult AddRolePost(string role)
        {
            if(string.IsNullOrWhiteSpace(role))
            {
                return RedirectToAction("AddRole", new { message = "Rol Boş olamaz" });
            }
            if (Roles.RoleExists(role))
                return RedirectToAction("AddRole", new { message = "Rol zaten kayıtlı" });

            Roles.CreateRole(role);
            return View();
        }
        //public ActionResult AddRole(string role)
        //{
        //    ViewBag.CreateMessage = "";
        //    if(!(role==""))
        //    {
        //        Roles.CreateRole(role);
        //        return RedirectToAction("Index");
                
        //    }
        //    else
        //    {
        //        ViewBag.CreateMessage = "Rol ismi boş bırakılamaz";
        //    }
        //    return View();
        //}
        
        public ActionResult DeleteRole(string role)
        {
            Roles.DeleteRole(role);
            return RedirectToAction("Index");
        }
        
    }
}