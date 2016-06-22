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
        /// 新增訂單頁面
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
                    return RedirectToAction("Index");
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
        /// <summary>
        /// 修改訂單頁面
        /// </summary>
        /// <param name="OrderID"></param>
        /// <returns></returns>
        public ActionResult UpdateOrder(string OrderID)
        {
            ViewBag.select1 = GetInfo.GetOrderInfo(OrderID);
            foreach (var item in (List<_1102137210.Models.Order>)ViewBag.select1)
            {
                ViewBag.orderid = item.OrderID;
                ViewBag.customer = item.CompanyName;
                ViewBag.customerid = item.CustomerID.ToString();
                ViewBag.lastname = item.Lastname;
                ViewBag.employeeid = item.EmployeeID.ToString();
                ViewBag.orderdate = item.OrderDate;
                ViewBag.requireddate = item.RequiredDate;
                ViewBag.shippeddate = item.ShippedDate;
                ViewBag.shippername = item.ShipperName;
                ViewBag.shipperid = item.ShipperID.ToString();
                ViewBag.freight = item.Freight;
                ViewBag.shipname = item.ShipName;
                ViewBag.shipaddress = item.ShipAddress;
                ViewBag.shipcity = item.ShipCity;
                ViewBag.shipregion = item.ShipRegion;
                ViewBag.shippostalcode = item.ShipPostalCode;
                ViewBag.shipcountry = item.ShipCountry;
            }
            List<SelectListItem> CustomersData = this.GetInfo.GetCus();
            ViewBag.Cusdata = CustomersData;
            ViewBag.EmpData = this.GetInfo.GetEmp();
            ViewBag.ShipperData = this.GetInfo.GetShipper();
            ViewBag.ProductData = this.GetInfo.GetProduct();
            
            List<SelectListItem> UnitPrice = this.GetProductInfo.GetPrice();
            ViewBag.PriceData = UnitPrice;
            ViewBag.orderdetail = GetInfo.GetOrderDetail(OrderID);

            List<SelectListItem> ProductsData = new List<SelectListItem>();
            List<List<SelectListItem>> getProductList = new List<List<SelectListItem>>();
            List<Products> result3 = GetInfo.GetProductName();
            for (int i = 0; i < ViewBag.orderdetail.Count; i++)
            {
                foreach (var item in result3)
                {
                    ProductsData.Add(new SelectListItem()
                    {
                        Text = item.ProductName,
                        Value = item.ProductID.ToString(),
                        Selected = item.ProductID.Equals(ViewBag.orderdetail[i].ProductID)
                    });
                    ViewData["ProductsData"] = ProductsData;
                }
                getProductList.Add(new List<SelectListItem>(ProductsData));
                ProductsData.Clear();
            }

            ViewBag.ProductData = getProductList;

            return View();
        }
        [HttpPost()]
        public ActionResult UpdateOrder(Models.Order update)
        {
            if (ModelState.IsValid)
            {

                try
                {
                    OrderService.DeleteOrderDetail(update.OrderID.ToString());
                    OrderService.UpdateOrder(update);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
            return RedirectToAction("UpdateOrder");
        }

    }
}