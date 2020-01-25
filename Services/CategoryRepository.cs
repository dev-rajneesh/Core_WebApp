using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core_WebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace Core_WebApp.Services
{
    public class CategoryRepository : IRepository<Category, int>
    {
        private readonly AppDbContext ctx;

        public CategoryRepository(AppDbContext ctx)
        {
            this.ctx = ctx;
        }
         
        public async Task<Category> CreateAsync(Category entity)
        {
            var res = await ctx.Categories.AddAsync(entity); // append record in categories
            await ctx.SaveChangesAsync(); // commit transactions
            return res.Entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var res = await ctx.Categories.FindAsync(id);
            if(res != null)
            {
                ctx.Categories.Remove(res);
                await ctx.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<IEnumerable<Category>> GetAsync()
        {
            return await ctx.Categories.ToListAsync();
        }

        public async Task<Category> GetAsync(int id)
        {
            return await ctx.Categories.FindAsync(id);
        }

        public async Task<Category> UpdateAsync(int id, Category entity)
        {
            var res = await ctx.Categories.FindAsync(id);
            if(res != null)
            {
                res.CategoryId = entity.CategoryId;
                res.CategoryName = entity.CategoryName;
                res.BasePrice = entity.BasePrice;
                
                await ctx.SaveChangesAsync();
                return res;
            }
            return res;
        }
    }
}
