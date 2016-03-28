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
		public List<Orders> GetOrderByCondtioin(Models.OrderSearchCondition a)
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

        private List<Models.Orders> MapOrderDataToList(DataTable orderData)
        {
            List<Models.Orders> result = new List<Orders>();


            foreach (DataRow row in orderData.Rows)
            {
                result.Add(new Orders()
                {
                    OrderID = (int)row["OrderId"],
                    OrderDate= row["OrderDate"].ToString(),
                    RequiredDate = row["RequiredDate"].ToString(),
                    ShippedDate = row["ShippedDate"].ToString(),
                    EmpName = row["EmpName"].ToString(),
                    ShipperName= row["ShipperName"].ToString(),
                    CompanyName=row["CompanyName"].ToString()

                });
            }
            return result;
        }
    }
}