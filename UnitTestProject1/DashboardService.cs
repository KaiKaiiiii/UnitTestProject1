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
            private static readonly string _connectionString = @"...";
            public static long GetTotalReturn(DateTime fromDate, DateTime toDate)
            {
                var sql = string.Format(@"SELECT SUM(...) FROM ... WHERE OrderDate BETWEEN  '{0}' and '{1}'", fromDate, toDate);

                return GetTotalByConditionsDate(sql, fromDate, toDate);
            }

            public static long GetTotalSales (DateTime fromDate, DateTime toDate)
            {
                var sql = string.Format(@"SELECT SUM(...) FROM ... WHERE OrderDate BETWEEN  '{0}' and '{1}'", fromDate, toDate);

                return GetTotalByConditionsDate(sql, fromDate, toDate);
            }

            internal static List<DashboardItem> GetSaleList(DateTime fromDate, DateTime toDate)
            {

                var sql = string.Format(@"SELECT ....FROM ... WHERE OrderDate BETWEEN  '{0}' and '{1}'", fromDate, toDate);

                return GetDashboardItemByConditionsDate (sql);
            }

            internal static List<DashboardItem> GetTotalReturnList(DateTime fromDate, DateTime toDate)
            {
                var sql = string.Format(@"SELECT ....FROM ... WHERE OrderDate BETWEEN  '{0}' and '{1}'", fromDate, toDate);

                return GetDashboardItemByConditionsDate(sql);
            }

            private static List<DashboardItem> GetDashboardItemByConditionsDate(string sql)
            {
                var reader = SqlHelper.ExecuteReader(_connectionString, sql, CommandType.Text);

                var result = new List<DashboardItem>();

                while (reader.Read())
                {
                    var item = new DashboardItem
                    {
                        Total = long.Parse(reader[0] + ""),
                        DisplayText = reader[1] + ""
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