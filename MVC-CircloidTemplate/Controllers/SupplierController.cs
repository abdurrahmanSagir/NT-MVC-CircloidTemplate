using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC_CircloidTemplate.Models;

namespace MVC_CircloidTemplate.Controllers
{
    public class SupplierController : Controller
    {
        NorthwindEntities db = new NorthwindEntities();

        // GET: Supplier
        public ActionResult Index()
        {
            List<Supplier> sup = db.Suppliers.ToList();
            return View(sup);
        }
        public ActionResult AddSupplier()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddSupplier(Supplier sup)
        {
            db.Suppliers.Add(sup);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult UpdateSupplier(int id)
        {
          

            Supplier sup = db.Suppliers.Find(id);

            

            return View(sup);
        }

        [HttpPost]
        public ActionResult UpdateSupplier(Supplier s)
        {
            Supplier sup = db.Suppliers.Find(s.SupplierID);


            //db.Entry(s).State = System.Data.Entity.EntityState.Modified;
            sup.CompanyName = s.CompanyName;
            sup.ContactName = s.ContactName;
            sup.ContactTitle = s.ContactTitle;


            db.SaveChanges();

            return RedirectToAction("Index");
        }

        //public ActionResult DeleteSupplier(int id)
        //{
        //    Supplier sup = db.Suppliers.Find(id);
        //    return View(sup);

        //}
        //Bu methodun içinde aluşan gata Ajax'ı etkilemez. Ajax için success Ajax'ın doğru bir şekilde actiona ulaşmış olmasıyyla ilgilidir. Bu methodda veritabanında ilişkilerden dolayı kayıt silinemez ve benzeri hatalar Ajax'ı ilgilendirmez. bu yüzden bu method içinde oluşan hatalarla ilgili Ajax tarafına bilgi göndermeliyiz.
        [HttpPost]
        public string DeleteSupplier(int id)
        {
            try
            {
                Supplier sup = db.Suppliers.Find(id);
                db.Suppliers.Remove(sup);
                db.SaveChanges();
                return "OK";
            }
            catch (Exception)
            {

                return "ERROR";
            }


        }
        //[HttpPost]
    //    public ActionResult DeleteSupplier(Supplier s)
    //    {
    //        Supplier sup = db.Suppliers.Find(s.SupplierID);
    //        db.Suppliers.Remove(sup);
    //        db.SaveChanges();
    //        return RedirectToAction("Index");
    //    }
        }
}