using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core_WebApp.Models;
using Core_WebApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace Core_WebApp.Controllers
{
    /// <summary>
    /// Dependency Inject the Repository classes
    /// Controller is a base class for 
    /// </summary>
    public class CategoryController : Controller
    {
        private readonly IRepository<Category, int> repository;

        public CategoryController(IRepository<Category, int> repository)
        {
            this.repository = repository;
        }

        public async Task<IActionResult> Index()
        {
            var res = await repository.GetAsync();
            return View(res);
        }

        // test
        public async Task<IActionResult> Create()
        {
            return View(new Category());
        }

        [HttpPost]
        public async Task<IActionResult> Create(Category category)
        {
            // Check for the validation
            if (ModelState.IsValid)
            {
                var res = await repository.CreateAsync(category);
                return RedirectToAction("Index");
            }
            return View(category); // stay on current view with validation error messages
        }

        public async Task<IActionResult> Edit(int id)
        {
            var res = await repository.GetAsync(id);
            return View(res); // return view with data to be edited
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Category category)
        {
            // Check for the validation
            if (ModelState.IsValid)
            {
                var res = await repository.UpdateAsync(id, category);
                return RedirectToAction("Index");
            }
            return View(category); // stay on current view with validation error messages
        }

        public async Task<IActionResult> Delete(int id)
        {
            var res = await repository.DeleteAsync(id);
            if (res) // delete successful
            {
                return RedirectToAction("Index");
            }
            return View("Index"); 
        }

        public IActionResult ShowProducts(int id)
        {
            TempData["CategoryRowId"] = id;
            return RedirectToAction("Index", "Product"); // return to the Index View from ProductController
        }
    }
}