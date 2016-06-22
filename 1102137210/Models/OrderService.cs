using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace _1102137210.Models
{
    public class OrderService
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
		/// 依照條件取得訂單資料
		/// </summary>
		/// <returns></returns>
		public List<Order> GetOrderByCondtioin(Models.OrderSearchCondition a)
        {

            DataTable dt = new DataTable();
            string sql = @"Select A.OrderID,
                    B.CompanyName,
                    C.LastName+'-'+C.FirstName As EmpName,
					CONVERT(VARCHAR(10),A.OrderDate,111) As OrderDate,
                    CONVERT(VARCHAR(10),A.RequiredDate,111) As RequiredDate,
                    CONVERT(VARCHAR(10),A.ShippedDate,111) As ShippedDate,
                    D.CompanyName As ShipperName
					From Sales.Orders As A 
				    JOIN Sales.Customers As B ON A.CustomerID=B.CustomerID
					JOIN HR.Employees As C On A.EmployeeID=C.EmployeeID
					JOIN Sales.Shippers As D ON A.ShipperID=D.ShipperID
                    Where (A.OrderID=@OrderID Or @OrderID='')
                    And(B.CompanyName Like '%'+@CompanyName+'%' Or @CompanyName='')
                    And(C.EmployeeID=@EmployeeID Or @EmployeeID='')
                    And(A.ShipperID=@ShipperID Or @ShipperID='')
                    And(OrderDate >= @OrderDate AND OrderDate < DATEADD(DAY,1,@OrderDate) OR @OrderDate = '')
                    And(RequiredDate >= @RequiredDate AND RequiredDate < DATEADD(DAY,1,@RequiredDate) OR @RequiredDate = '')
                    And(ShippedDate >= @ShippedDate AND ShippedDate < DATEADD(DAY,1,@ShippedDate) OR @ShippedDate = '')";



            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@OrderID", a.OrderID == null ? string.Empty : a.OrderID));
                cmd.Parameters.Add(new SqlParameter("@CompanyName", a.CompanyName == null ? string.Empty : a.CompanyName));
                cmd.Parameters.Add(new SqlParameter("@EmployeeID", a.EmployeeID == null ? string.Empty : a.EmployeeID));
                cmd.Parameters.Add(new SqlParameter("@ShipperID", a.ShipperID == null ? string.Empty : a.ShipperID));
                cmd.Parameters.Add(new SqlParameter("@OrderDate", a.OrderDate == null ? string.Empty : a.OrderDate));
                cmd.Parameters.Add(new SqlParameter("@ShippedDate", a.ShippedDate == null ? string.Empty : a.ShippedDate));
                cmd.Parameters.Add(new SqlParameter("@RequiredDate", a.RequiredDate == null ? string.Empty : a.RequiredDate));
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                sqlAdapter.Fill(dt);
                conn.Close();
            }


            return this.MapOrderDataToList(dt);
        }

        private List<Models.Order> MapOrderDataToList(DataTable orderData)
        {
            List<Models.Order> result = new List<Order>();


            foreach (DataRow row in orderData.Rows)
            {
                result.Add(new Order()
                {
                    OrderID = (int)row["OrderID"],
                    OrderDate= row["OrderDate"].ToString(),
                    RequiredDate = row["RequiredDate"].ToString(),
                    ShippedDate = row["ShippedDate"].ToString(),
                    EmpName = row["EmpName"].ToString(),
                    ShipperName = row["ShipperName"].ToString(),
                    CompanyName=row["CompanyName"].ToString()

                });
            }
            return result;
        }
        /// <summary>
        /// 刪除訂單
        /// </summary>
        /// <param name="DeleteOrderId"></param>
        public void DeleteOrderById(string DeleteOrderId)
        {
            try
            {
                string sql = "Delete FROM Sales.OrderDetails Where OrderID=@DeleteOrderId";
                using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.Add(new SqlParameter("@DeleteOrderId", DeleteOrderId));
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            try
            {
                string sql = "Delete FROM Sales.Orders Where OrderID=@DeleteOrderId";
                using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.Add(new SqlParameter("@DeleteOrderId", DeleteOrderId));
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        /// <summary>
        /// 新增訂單
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public int InsertOrder(Models.Order order)
        {
            string sql = @"INSERT INTO Sales.Orders
                           (CustomerID,EmployeeID,OrderDate,RequiredDate,ShippedDate,ShipperID,Freight,ShipName,ShipAddress,ShipCity,ShipRegion,ShipPostalCode,ShipCountry) 
                           VALUES (@CustomerID,@EmployeeID,@OrderDate,@RequiredDate,@ShippedDate,@ShipperID,@Freight,@ShipName,@ShipAddress,@ShipCity,@ShipRegion,@ShipPostalCode,@ShipCountry)
                           Select SCOPE_IDENTITY()";
            string sql2 = @"INSERT INTO Sales.OrderDetails
                           (OrderID,ProductID,UnitPrice,Qty) 
                           VALUES (@OrderID,@ProductID,@UnitPrice,@Qty)";
            string OrderID;
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand command = new SqlCommand(sql, conn);
                SqlCommand command2 = new SqlCommand(sql2, conn);
                command.Parameters.Add(new SqlParameter("@CustomerID", order.CustomerID));
                command.Parameters.Add(new SqlParameter("@EmployeeID", order.EmployeeID));
                command.Parameters.Add(new SqlParameter("@OrderDate", order.OrderDate));
                command.Parameters.Add(new SqlParameter("@RequiredDate", order.RequiredDate));
                command.Parameters.Add(new SqlParameter("@ShippedDate", order.ShippedDate));
                command.Parameters.Add(new SqlParameter("@ShipperID", order.ShipperID));
                command.Parameters.Add(new SqlParameter("@Freight", order.Freight));
                command.Parameters.Add(new SqlParameter("@ShipName", order.ShipName));
                command.Parameters.Add(new SqlParameter("@ShipAddress", order.ShipAddress));
                command.Parameters.Add(new SqlParameter("@ShipCity", order.ShipCity));
                command.Parameters.Add(new SqlParameter("@ShipRegion", order.ShipRegion));
                command.Parameters.Add(new SqlParameter("@ShipPostalCode", order.ShipPostalCode));
                command.Parameters.Add(new SqlParameter("@ShipCountry", order.ShipCountry));

                OrderID = command.ExecuteScalar().ToString();

                for (int i = 0; i < order.OrderDetails.Count; i++)
                {
                    command2 = new SqlCommand(sql2, conn);
                    command2.Parameters.Add(new SqlParameter("@OrderID",OrderID));
                    command2.Parameters.Add(new SqlParameter("@ProductID", order.OrderDetails[i].ProductID));
                    command2.Parameters.Add(new SqlParameter("@UnitPrice", order.OrderDetails[i].UnitPrice));
                    command2.Parameters.Add(new SqlParameter("@Qty", order.OrderDetails[i].Qty));
                    command2.ExecuteNonQuery();
                }


                conn.Close();
            }
            return Int32.Parse(OrderID);

        }
        public void DeleteOrderDetail(string OrderID)
        {
            try
            {
                string sql = "DELETE FROM Sales.OrderDetails WHERE OrderID=@OrderID";
                using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
                {
                    conn.Open();
                    SqlCommand command = new SqlCommand(sql, conn);
                    command.Parameters.Add(new SqlParameter("@OrderID", OrderID));
                    command.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string UpdateOrder(Models.Order update)
        {
            string sql = "update Sales.Orders set CustomerID=@CustomerID,EmployeeID=@EmployeeID,OrderDate=@OrderDate,RequiredDate=@RequiredDate,ShippedDate=@ShippedDate,ShipperID=@ShipperID, Freight=@Freight,ShipName=@ShipName,ShipAddress=@ShipAddress,ShipCity=@ShipCity,ShipRegion= @ShipRegion,ShipPostalCode= @ShipPostalCode,ShipCountry= @ShipCountry where OrderID =@OrderID";
            string sql2 = @"INSERT INTO Sales.OrderDetails
                           (OrderID,ProductID,UnitPrice,Qty) 
                           VALUES (@OrderID,@ProductID,@UnitPrice,@Qty)";
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand command = new SqlCommand(sql, conn);
                SqlCommand command2 = new SqlCommand(sql2, conn);
                command.Parameters.Add(new SqlParameter("@OrderID", update.OrderID));
                command.Parameters.Add(new SqlParameter("@CustomerID", update.CustomerID));
                command.Parameters.Add(new SqlParameter("@EmployeeID", update.EmployeeID));
                command.Parameters.Add(new SqlParameter("@OrderDate", update.OrderDate));
                command.Parameters.Add(new SqlParameter("@RequiredDate", update.RequiredDate));
                command.Parameters.Add(new SqlParameter("@ShippedDate", update.ShippedDate));
                command.Parameters.Add(new SqlParameter("@ShipperID", update.ShipperID));
                command.Parameters.Add(new SqlParameter("@Freight", update.Freight));
                command.Parameters.Add(new SqlParameter("@ShipName", update.ShipName));
                command.Parameters.Add(new SqlParameter("@ShipAddress", update.ShipAddress));
                command.Parameters.Add(new SqlParameter("@ShipCity", update.ShipCity));
                command.Parameters.Add(new SqlParameter("@ShipRegion", update.ShipRegion));
                command.Parameters.Add(new SqlParameter("@ShipPostalCode", update.ShipPostalCode));
                command.Parameters.Add(new SqlParameter("@ShipCountry", update.ShipCountry));
                command.ExecuteNonQuery();
                for (int i = 0; i < update.OrderDetails.Count; i++)
                {
                    command2 = new SqlCommand(sql2, conn);
                    command2.Parameters.Add(new SqlParameter("@OrderID", update.OrderID));
                    command2.Parameters.Add(new SqlParameter("@ProductID", update.OrderDetails[i].ProductID));
                    command2.Parameters.Add(new SqlParameter("@UnitPrice", update.OrderDetails[i].UnitPrice));
                    command2.Parameters.Add(new SqlParameter("@Qty", update.OrderDetails[i].Qty));
                    command2.ExecuteNonQuery();
                }
                conn.Close();
            }
            return null;
        }

    }
}