using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _1102137210.Models
{
    public class OrderSearchCondition
    {
        public string OrderID { get; set; }
        public string CompanyName { get; set; }
        public string EmployeeID { get; set; }
        public string ShipperID { get; set; }
        public string OrderDate { get; set; }
        public string ShippedDate { get; set; }
        public string RequiredDate { get; set; }
    }
}