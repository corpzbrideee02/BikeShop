using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Casestudy.DAL.DomainClasses;
using Casestudy.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Casestudy.DAL.DAO
{
    public class OrderDAO
    {
        private AppDbContext _db;
        public OrderDAO(AppDbContext ctx)
        {
            _db = ctx;
        }
        public async Task<(int oId, bool backOrder)> AddOrder(int userid, OrderSelectionHelper[] selections)
        {
            int orderId = -1;
            bool isBackOrdered = false;
            List<bool> isBackOrdered2 = new List<bool>();
            using (_db)
            {
                // we need a transaction as multiple entities involved
                using (var _trans = await _db.Database.BeginTransactionAsync())
                {
                    try
                    {
                        Order order = new Order();
                        order.UserId = userid;
                        order.OrderDate = System.DateTime.Now;
                        order.OrderAmount = 0;

                        // calculate the totals
                        foreach (OrderSelectionHelper selection in selections)
                        {
                            order.OrderAmount += selection.item.MSRP * selection.Qty;
                        }
                       
                        await _db.Orders.AddAsync(order);
                        await _db.SaveChangesAsync();

                        foreach (OrderSelectionHelper selection in selections)
                        {
                            OrderLineItem olItem = new OrderLineItem();
                            Product product = selection.item;
                            olItem.ProductId = selection.item.Id;
                            olItem.OrderId = order.Id;
                            olItem.QtyOrdered = selection.Qty;

                             if (olItem.QtyOrdered<=product.QtyOnHand)
                            {
                                product.QtyOnHand -= selection.Qty;
                                olItem.QtySold = selection.Qty;
                                olItem.QtyOrdered = selection.Qty;
                                olItem.QtyBackOrdered = 0;
                                isBackOrdered2.Add(false);
                            }
                            else if(olItem.QtyOrdered>product.QtyOnHand)
                            {
                                product.QtyOnBackOrder += (selection.Qty - product.QtyOnHand);
                                olItem.QtySold = selection.item.QtyOnHand;
                                olItem.QtyOrdered = selection.Qty;
                                olItem.QtyBackOrdered = selection.Qty - selection.item.QtyOnHand;

                                product.QtyOnHand = 0;
                                isBackOrdered2.Add(true);
                            }

                            olItem.SellingPrice = selection.item.MSRP;
                            await _db.OrderLineItems.AddAsync(olItem);
                             _db.Products.Update(product);
                            await _db.SaveChangesAsync();
                        }

                        await _trans.CommitAsync();
                        orderId = order.Id;
                        if(isBackOrdered2.Contains(true))
                        {
                            isBackOrdered = true;
                        }
                        
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        await _trans.RollbackAsync();
                    }
                }
            }
            return (orderId, isBackOrdered);
        }

        public async Task<List<Order>> GetAll(int id)
        {
            return await _db.Orders.Where(order => order.UserId == id).ToListAsync<Order>();
        }
        public async Task<List<OrderDetailsHelper>> GetOrderDetails(int oid, string email)
        {
            Customer cus= _db.Customers.FirstOrDefault(cus => cus.Email == email);
            List<OrderDetailsHelper> allDetails = new List<OrderDetailsHelper>();

            // LINQ way of doing INNER JOINS
            var results = from t in _db.Orders
                          join ti in _db.OrderLineItems on t.Id equals ti.OrderId
                          join mi in _db.Products on ti.ProductId equals mi.Id
                          where (t.UserId == cus.Id && t.Id == oid)
                          select new OrderDetailsHelper
                          {
                               OrderId=t.Id,
                               CustomerId=cus.Id,
                              ProductId=ti.ProductId,
                              ProductName=mi.ProductName,
                              QtySold=ti.QtySold,
                              QtyOrdered=ti.QtyOrdered,
                              QtyBackOrdered=ti.QtyBackOrdered,
                              SellingPrice=ti.SellingPrice,
                              OrderAmount=t.OrderAmount,
                              OrderDate = t.OrderDate.ToString("yyyy/MM/dd - hh:mm tt")
                          };


            allDetails = await results.ToListAsync<OrderDetailsHelper>();
            return allDetails;
        }
    }
}