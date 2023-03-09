using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Casestudy.DAL;
using Casestudy.DAL.DAO;
using Casestudy.DAL.DomainClasses;
using Casestudy.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace Casestudy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        AppDbContext _ctx;
        public OrderController(AppDbContext context) // injected here
        {
            _ctx = context;
        }
        [HttpPost]
        [Produces("application/json")]
        public async Task<ActionResult<string>> Index(OrderHelper helper)
        {
            string retVal = "";
            try
            {
                CustomerDAO cDao = new CustomerDAO(_ctx);
                Customer orderOwner = await cDao.GetByEmail(helper.email);
                OrderDAO oDao = new OrderDAO(_ctx);
                (int orderId,bool isBackOrdered) = await oDao.AddOrder(orderOwner.Id, helper.selections);
                if ((orderId > 0) && isBackOrdered)
                {
                    retVal = "Order " + orderId + " created! \t Goods BackOrdered!";
                }
                else if(orderId > 0)
                {
                    retVal = "Order " + orderId + " created!";
                }
                else
                {
                    retVal = "Order not saved";
                }
            }
            catch (Exception ex)
            {
                retVal = "Order not saved " + ex.Message;
            }
            return retVal;
        }

        [Route("{email}")]
        public async Task<ActionResult<List<Order>>> List(string email)
        {
            List<Order> orderList = new List<Order>();
            CustomerDAO cDao = new CustomerDAO(_ctx);
            Customer orderOwner = await cDao.GetByEmail(email);
            OrderDAO oDao = new OrderDAO(_ctx);
            orderList = await oDao.GetAll(orderOwner.Id);
            return orderList;
        }

        [Route("{orderId}/{email}")]
        public async Task<ActionResult<List<OrderDetailsHelper>>> GetTrayDetails(int orderid, string email)
        {
            OrderDAO dao = new OrderDAO(_ctx);
            return await dao.GetOrderDetails(orderid, email);
        }
    }
}
