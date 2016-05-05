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
            ViewBag.EmpData = this.GetInfo.GetEmp();
            ViewBag.CusData = this.GetInfo.GetCus();
            ViewBag.ShipperData = this.GetInfo.GetShipper();
            ViewBag.ProductData = this.GetInfo.GetProduct();
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
                OrderService.InsertOrder(order);
                return RedirectToAction("Index");

            }
            return View(order);
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
        /// <summary>
        /// 取得商品單價
        /// </summary>
        /// <param name="ProductID"></param>
        /// <returns></returns>
        [HttpPost()]
        public JsonResult GetPrice(string ProductID)
        {
            try
            {
                GetProductInfo.GetPrice(ProductID);
                return this.Json(true);
            }
            catch (Exception)
            {

                return this.Json(false);
            }


        }
    }
}