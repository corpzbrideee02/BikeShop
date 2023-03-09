using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Casestudy.Helpers
{
    public class OrderDetailsHelper
    {
       /* public int TrayId { get; set; }
        public int MenuItemId { get; set; }
        public int Qty { get; set; }
        public string Description { get; set; }
        public int UserId { get; set; }
        public string DateCreated { get; set; }
        public int TotalCalories { get; set; }
        public int TotalCarbs { get; set; }
        public int TotalCholesterol { get; set; }
        public decimal TotalFat { get; set; }
        public int TotalFibre { get; set; }
        public int TotalProtein { get; set; }
        public int TotalSalt { get; set; }*/

        public int OrderId { get; set; }
        public string ProductId { get; set; }
        public int CustomerId { get; set; }
        public string ProductName { get; set; }
        public int QtySold { get; set; }
        public int QtyBackOrdered { get; set; }
        public int QtyOrdered { get; set; }
        public decimal SellingPrice { get; set; }
        public decimal OrderAmount { get; set; }
        public string OrderDate { get; set; }
    }
}
