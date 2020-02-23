using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Core_WebApp.Models;
using Core_WebApp.Services;

namespace Core_WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController] // Attribute that is used to map the POSTED for, data from, Body to CLR object
    public class CategoryAPIController : ControllerBase
    {
        private readonly IRepository<Category, int> _catRepository;
        public CategoryAPIController(IRepository<Category, int> catRepository)
        {
            this._catRepository = catRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            var cats = await _catRepository.GetAsync();
            return Ok(cats);
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync(Category cat)
        {
            if (ModelState.IsValid)
            {
                cat = await _catRepository.CreateAsync(cat);
                return Ok(cat);
            }
            return BadRequest(ModelState);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(int id, Category cat)
    {
        if (ModelState.IsValid)
        {
            cat = await _catRepository.UpdateAsync(id, cat);
            return Ok(cat);
        }
        return BadRequest(ModelState);
    }

    [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var res = await _catRepository.DeleteAsync(id);
            return Ok(res);            
        }
    }
}