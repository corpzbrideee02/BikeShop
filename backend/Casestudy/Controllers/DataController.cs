using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Casestudy.DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Casestudy.DAL.DAO;

namespace Casestudy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataController : ControllerBase
    {
        AppDbContext _ctx;
        IWebHostEnvironment _env;

        public DataController(AppDbContext context, IWebHostEnvironment env) //injected here
        {
            _ctx = context;
            _env = env;
        }
        public async Task<ActionResult<String>> Index()
        {
            DataUtility util = new DataUtility(_ctx);
            string payload = "";
            var json = await getProductJsonFromWebAsync();
            try
            {
                payload = (await util.loadProductInfoFromWebToDb(json)) ? "tables loaded" : "problem loading tables";
            }
            catch (Exception ex)
            {
                payload = ex.Message;
            }
            return JsonSerializer.Serialize(payload);
        }

        private async Task<String> getProductJsonFromWebAsync()
        {
            string url = "https://corpzbrideee02.github.io/data/product.json"; 
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(url);
            var result = await response.Content.ReadAsStringAsync();
            return result;
        }
        /*[Route("loadbranches")]
        public async Task<ActionResult<String>> LoadBranches()
        {
            string payload = "";
            BranchDAO dao = new BranchDAO(_ctx);
            bool storesLoaded = await dao.LoadBranchFromFile(_env.WebRootPath);
            try
            {
                payload = storesLoaded ? "branch loaded successfully" : "problem loading branch data";
            }
            catch (Exception ex)
            {
                payload = ex.Message;
            }
            return JsonSerializer.Serialize(payload);
        }*/
    }
}
