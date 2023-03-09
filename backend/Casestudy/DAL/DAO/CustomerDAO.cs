using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Casestudy.DAL.DomainClasses;
using Microsoft.EntityFrameworkCore;

namespace Casestudy.DAL.DAO
{
    public class CustomerDAO
    {
        private AppDbContext _db;
        public CustomerDAO(AppDbContext ctx)
        {
            _db = ctx;
        }
        public async Task<Customer> Register(Customer cus)
        {
            await _db.Customers.AddAsync(cus);
            await _db.SaveChangesAsync();
            return cus;
        }
        public async Task<Customer> GetByEmail(string email)
        {
            Customer user = await _db.Customers.FirstOrDefaultAsync(u => u.Email == email);
            return user;
        }
    }
}
