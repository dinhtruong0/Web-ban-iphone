using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using C21_TH1_Lu_van_Duong.Models;
using C21_TH1_Lu_van_Duong.Data;

namespace C21_TH1_Lu_van_Duong.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;
        public CategoryController(ApplicationDbContext db)
        {

            _db = db;
        }
        public IActionResult Index()
        {

            var dstheloai = _db.Categories.ToList();
            return View(dstheloai);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                this._db.Categories.Add(category);
                this._db.SaveChanges();
                return RedirectToAction("Index");
            }


            return View();
        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var deleteCategory = this._db.Categories.Find(id);
            if (deleteCategory == null)
            {
                return RedirectToAction("Index");
            }
            this._db.Categories.Remove(deleteCategory);
            this._db.SaveChanges();
            TempData["SUCCESS"] = "OK";
            return RedirectToAction("Index");
        }


        [HttpGet]
        public IActionResult Edit(int id)
        {
            var objCategory = _db.Categories.Find(id);
            if (objCategory == null)
            {
                return NotFound();
            }
            return View(objCategory);
        }
        [HttpPost]
        public IActionResult Edit(Category objCategory)
        {
            if (ModelState.IsValid)
            {
                _db.Categories.Update(objCategory);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}
