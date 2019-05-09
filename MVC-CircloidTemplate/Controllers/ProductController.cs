using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC_CircloidTemplate.App_Classes;
using MVC_CircloidTemplate.Models;

namespace MVC_CircloidTemplate.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        NorthwindEntities db = new NorthwindEntities();
        // GET: Product
        public ActionResult Index()
        {
            List<Product> prdList = db.Products.ToList();
            return View(prdList);
        }
        public ActionResult AddProduct()
        {
            ViewBag.catList = db.Categories.ToList();
            ViewBag.suplist = db.Suppliers.ToList();
            return View();
        }

        [HttpPost]
        public ActionResult AddProduct(Product p)
        {
            db.Products.Add(p);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        /* Delete islemini 3 farklı yol ile yapacagız. Ilk cözümümüz sil butonuna basılınca yeni bir view acılacak yani kullanıcı yeni bir sayfaya 
         * yönlendirilecek ve onay verilirse silinecek.
         2. yol sil butonuna bsılınca yukrıda alert kutusu cıkacak ve kayıt silinsin mi diye soracak evet denirse silinecek bu yöntemin dezavantajı
         alert kutusu bir kaç kez görüntülendikten sonra browser otomatik olarak alert kutusu altına checkbox ekliyor ve bu mesajı tekrar gösterme seceneği 
         sunuyor. Eğer kullanıcı checkbox işarertlerse tekrar alert kutuusu gözükmeyeceği için silme işlemi gerçekleştirilemiyor(Ajax kodu yazılacak) 
         
         3. yol  sil butonuna basılınca küçük bir pencere acılacak yani başka sayfaya yönderilmeyecek evet seçilirse silme işlemi gerçekleşecek
         (Ajax Kodu yazılacak )*/
        public ActionResult DeleteProduct(int id)
        {
            Product prd = db.Products.FirstOrDefault(x=>x.ProductID==id);
            return View(prd);

        }

        [HttpPost]
        public ActionResult DeleteProduct(Product p)
        {
            Product prod = db.Products.FirstOrDefault(x => x.ProductID == p.ProductID);
            db.Products.Remove(prod);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult UpdateProduct(/*int id*/)
        {
            int productID = Convert.ToInt32(Request.QueryString["prdID"].ToString());
            string productName = Request.QueryString["prdName"].ToString();
            string productFrom = Request.QueryString["prdFrom"].ToString();
            Product product = db.Products.FirstOrDefault(x => x.ProductID == productID);
            //Product product = db.Products.Find(id);
            ViewBag.catList = db.Categories.ToList();
            ViewBag.supList = db.Suppliers.ToList();
            ViewBag.productFrom = productFrom;

            return View(product);
        }

        [HttpPost]
        public ActionResult UpdateProduct(Product prd)
        {
            Product product = db.Products.Find(prd.ProductID);

           

            product.ProductName = prd.ProductName;
            product.UnitPrice = prd.UnitPrice;
            product.UnitsInStock = prd.UnitsInStock;
            product.CategoryID = prd.CategoryID;
            product.SupplierID = prd.SupplierID;

            db.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public string AddToCart(int id)
        {
            //Sepet varsa, gelen ürünü var olan sepete ekle,Sepet yoksa önce aktif session için sepet oluştur ve oluşan sepete gelen ürünü ekle
            Cart crt;
            if (Session["CurrentCart"]==null)
            {
                crt = new Cart();
            }
            else
            {
                crt = (Cart)Session["CurrentCart"];                
            }
            foreach (Product item in crt.PrdList)
            {
                if(item.ProductID==id)
                {
                    return "Bu Eleman Zaten Sepette Mevcut";
                }
            }
            Product prd = db.Products.Find(id);
            crt.PrdList.Add(prd);
            Session["CurrentCart"] = crt;

            return "Ürün Başarıyla Eklendi";
        }
        public ActionResult PartialProductCountNav()
        {
            if(Session["CurrentCart"]!=null)
            {
                Cart c = (Cart)Session["CurrentCart"];
                int n = c.PrdList.Count();
                return PartialView(c.PrdList);
            }
            return PartialView();

        }
        

        

    }
}