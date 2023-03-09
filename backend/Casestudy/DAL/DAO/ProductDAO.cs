using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Casestudy.DAL.DomainClasses;

namespace Casestudy.DAL.DAO
{
    public class ProductDAO
    {
        private AppDbContext _db;
        public ProductDAO(AppDbContext ctx)
        {
            _db = ctx;
        }
        public async Task<List<Product>> GetAllByBrand(int id)
        {
            return await _db.Products.Where(item => item.Brand.Id == id).ToListAsync();
        }
    }
}
