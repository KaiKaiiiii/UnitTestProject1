using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using static DataLogic;

namespace UnitTestProject1
{
    public partial class UnitTest1
    {
        public static class DashboardService
        {
            private static readonly string _connectionString = @"Data Source=.;Initial Catalog=OMSDb;Integrated Security=True";

            internal static List<DashboardTotalOrderItem> GetMonthlyCancelledDeliveriedReturn(DateTime fromDate, DateTime toDate)
            {
                var sql = string.Format(@"
               SELECT COUNT(CASE
                            WHEN od.OrderStatus = 'DELIVERY' THEN 1
                            END) AS TotalDelivery,
                      COUNT(CASE
                            WHEN od.OrderStatus = 'FAILED' THEN 1
                            END) AS TotalCancel,
                      COUNT(CASE
                            WHEN re.ReturnStatus = 'APPROVED' THEN 1
                            END) AS TotalReturn
               FROM [Order] AS od,
                    [Return] AS re
               WHERE od.OrderDate BETWEEN '{0}' AND '{1}'
                ", fromDate, toDate);
                var reader = SqlHelper.ExecuteReader(_connectionString, sql, CommandType.Text);
                var result = new List<DashboardTotalOrderItem>();
                while (reader.Read())
                {
                    result.Add(new DashboardTotalOrderItem
                    {
                        TotalDelivery = reader[0],
                        TotalCancel = reader[1],
                        TotalReturn = reader[2]
                    });
                }
                return result;
            }

            internal static List<DashboardSaleByLocationItem> GetSalesAnalyticsByCountryMonthly()
            {
                var sql = string.Format(@"
                SELECT month(ord.OrderDate) AS MONTH,
                       ctr.name,
                       SUM(ord.TotalPrice) AS TotalPrice
                FROM [Country] AS ctr
                JOIN [Channel] AS chn ON ctr.Id = chn.CountryId
                JOIN [Order] AS ord ON chn.Id = ord.ChannelId
                GROUP BY month(ord.OrderDate),
                         ctr.name
                ORDER BY month(ord.OrderDate)");

                var reader = SqlHelper.ExecuteReader(_connectionString, sql, CommandType.Text);
                var result = new List<DashboardSaleByLocationItem>();

                while (reader.Read())
                {
                    result.Add(new DashboardSaleByLocationItem
                    {
                        Year = Int32.Parse(reader[0]+""),
                        Month = Int32.Parse(reader[1] + ""),
                        Location = Convert.ToString(reader[2]),
                        Total = long.Parse(reader[3] + "")
                    }); ;
                }
                return result;
            }

            internal static List<DashboardItem> GetTotalProductByCatalog(DateTime fromDate, DateTime toDate, string productName)
            {
                var sql = string.Format(@"
               SELECT ord.OrderDate,
                      SUM(ord.TotalPrice) AS Total
               FROM [Order] AS ord
               JOIN [OrderDetail] AS odt ON ord.Id = odt.OrderId
               JOIN [Product] AS prd ON odt.ProductId = prd.Id
               AND prd.ProductName LIKE '%{0}%'
               WHERE ord.OrderDate BETWEEN '{1}' AND '{2}'
               GROUP BY ord.OrderDate
                ", productName, fromDate, toDate);
                return GetDashboardItemByConditions(sql);
            }


            private static List<DashboardItem> GetDashboardItemByConditions(string sql)
            {
                var reader = SqlHelper.ExecuteReader(_connectionString, sql, CommandType.Text);

                var result = new List<DashboardItem>();

                while (reader.Read())
                {
                    var item = new DashboardItem
                    {
                        DisplayText = reader[0] + "",
                        Total = long.Parse(reader[1] + ""),
                    };

                    result.Add(item);
                }
                return result;
            }

            static long GetTotalByConditionsDate(string sql, DateTime fromDate, DateTime toDate)
            {
                var respond = SqlHelper.ExecuteScalar(_connectionString, sql, CommandType.Text);

                long result;
                long.TryParse(respond + "", out result);

                return result;
            }
        }
    }


}