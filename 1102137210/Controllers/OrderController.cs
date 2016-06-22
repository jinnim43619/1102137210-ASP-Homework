using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using _1102137210.Models;

namespace _1102137210.Controllers
{
    public class OrderController : Controller
    {
        Models.GetInfo GetInfo = new Models.GetInfo();
        Models.OrderService OrderService = new Models.OrderService();
        Models.GetProductInfo GetProductInfo = new Models.GetProductInfo();

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
        /// <summary>
        /// 新增訂單
        /// </summary>
        /// <returns></returns>
        public ActionResult InsertOrder()
        {
            List<SelectListItem> CustomersData = this.GetInfo.GetCus();
            ViewBag.Cusdata = CustomersData;
            ViewBag.EmpData = this.GetInfo.GetEmp();
            ViewBag.ShipperData = this.GetInfo.GetShipper();
            ViewBag.ProductData = this.GetInfo.GetProduct();
            List<SelectListItem> UnitPrice = this.GetProductInfo.GetPrice();
            ViewBag.PriceData = UnitPrice;
            return View(new Models.Order());
        }
        /// <summary>
        /// 新增訂單結果
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        [HttpPost()]
        public ActionResult InsertOrder(Models.Order order)
        {
            if (ModelState.IsValid)
            {

                try
                {
                    OrderService.InsertOrder(order);
                    return RedirectToAction("InsertOrder");
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
            return RedirectToAction("InsertOrder");
        }
            
        /// <summary>
        /// 刪除訂單
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpPost()]
        public JsonResult DeleteOrder(string DeleteOrderId)
        {
            try
            {
                OrderService.DeleteOrderById(DeleteOrderId);
                return this.Json(true);
            }
            catch (Exception)
            {

                return this.Json(false);
            }


        }
    }
}