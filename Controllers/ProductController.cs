using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core_WebApp.Models;
using Core_WebApp.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Core_WebApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly IRepository<Product, int> repository;
        private readonly IRepository<Category, int> catRepository;

        public ProductController(IRepository<Product, int> repository, IRepository<Category, int> catRepository)
        {
            this.repository = repository;
            this.catRepository = catRepository;
        }

        // GET: Product
        public async Task<IActionResult> Index()
        {
            //var res = await repository.GetAsync();
            //ViewBag.CategoryRowId = new SelectList(await catRepository.GetAsync(), "CategoryRowId", "CategoryName");
            //return View(res);

            List<Product> products = new List<Product>();
            // read data from TempData
            int CatRowId = Convert.ToInt32(TempData["CategoryRowId"]);
            if (CatRowId != 0)
            {
                products = (from p in await repository.GetAsync()
                            where p.CategoryRowId == CatRowId
                            select p).ToList();
            }
            else
            {
                products = repository.GetAsync().Result.ToList();
            }
            TempData.Keep();
            return View(products); // return the Index View
        }

        // GET: Product/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Product/Create
        public async Task<ActionResult> Create()
        {
            var r = TempData["CategoryRowId"];
            var res = new Product();
            //res = TempData["FormData"];
            ViewBag.CategoryRowId = new SelectList(await catRepository.GetAsync(), "CategoryRowId", "CategoryName");

            return View(res);
        }

        // POST: Product/Create
        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            // We will later remove the try catch and handle the exceptions using filters at a generic level
            //try
            //{
            Product formData = product;
            TempData["FormData"] = product;
                // check for the validation
                if (ModelState.IsValid)
                {
                    if(product.ProductPrice < 0)
                    {
                        throw new Exception("Product price cannot be negative");
                    }
                    var res = await repository.CreateAsync(product);
                    return RedirectToAction("Index"); // return to the Index View
                }
                else
                {
                    ViewBag.CategoryRowId = new SelectList(await catRepository.GetAsync(),
                  "CategoryRowId", "CategoryName");
                    return View(product); // stey on create view with validation error messages
                }
            //}
            //catch (Exception ex)
            //{
            //    return View("Error", new ErrorViewModel()
            //    {
            //        ControllerName = this.RouteData.Values["controller"].ToString(),
            //        ActionName = this.RouteData.Values["action"].ToString(),
            //        ErrorMessage = ex.Message
            //    });
            //}
        }

        // GET: Product/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var res = await repository.GetAsync(id);

            ViewBag.CategoryRowId = new SelectList(await catRepository.GetAsync(), "CategoryRowId", "CategoryName", res.CategoryRowId);
            return View(res); // return view with data to be edited
        }

        // POST: Product/Edit/5
        [HttpPost]
        public async Task<IActionResult> Edit(int id, Product product)
        {
            // Check for the validation
            if (ModelState.IsValid)
            {
                var res = await repository.UpdateAsync(id, product);
                return RedirectToAction("Index");
            }
            return View(product); // stay on current view with validation error messages
        }

        // GET: Product/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var res = await repository.DeleteAsync(id);
            if (res) // delete successful
            {
                return RedirectToAction("Index");
            }
            return View("Index");
        }
    }
}