using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC_CircloidTemplate.Models;

namespace MVC_CircloidTemplate.Controllers
{
    [Authorize]
    public class CategoryController : Controller
    {
        NorthwindEntities db = new NorthwindEntities();

        // GET: Category
        public ActionResult Index()
        {
            List<Category> ctg = db.Categories.ToList();
            return View(ctg);
        }

        // GET
        public ActionResult AddCategory ()
        {
            return View();
        }
        //Category PİCTURE nesnesi Database'de byte[] şekklinde tutulduğu için seçilen resmi byte[] ' e cevrilmesini sağlayan metod 
        public byte[] ConvertToBytes(HttpPostedFileBase image)
        {
            byte[] imageBytes = null;
            BinaryReader reader = new BinaryReader(image.InputStream);
            imageBytes = reader.ReadBytes(image.ContentLength);
            byte[] bytes = new byte[imageBytes.Length + 78];
            Array.Copy(imageBytes, 0, bytes, 78, imageBytes.Length);
            return bytes;
        }

        [HttpPost]
        public ActionResult AddCategory([Bind(Include = "CategoryName, Description")] Category ctg, HttpPostedFileBase Picture)
        {
            if (Picture==null)
            {
                return View();
            }
            ctg.Picture = ConvertToBytes(Picture);
            db.Categories.Add(ctg);
            db.SaveChanges();
            return RedirectToAction("Index");


        }

     
        public ActionResult UpdateCategory(int id)
        {


            Category ctg = db.Categories.Find(id);

           

            return View(ctg);
        }

        [HttpPost]
        public ActionResult Updatecategory([Bind(Include = "CategoryID,CategoryName, Description")] Category ctg, HttpPostedFileBase Picture)
        {
            if (ModelState.IsValid)
            {

                Category k = db.Categories.Find(ctg.CategoryID);

                k.CategoryName = ctg.CategoryName;
                k.Description = ctg.Description;
                if (Picture != null)
                {
                    k.Picture = ConvertToBytes(Picture);
                }

                db.SaveChanges();

              
            }
            return RedirectToAction("Index");
        }
        //public ActionResult DeleteCategory(int id)
        //{

        //    Category ctg = db.Categories.Find(id);
        //    return View(ctg);

        //}

        //[HttpPost]
        //public ActionResult DeleteCategory(Category c)
        //{
        //    Category ctg = db.Categories.Find(c.CategoryID);
        //    db.Categories.Remove(ctg);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}



        [HttpPost]
        public ActionResult DeleteCategory(int? id)
        {
            Category ctg = db.Categories.Find(id);
            db.Categories.Remove(ctg);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


    }
}