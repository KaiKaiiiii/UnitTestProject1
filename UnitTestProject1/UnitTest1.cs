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

        //[TestMethod]
        //public void GetSaleListTest()
        //{
        //    DateTime fromDate = default;
        //    DateTime toDate = default;
        //    var result = DashboardService.GetTotalSales(fromDate, toDate);

        //    Assert.IsNotNull(result >= 0);
        //}

        //[TestMethod]
        //public void GetTotalSalesTest()
        //{
        //    DateTime fromDate = default;
        //    DateTime toDate = default;
        //    var result = DashboardService.GetSaleList(fromDate, toDate);

        //    Assert.IsTrue(result.Count >= 0);
        //}

        //[TestMethod]
        //public void GetTotalReturnListTest()
        //{
        //    DateTime fromDate = default;
        //    DateTime toDate = default;
        //    var result = DashboardService.GetTotalReturnList(fromDate, toDate);

        //    Assert.IsTrue(result.Count >= 0);
        //}

        //[TestMethod]
        //public void GetTotalReturnTest()
        //{
        //    DateTime fromDate = default;
        //    DateTime toDate = default;
        //    var result = DashboardService.GetTotalReturn(fromDate, toDate);

        //    Assert.IsTrue(result >= 0);
        //}

        [TestMethod]
        public void GetTotalProductByCatalogTest()
        {
            DateTime fromDate = new DateTime(2023, 1, 24, 0, 0, 0);
            DateTime toDate = new DateTime(2023, 1, 24, 0, 0, 0);
            string productName = "Avocado 10kg Box";
            var result = DashboardService.GetTotalProductByCatalog(fromDate, toDate, productName);

            Assert.IsTrue(result.Count > 0);
        }

        [TestMethod]
        public void GetMonthlyCancelledDeliveriedReturnTest()
        {
            DateTime fromDate = new DateTime(2023, 1, 24, 0, 0, 0);
            DateTime toDate = new DateTime(2023, 1, 24, 0, 0, 0);
            var result = DashboardService.GetMonthlyCancelledDeliveriedReturn(fromDate, toDate);

            Assert.IsTrue(result.Count > 0);
        }

        [TestMethod]
        public void GetSalesAnalyticsByCountryMonthlyTest()
        {
            var result = DashboardService.GetSalesAnalyticsByCountryMonthly();

            Assert.IsTrue(result.Count > 0);
        }

        [TestMethod]
        public void UpdatePasswordSuccessTest()
        {
            string oldPassword = "123456";
            string newPassword = "12345678";
            int id = 2;
            var result = UserService.UpdatePassword(id, oldPassword, newPassword);

            Assert.AreEqual(result, 1);
        }

        [TestMethod]
        public void UpdatePasswordFailTest()
        {
            string oldPassword = "1231231231231231232123";
            string newPassword = "12345678";
            int id = 2;
            var result = UserService.UpdatePassword(id, oldPassword, newPassword);

            Assert.AreEqual(result, 0);
        }

        [TestMethod]
        public void GetUserInformationByIdTest()
        {
            int id = 2;
            var result = UserService.GetUserInformationById(id);

            Assert.IsTrue(result.FullName != null);
        }

        [TestMethod]
        public void GetDateToDateAllOrderTest()
        {
            DateTime fromDate = new DateTime(2023, 1, 24);
            DateTime toDate = new DateTime(2023, 2, 24);
            int channelId = 2;
            string orderStatus = string.Format("COMPLETED");

            var result = OrderService.GetDateToDateAllOrder(fromDate.ToString("yyyy'-'MM'-'dd"), toDate.ToString("yyyy'-'MM'-'dd"), channelId, orderStatus);

            Assert.IsTrue(result.Count > 0);

            var resultWithoutChannelId = OrderService.GetDateToDateAllOrder(fromDate.ToString("yyyy'-'MM'-'dd"), toDate.ToString("yyyy'-'MM'-'dd"), null, orderStatus);

            Assert.IsTrue(resultWithoutChannelId.Count > 0);

            var resultWithoutOrderStatus = OrderService.GetDateToDateAllOrder(fromDate.ToString("yyyy'-'MM'-'dd"), toDate.ToString("yyyy'-'MM'-'dd"), channelId, null);

            Assert.IsTrue(resultWithoutOrderStatus.Count > 0);
        }

        [TestMethod]
        public void GetDateToDateAllOderNoneValueTest()
        {
            try
            {

                DateTime fromDate = new DateTime(2023, 1, 24);
                DateTime toDate = new DateTime(2023, 1, 24);
                int channelId = 4;
                string orderStatus = string.Format("COMPLETED");

                var result = OrderService.GetDateToDateAllOrder(fromDate.ToString("yyyy'-'MM'-'dd"), toDate.ToString("yyyy'-'MM'-'dd"), channelId, orderStatus);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is Exception);
                throw new ArgumentNullException("There's no order");
            }

        }

        [TestMethod]
        public void GetOrderDetailByIdTest()
        {
            int id = 1;
            var result = OrderService.GetOrderDetailById(id);

            Assert.IsNotNull(result);
        }
        [TestMethod]
        public void GetOrderDetailByNullIdTest()
        {
            int id = 0;
            var result = OrderService.GetOrderDetailById(id);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void UpdateReturnOrderTest()
        {
            string updateStatus = "CANCEL";
            User user = UserService.GetUserInformationById(2);
            int returnId = 22;

            var result = OrderService.UpdateReturnOrder(returnId, updateStatus, user);

            Assert.AreEqual(result, 1);
        }

        [TestMethod]
        public void UpdateReturnOrderFailTest()
        {
            string updateStatus = "PENDING";
            User user = UserService.GetUserInformationById(2);
            int returnID = 0;

            var result = OrderService.UpdateReturnOrder(returnID, updateStatus, user);

            Assert.AreEqual(result, 0);
        }
    }


}