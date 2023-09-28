using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using C21_TH1_Lu_van_Duong.Models;
using Microsoft.EntityFrameworkCore;
using C21_TH1_Lu_van_Duong.Data;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace C21_TH1_Lu_van_Duong.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly IHostingEnvironment host;
        public ProductController(ApplicationDbContext _db, IHostingEnvironment _host)
        {
            db = _db;
            host = _host;
        }

        public IActionResult Index(int? page)
        {
            int pageSize = 5;
            int pageIndex;

            pageIndex = page == null ? 1 : (int)page;
            var lstProduct = db.Products.Include(x => x.Category).ToList();
            //int.pageCount = (int)Math.Ceiling((double)lstProduct.Count / pageSize);
            int pageCount = lstProduct.Count / pageSize + (lstProduct.Count % pageSize > 0 ? 1 : 0);

            ViewBag.pageCount = pageCount;
            ViewBag.pageIndex = pageIndex;

            return View(lstProduct.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList());

        }
        [HttpGet]
        public IActionResult Create()
        {
            var lstCategory = db.Categories.Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() });
            ViewBag.LstCategory = lstCategory;
            return View();
        }


        [HttpPost]
        public IActionResult Create(Product objProduct, IFormFile img)
        {
            if (ModelState.IsValid)
            {
                if (img != null)
                {
                    string path = Path.Combine(host.WebRootPath, @"img/Product");
                    string filename = Guid.NewGuid().ToString() + Path.GetExtension(img.FileName);
                    img.CopyTo(new FileStream(Path.Combine(path, filename), FileMode.Create));
                    objProduct.ImageUrl = @"img/Product" + filename;
                }
                db.Products.Add(objProduct);
                db.SaveChanges();
                TempData["success"] = "Product inserted success";
                return RedirectToAction("Index");
            }

            var lstCategory = db.Categories.Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() });
            ViewBag.LstCategory = lstCategory;

            return View();
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var deleteCategory = this.db.Products.Find(id);
            if (deleteCategory == null)
            {
                return RedirectToAction("Index");
            }
            this.db.Products.Remove(deleteCategory);
            this.db.SaveChanges();
            TempData["SUCCESS"] = "OK";
            return RedirectToAction("Index");
        }


        [HttpGet]
        public IActionResult Edit(int id)
        {
            var objproduct = db.Products.Find(id);
            ViewBag.LISTCATEGORY = db.Categories.Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() });
            return View(objproduct);
        }
        [HttpPost]
        public IActionResult Edit(Product Obj, IFormFile File)
        {

            if (ModelState.IsValid)
            {
                if (File != null)
                {
                    string path = Path.Combine(host.WebRootPath, @"img/Product");
                    string filename = Guid.NewGuid().ToString() + Path.GetExtension(File.FileName);
                    using (var filestream = new FileStream(Path.Combine(path, filename), FileMode.Create))
                    {
                        File.CopyTo(filestream);
                    }
                    if (!String.IsNullOrEmpty(Obj.ImageUrl))
                    {
                        var oldFilePath = Path.Combine(host.WebRootPath, Obj.ImageUrl);
                        if (System.IO.File.Exists(oldFilePath))
                        {
                            System.IO.File.Delete(oldFilePath);
                        }
                    }
                    Obj.ImageUrl = @"img/Product/" + filename;
                }
                db.Products.Update(Obj);
                db.SaveChanges();
                TempData["success"] = "Product Updated success";
                return RedirectToAction("Index");
            }
            ViewBag.LstCategory = db.Categories.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });
            return View(Obj);

        }
        [HttpGet]
        public IActionResult Detail(int Id)
        {
            var getProduct = db.Products.Find(Id);
            var idCategory= getProduct.CategoryId;
            List<Product> products = db.Products.Where(x => x.CategoryId == idCategory).ToList();
            ViewBag.products=products;
            return View(getProduct);
        }

    }
}

