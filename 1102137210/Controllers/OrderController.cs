using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _1102137210.Controllers
{
    public class OrderController : Controller
    {
        Models.GetInfo GetInfo = new Models.GetInfo();
        // GET: Order
        /// <summary>
        /// 訂單首頁
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ViewBag.EmpData = this.GetInfo.GetEmp();
            ViewBag.ShipperData = this.GetInfo.GetShipper();
            return View();
        }
        /// <summary>
        /// 取得訂單結果
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        [HttpPost()]
        public ActionResult Index(Models.OrderSearchCondition a)
        {
            ViewBag.EmpData = this.GetInfo.GetEmp();
            ViewBag.ShipperData = this.GetInfo.GetShipper();
            Models.OrderService OrderService = new Models.OrderService();
            ViewBag.Order = OrderService.GetOrderByCondtioin(a);
            return View("Index");
        }
        public ActionResult InsertOrder()
        {
            ViewBag.EmpData = this.GetInfo.GetEmp();
            ViewBag.CusData = this.GetInfo.GetCus();
            ViewBag.ShipperData = this.GetInfo.GetShipper();
            return View();
        }
    }
}