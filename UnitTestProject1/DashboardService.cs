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


            internal static List<DashboardTotalOrderItem> GetMonthlyCancelledDeliveriedReturn(DateTime fromDate, DateTime toDate)
            {
                var sql = string.Format(@"
                SELECT datepart(WEEK, od.OrderedAt) AS WeekNumber,
                       SUM(CASE
                               WHEN od.OrderStatus = 'DELIVERY' THEN 1
                               ELSE 0
                               END) AS TotalDelivery,
                       SUM(CASE
                               WHEN od.OrderStatus = 'FAILED' THEN 1
                               ELSE 0
                               END) AS TotalCancel,
                       SUM(CASE
                               WHEN re.ReturnStatus = 'APPROVED' THEN 1
                               ELSE 0
                               END) AS TotalReturn
                FROM OrderList AS od
                LEFT JOIN ReturnList AS re ON od.Id = re.OrderId
                WHERE CAST(od.orderedAt AS DATE) BETWEEN '{0}' AND '{1}'
                GROUP BY datepart(WEEK, od.OrderedAt)
                ", fromDate, toDate);
                var reader = SqlHelper.ExecuteReader(_connectionString, sql, CommandType.Text);
                var result = new List<DashboardTotalOrderItem>();
                while (reader.Read())
                {
                    result.Add(new DashboardTotalOrderItem
                    {
                        WeekNumber = Convert.ToInt32(reader[0]),
                        TotalDelivery = Convert.ToInt32(reader[1]),
                        TotalCancel = Convert.ToInt32(reader[2]),
                        TotalReturn = Convert.ToInt32(reader[3])
                    });
                }
                return result;
            }

            internal static List<DashboardSaleByLocationItem> GetSalesAnalyticsByCountryMonthly()
            {
                var sql = string.Format(@"
                   SELECT month(ord.OrderedAt) AS MONTH,
                          year(ord.OrderedAt) AS YEAR,
                          ctr.CountryName,
                          SUM(ord.TotalPrice) AS TotalPrice
                    FROM Country AS ctr
                    JOIN Channel AS chn ON ctr.Id = chn.CountryId
                    JOIN OrderList AS ord ON chn.Id = ord.ChannelId
                    GROUP BY month(ord.OrderedAt),
                             year(ord.OrderedAt),
                             ctr.CountryName
                    ORDER BY year(ord.OrderedAt),
                             month(ord.OrderedAt)");

                var reader = SqlHelper.ExecuteReader(_connectionString, sql, CommandType.Text);
                var result = new List<DashboardSaleByLocationItem>();

                while (reader.Read())
                {
                    result.Add(new DashboardSaleByLocationItem
                    {
                        Year = Convert.ToInt32(reader[0]),
                        Month = Convert.ToInt32(reader[1]),
                        Location = Convert.ToString(reader[2]),
                        Total = Convert.ToInt64(reader[3])
                    });
                }
                return result;
            }

            internal static List<DashboardItem> GetTotalProductByCatalog(DateTime fromDate, DateTime toDate, string productName)
            {
                var sql = string.Format(@"
                   SELECT CAST(ord.OrderedAt AS date),
                          SUM(ord.TotalPrice) AS Total
                   FROM OrderList AS ord
                   JOIN OrderDetail AS odt ON ord.Id = odt.OrderId
                   JOIN Product AS prd ON odt.ProductId = prd.Id
                   AND prd.ProductName LIKE '%{0}%'
                   WHERE CASE(ord.OrderedAt AS Date) BETWEEN '{1}' AND '{2}'
                   GROUP BY ord.OrderedAt
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