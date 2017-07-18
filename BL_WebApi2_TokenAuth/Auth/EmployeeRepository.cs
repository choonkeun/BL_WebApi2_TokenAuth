using System;
using System.Data;
using System.Data.SqlClient;
using BL_WebApi2_TokenAuth.Models;
using DL_ADONet_BASE;

namespace BL_WebApi2_TokenAuth.Auth
{
    public class EmployeeRepository
    {
        public static Employee GetEmployeeByUserName(string ConnString, string userName, string password)
        {
            using (SqlCommand cmd = new SqlCommand())
            {
                Employee employee = new Employee();
                string sql = string.Empty;
                sql += " select top 1 * from [employees] ";
                sql += " where lastName=@lastName AND firstName=@firstName;";

                cmd.CommandTimeout = 50;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sql;
                cmd.Parameters.AddWithValue("@lastName", userName);     //Davolio
                cmd.Parameters.AddWithValue("@firstName", password);    //Nancy
                DataTable dt = DataAccessLayer.GetDataTable(ConnString, cmd);
                foreach (DataRow dr in dt.Rows)
                {
                    employee = new Employee
                    {
                        EmployeeID = Convert.ToInt32(dr["EmployeeID"] == System.DBNull.Value ? 0 : dr["EmployeeID"]),
                        LastName = Convert.ToString(dr["LastName"] == System.DBNull.Value ? String.Empty : dr["LastName"]),
                        FirstName = Convert.ToString(dr["FirstName"] == System.DBNull.Value ? String.Empty : dr["FirstName"]),
                        Title = Convert.ToString(dr["Title"] == System.DBNull.Value ? String.Empty : dr["Title"]),
                        BirthDate = Convert.ToDateTime(dr["BirthDate"] == System.DBNull.Value ? DateTime.MinValue : dr["BirthDate"]),
                        HireDate = Convert.ToDateTime(dr["HireDate"] == System.DBNull.Value ? DateTime.MinValue : dr["HireDate"]),
                        Address = Convert.ToString(dr["Address"] == System.DBNull.Value ? String.Empty : dr["Address"]),
                        City = Convert.ToString(dr["City"] == System.DBNull.Value ? String.Empty : dr["City"]),
                        Region = Convert.ToString(dr["Region"] == System.DBNull.Value ? String.Empty : dr["Region"]),
                        PostalCode = Convert.ToString(dr["PostalCode"] == System.DBNull.Value ? String.Empty : dr["PostalCode"]),
                        Country = Convert.ToString(dr["Country"] == System.DBNull.Value ? String.Empty : dr["Country"]),
                        HomePhone = Convert.ToString(dr["HomePhone"] == System.DBNull.Value ? String.Empty : dr["HomePhone"]),
                        Extension = Convert.ToString(dr["Extension"] == System.DBNull.Value ? String.Empty : dr["Extension"]),
                        ReportsTo = Convert.ToInt32(dr["ReportsTo"] == System.DBNull.Value ? 0 : dr["ReportsTo"]),
                        Notes = Convert.ToString(dr["Notes"] == System.DBNull.Value ? String.Empty : dr["Notes"])
                    };
                }
                return employee;
            }
        }


    }
}