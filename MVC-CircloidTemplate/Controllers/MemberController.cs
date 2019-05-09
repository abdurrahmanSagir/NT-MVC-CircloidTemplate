using MVC_CircloidTemplate.App_Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MVC_CircloidTemplate.Controllers
{
    public class MemberController : Controller
    {
        public MemberController()
        {
            ViewBag.Username = "";
        }
        // GET: Member
        public ActionResult MemberLogin()
        {
            return View();
        }
        public ActionResult MemberLogout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
        //[HttpPost]
        //public ActionResult MemberLogout()
        //{
        //    FormsAuthentication.SignOut();
        //    return RedirectToAction("Index", "Home");
        //}
        [HttpPost]
        public ActionResult MemberLogin(UserClass uc,string rememberMe)
        {
            bool validationResult = Membership.ValidateUser(uc.UserName, uc.Password);
            if (validationResult)
                
            {
                //Burası kullanıcı bilgileri doğru olduğunda gidilecek ekran için. Bunun için web.Config Dosyasında authorization ayarlarının yapılması gerekir. Ayarlar yapıldıktan sonra FormsAuthentication.RedirectFromLoginPage() methodu kullanılacaktır.
                if (rememberMe == "on")
                {
                    FormsAuthentication.RedirectFromLoginPage(uc.UserName, true);

                }
                else
                {
                    FormsAuthentication.RedirectFromLoginPage(uc.UserName, false);
                }

                return View();
            }
            else
            {
                ViewBag.Message = "Kullanıcı ya da Parola Hatalı";
                return View();
            }

        }
        public ActionResult CreateAccount()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateAccount(UserClass uc)
        {
            Membership.CreateUser(uc.UserName, uc.Password, uc.Email, uc.PasswordQuestion, uc.PasswordAnswer, true, out MembershipCreateStatus status);
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
                    createMessage = "Geçersiz Gizli Soru!";
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
            ViewBag.createMessage = createMessage;
            if (createMessage == "")
            {
                return RedirectToAction("MemberLogin");
            }
            else
            {
                return View();

            }
        }
        [HttpGet]
        public ActionResult ForgotPassword(string Username)
        {
            ViewBag.Username = Username;
            return View();
        }
        [HttpPost]
        public ActionResult ForgotPassword(UserClass uc)
        {
            MembershipUser user = Membership.GetUser(uc.UserName);
            //Burada başka kontroller yapabilirsin
            if(user.PasswordQuestion==uc.PasswordQuestion)
            {
                string oldpwd = user.ResetPassword(uc.PasswordAnswer);
                user.ChangePassword(oldpwd, uc.Password);
            }
            else
            {
                ViewBag.Message="Soru yanlış,Tekrar Dene";
                return View();
            }
                return RedirectToAction("MembereLogin");
        }
    }
}