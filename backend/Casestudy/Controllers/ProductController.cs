using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Casestudy.DAL.DAO;
using Casestudy.DAL.DomainClasses;
using Casestudy.DAL;
using Microsoft.AspNetCore.Authorization;

namespace Casestudy.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        AppDbContext _db;
        public ProductController(AppDbContext context)
        {
            _db = context;
        }
        [Route("{brandid}")]
        public async Task<ActionResult<List<Product>>> Index(int brandid)
        {
            ProductDAO dao = new ProductDAO(_db);
            List<Product> itemsForBrand = await dao.GetAllByBrand(brandid);
            return itemsForBrand;
        }

    }
}
