using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data;
using System.Data.SqlClient;
using static DataLogic;

namespace UnitTestProject1
{
    [TestClass]
    public partial class UnitTest1
    {
        private string cmdText = "Select * from members";

        [TestMethod]
        public void TestMethod1()
        {
            string connectionString = "Data Source=.;Initial Catalog=learning;Integrated Security=True";
            var connection = new SqlConnection(connectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = cmdText;
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                Console.WriteLine(reader[0]);
                Console.WriteLine(reader[0]);
            }

        }

        [TestMethod]
        public void GetSaleListTest()
        {
            DateTime fromDate = default;
            DateTime toDate = default;
            var result = DashboardService.GetTotalSales(fromDate, toDate);

            Assert.IsNotNull(result >= 0);
        }

        [TestMethod]
        public void GetTotalSalesTest()
        {
            DateTime fromDate = default;
            DateTime toDate = default;
            var result = DashboardService.GetSaleList(fromDate, toDate);

            Assert.IsTrue(result.Count >= 0);
        }
        
        [TestMethod]
        public void GetTotalReturnListTest()
        {
            DateTime fromDate = default;
            DateTime toDate = default;
            var result = DashboardService.GetTotalReturnList(fromDate, toDate);

            Assert.IsTrue(result.Count >= 0);
        }

        [TestMethod]
        public void GetTotalReturnTest()
        {
            DateTime fromDate = default;
            DateTime toDate = default;
            var result = DashboardService.GetTotalReturn(fromDate, toDate);

            Assert.IsTrue(result >= 0);
        }
    }


}