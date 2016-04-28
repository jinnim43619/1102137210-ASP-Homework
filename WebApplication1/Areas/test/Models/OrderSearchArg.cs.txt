using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eSale.Models
{
    public class OrderSearchArg
    {
        public string CustName { get; set; }
        public string OrderDate { get; set; }
        public string EmpId { get; set; }
        public string DeleteOrderId { get; set; }
    }
}