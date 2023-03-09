using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Casestudy.DAL;
using Casestudy.DAL.DAO;
using Casestudy.DAL.DomainClasses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Casestudy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BranchController : ControllerBase
    {
        AppDbContext _db;
        public BranchController(AppDbContext context)
        {
            _db = context;
        }
        [HttpGet("{lat}/{lon}")]
        public async Task<ActionResult<List<Branch>>> Index(float lat, float lon)
        {
            BranchDAO dao = new BranchDAO(_db);
            return await dao.GetThreeClosestBranches(lat, lon);
        }
    }
}
