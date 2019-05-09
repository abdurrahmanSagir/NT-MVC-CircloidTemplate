using MVC_CircloidTemplate.App_Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MVC_CircloidTemplate.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        public UserController()
        {
            ViewBag.UserSelected = "selected";
        }
        // GET: User
        public ActionResult Index()
        {
            MembershipUserCollection users = Membership.GetAllUsers();
            return View(users);
        }
        public ActionResult AddUser()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddUser(UserClass uc)
        {
            Membership.CreateUser(uc.UserName, uc.Password, uc.Email, uc.PasswordQuestion, uc.PasswordAnswer, true,out MembershipCreateStatus status);
            string createMessage = "";
            switch (status)
            {
                case MembershipCreateStatus.Success:
                    break;
                case MembershipCreateStatus.InvalidUserName:
                    createMessage = "Geçersiz Kullanıcı Adı!";
                    break;
                case MembershipCreateStatus.InvalidPassword:
                    createMessage = "Geçersiz Şifre!";
                    break;
                case MembershipCreateStatus.InvalidQuestion:
                    createMessage="Geçersiz Gizli Soru!";
                    break;
                case MembershipCreateStatus.InvalidAnswer:
                    createMessage = "Geçersiz Gizli Cevap!";
                    break;
                case MembershipCreateStatus.InvalidEmail:
                    createMessage = "Geçersiz E-Mail!";
                    break;
                case MembershipCreateStatus.DuplicateUserName:
                    createMessage = "Kullanılmış Kullanıcı Adı!";
                    break;
                case MembershipCreateStatus.DuplicateEmail:
                    createMessage = "Kullanılmış email adresi!";
                    break;
                case MembershipCreateStatus.UserRejected:
                    createMessage = "Kullanıcı Engellendi!";
                    break;
                case MembershipCreateStatus.InvalidProviderUserKey:
                    createMessage = "Geçersiz Kullanıcı Anahtar Hatası!";
                    break;
                case MembershipCreateStatus.DuplicateProviderUserKey:
                    createMessage = "Tekrarlanmış Kullanıcı Anahtarı!";
                    break;
                case MembershipCreateStatus.ProviderError:
                    createMessage = "Sağlayıcı Hatası!";
                    break;
                default:
                    break;
            }
            ViewBag.createMessage= createMessage;
            if(createMessage=="")
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View();

            }
        }
        public ActionResult AssignRole(string username,string message=null)
        {
            /*Parametre olarak id yazmak zorundayız, sebebi projenin App_Start kalasörünün altında
             * Route_Config.cs dosyasında "{Controller}/{action}/{id}" bu parametre adının default adı
             * id olduğu için parametre adının da id olması gerekiyor.
             * 
             * Kullanıcı RolAta'ya tıkladığında kullanıcı adını parametre olarak buraya alıyoruz.
             * Buradan da kullanıcının adını view a gönderiyoruz. Amacımız parametre bilgisini View'e
             * taşımak. View tarafında Ekle butonuna basınca tekrar kullanıcı adını ve rol adını 
             * View'den alıp Post tarafına Taşımak.
             */
            if (string.IsNullOrWhiteSpace(username))
                return RedirectToAction("Index");
            MembershipUser user = Membership.GetUser(username);

            if (user == null)
                return HttpNotFound();

            string[] userRoles = Roles.GetRolesForUser(username);
            string[] allRoles= Roles.GetAllRoles();

            List<string> availableRoles = new List<string>();
            foreach (string  role in allRoles)
            {
                if (!userRoles.Contains(role))
                    availableRoles.Add(role);
            }
            ViewBag.AvailableRoles = availableRoles;
            ViewBag.UserRoles = userRoles;
            ViewBag.Username = username;
            ViewBag.Message = message;

            return View();
            
            
        }
        [HttpPost]
        public ActionResult AssignRole(string username,List<string> addedRoles)
        {
            if (addedRoles.Count < 1)
                return RedirectToAction("AssignRole", new { username = username, message = "Hata" });
            Roles.AddUserToRoles(username, addedRoles.ToArray());

            return RedirectToAction("AssignRole", new { username = username, message = "Başarılı" });
        }
        [HttpPost]
        public string DeleteRole(string username,string removedRoles)
        {
            string[] removedRolesArray = removedRoles.Split(',');
            if (removedRolesArray.Length < 1 || string.IsNullOrWhiteSpace(removedRolesArray[0]))
                return "Hata";
            Roles.RemoveUserFromRoles(username, removedRolesArray);
            return "Başarılı";
         }

        public ActionResult DeleteUser(string username)
        {
            MembershipUser user = Membership.GetUser(username);
            Membership.DeleteUser(username);
            return RedirectToAction("Index");
        }

        public ActionResult UserRoles(string username)
        {
            string[] userRoles = Roles.GetRolesForUser(username);
            ViewBag.UserRoles = userRoles;
            ViewBag.Username = username;
            return View();
        }
        [HttpPost]
        [ActionName("UserRoles")]
        public string UserRoles2(string username)
        {
            string roles="";
            string[] userRoles = Roles.GetRolesForUser(username);
            ViewBag.UserRoles = userRoles;
            foreach (var item in userRoles)
            {
                roles += item + " ";
            }
            return roles;
        }
    }
}