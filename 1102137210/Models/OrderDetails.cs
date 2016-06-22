using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _1102137210.Models
{
    public class OrderDetails
    {
        /// <summary>
        /// 訂單編號
        /// </summary>
        public int OrderID { get; set; }

        /// <summary>
        /// 產品代號
        /// </summary>
        public int ProductID { get; set; }

        /// <summary>
        /// 單價
        /// </summary>
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// 數量
        /// </summary>
        public short Qty { get; set; }

        /// <summary>
        /// 折扣
        /// </summary>
        public int Discount { get; set; }

        public string ProductName { get; set; }
    }
}