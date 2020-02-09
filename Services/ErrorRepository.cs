using Core_WebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core_WebApp.Services
{
    public class ErrorRepository : IRepository<ErrorLog, int>
    {
        private readonly AppDbContext ctx;
        public ErrorRepository(AppDbContext ctx)
        {
            this.ctx = ctx;
        }
        public async Task<ErrorLog> CreateAsync(ErrorLog entity)
        {
            var res = await ctx.ErrorLogs.AddAsync(entity); // append record in categories
            await ctx.SaveChangesAsync(); // commit transactions
            return res.Entity;
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ErrorLog>> GetAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ErrorLog> GetAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ErrorLog> UpdateAsync(int id, ErrorLog entity)
        {
            throw new NotImplementedException();
        }
    }
}
