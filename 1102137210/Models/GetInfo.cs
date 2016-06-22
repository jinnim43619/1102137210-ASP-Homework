using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _1102137210.Models
{
    public class GetInfo
    {
        /// <summary>
        /// 取得DB連線
        /// </summary>
        /// <returns></returns>
        private string GetDBConnectionString()
        {
            return
                System.Configuration.ConfigurationManager.ConnectionStrings["DBConnectionString"].ConnectionString.ToString();
        }
        /// <summary>
        /// 取得員工資料
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> GetEmp()
        {
            DataTable dt = new DataTable();
            string sql = @"Select EmployeeID As CodeId,LastName+'-'+FirstName As CodeName FROM HR.Employees";
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                sqlAdapter.Fill(dt);
                conn.Close();
            }
            return this.MapCodeData(dt);
        }
        /// <summary>
        /// 取得出貨公司資料
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> GetShipper()
        {
            DataTable dt = new DataTable();
            string sql = @"Select ShipperID As CodeId,CompanyName As CodeName FROM Sales.Shippers";
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                sqlAdapter.Fill(dt);
                conn.Close();
            }
            return this.MapCodeData(dt);
        }
        /// <summary>
        /// 取得客戶資料
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> GetCus()
        {
            DataTable dt = new DataTable();
            string sql = @"Select CustomerID As CodeId,CompanyName As CodeName FROM Sales.Customers";
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                sqlAdapter.Fill(dt);
                conn.Close();
            }
            return this.MapCodeData(dt);
        }
        public List<SelectListItem> GetProduct()
        {
            DataTable dt = new DataTable();
            string sql = @"Select ProductId As CodeId,ProductName As CodeName From Production.Products";
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                sqlAdapter.Fill(dt);
                conn.Close();
            }
            return this.MapCodeData(dt);
        }
        /// <summary>
        /// 取得訂單資料
        /// </summary>
        /// <param name="OrderID"></param>
        /// <returns></returns>
        public List<Order> GetOrderInfo(string OrderID)
        {
            DataTable result = new DataTable();
            string sql = @"SELECT OrderID,b.CompanyName,b.CustomerID,
                            (d.LastName+'-'+d.FirstName) as Lastname,d.EmployeeID,
                            CONVERT(varchar(10) ,OrderDate, 23 ) AS OrderDate,
                            CONVERT(varchar(10) ,RequiredDate, 23 ) AS RequiredDate,
                            CONVERT(varchar(10) ,ShippedDate, 23 ) AS ShippedDate,
                            c.CompanyName as ShipperName,c.ShipperID,Freight,ShipName,ShipAddress,ShipCity,ShipRegion,ShipPostalCode,ShipCountry
                            FROM Sales.Orders AS a JOIN Sales.Customers AS b ON a.CustomerID=b.CustomerID JOIN Sales.Shippers AS c ON a.ShipperID=c.ShipperID 
                            JOIN HR.Employees AS d ON a.EmployeeID=d.EmployeeID 
                            WHERE a.OrderID=@OrderID";

            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand command = new SqlCommand(sql, conn);
                command.Parameters.Add(new SqlParameter("@OrderID", OrderID == null ? string.Empty : OrderID));
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(command);
                sqlAdapter.Fill(result);
                conn.Close();
            }

            return this.MapSelectOrderByID(result);
        }
        /// <summary>
        /// Maping 代碼資料(Order)
        /// </summary>
        /// <param name="SelectData"></param>
        /// <returns></returns>
        private List<Order> MapSelectOrderByID(DataTable SelectData)
        {
            List<Order> result = new List<Order>();


            foreach (DataRow row in SelectData.Rows)
            {
                result.Add(new Order()
                {
                    OrderID = (int)row["OrderID"],
                    CompanyName = row["CompanyName"].ToString(),
                    CustomerID = (int)row["CustomerID"],
                    Lastname = row["Lastname"].ToString(),
                    EmployeeID = (int)row["EmployeeID"],
                    OrderDate = row["OrderDate"].ToString(),
                    RequiredDate = row["RequiredDate"].ToString(),
                    ShippedDate = row["ShippedDate"].ToString(),
                    ShipperName = row["ShipperName"].ToString(),
                    ShipperID = (int)row["ShipperID"],
                    Freight = (decimal)row["Freight"],
                    ShipName = row["ShipName"].ToString(),
                    ShipAddress = row["ShipAddress"].ToString(),
                    ShipCity = row["ShipCity"].ToString(),
                    ShipRegion = row["ShipRegion"].ToString(),
                    ShipPostalCode = row["ShipPostalCode"].ToString(),
                    ShipCountry = row["ShipCountry"].ToString()
                });
            }
            return result;
        }
        /// <summary>
        /// Maping 代碼資料
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private List<SelectListItem> MapCodeData(DataTable dt)
        {
            List<SelectListItem> result = new List<SelectListItem>();

            result.Add(new SelectListItem()
            {
                Text = "",
                Value = null
            });
            foreach (DataRow row in dt.Rows)
            {
                result.Add(new SelectListItem()
                {
                    Text = row["CodeName"].ToString(),
                    Value = row["CodeId"].ToString()
                });
            }
            return result;
        }
        /// <summary>
        /// 取得訂單明細資料
        /// </summary>
        /// <param name="OrderID"></param>
        /// <returns></returns>
        public List<OrderDetails> GetOrderDetail(string OrderID)
        {
            DataTable result = new DataTable();
            string sql = @"select OrderID,ProductID,UnitPrice,Qty from Sales.OrderDetails where OrderID=@OrderID";
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand command = new SqlCommand(sql, conn);
                command.Parameters.Add(new SqlParameter("@OrderID", OrderID == null ? string.Empty : OrderID));
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(command);
                sqlAdapter.Fill(result);
                conn.Close();
            }
            return this.MapSelectOrderDetailByID(result);
        }
        /// <summary>
        /// Maping 代碼資料(OrderDetail)
        /// </summary>
        /// <param name="SelectData"></param>
        /// <returns></returns>
        private List<OrderDetails> MapSelectOrderDetailByID(DataTable SelectData)
        {
            List<OrderDetails> OrderDetails = new List<OrderDetails>();


            foreach (DataRow row in SelectData.Rows)
            {
                OrderDetails.Add(new OrderDetails()
                {
                    OrderID = (int)row["OrderID"],
                    ProductID = (int)row["ProductID"],
                    UnitPrice = (decimal)row["UnitPrice"],
                    Qty = (short)row["Qty"],
                });
            }
            return OrderDetails;
        }
        public List<Products> GetProductName()
        {
            DataTable result = new DataTable();
            string sql = @"SELECT ProductID,ProductName FROM Production.Products ORDER BY ProductName";

            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlDataAdapter sqlAdapter = new SqlDataAdapter();
                sqlAdapter.SelectCommand = new SqlCommand(sql, conn);
                sqlAdapter.Fill(result);
                conn.Close();

            }
            return this.MapProductIDList(result);
        }
        private List<Products> MapProductIDList(DataTable orderData)
        {
            List<Products> result = new List<Products>();


            foreach (DataRow row in orderData.Rows)
            {
                result.Add(new Products()
                {
                    ProductID = (int)row["ProductID"],
                    ProductName = row["ProductName"].ToString()
                });
            }
            return result;
        }

    }
}