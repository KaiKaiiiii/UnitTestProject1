using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using static DataLogic;
namespace UnitTestProject1
{
    internal class OrderService
    {
        internal static List<Order> GetDateToDateAllOrder(string fromDate, string toDate, int? channelId = null, string orderStatus = "")
        {

            string byChannel = channelId > 0 ? string.Format("AND ChannelId = {0}", channelId) : string.Empty;

            string byOrderStatus = orderStatus?.Length > 0 ? $"AND OrderStatus = '{orderStatus}'" : string.Empty;


            var sql = string.Format(@"
            SELECT odl.Id,
                   CONVERT(DATE, odl.OrderedAt) AS Date,
                   chn.ChannelName,
                   COUNT(odt.ProductId) AS ProductUnit,
                   odl.TotalPrice,
                   odl.ShipmentProvider,
                   odl.OrderStatus
            FROM ORDERLIST AS odl
            JOIN Channel AS chn ON odl.ChannelId = chn.Id
            JOIN OrderDetail AS odt ON odt.OrderId = odl.Id
            WHERE CAST(odl.OrderedAt AS Date) BETWEEN '{0}' AND '{1}'
                  {2} {3}
            GROUP BY odl.Id,
                     CONVERT(DATE, odl.OrderedAt),
                     ChannelName,
                     TotalPrice,
                     ShipmentProvider,
                     OrderStatus
                        ", fromDate, toDate, byChannel, byOrderStatus);


            var reader = SqlHelper.ExecuteReader(_connectionString, sql, CommandType.Text);
            var result = new List<Order>();
            while (reader.Read())
            {
                result.Add(new Order
                {

                    OrderId = Int32.Parse(reader[0] + ""),
                    OrderDate = DateTime.Parse(reader[1] + ""),
                    ChannelName = Convert.ToString(reader[2]),
                    ProductUnit = Int32.Parse(reader[3] + ""),
                    TotalPrice = Int32.Parse(reader[4] + ""),
                    ShipmentProvider = Convert.ToString(reader[5]),
                    OrderStatus = Convert.ToString(reader[6]),
                });
            }
            return result;
        }

        internal static List<OrderDetail> GetOrderDetailById(int? id)
        {

            if (id != null && id > 0)
            {
                var sql = string.Format(@"
                SELECT prd.ProductMainImg,
                       prd.ProductName,
                       prd.SKU,
                       prd.Barcode,
                       odt.Quantity,
                       prd.Price AS PerProductPrice,
                       (odt.Quantity * prd.Price) AS TotalPerProductPrice,
                       odl.RecipientName,
                       odl.RecipientPhoneNumber,
                       odl.ShippingAddress
                FROM OrderList AS odl
                JOIN OrderDetail AS odt ON odl.Id = odt.OrderId
                JOIN Product AS prd ON odt.ProductId = prd.Id
                WHERE odl.Id = {0}    
                ", id);

                var reader = SqlHelper.ExecuteReader(_connectionString, sql, CommandType.Text);
                var result = new List<OrderDetail>();
                while (reader.Read())
                {
                    result.Add(new OrderDetail
                    {
                        ProductMainImg = Convert.ToString(reader[0]),
                        ProductName = Convert.ToString(reader[1]),
                        SKU = Convert.ToString(reader[2]),
                        Barcode = Convert.ToString(reader[3]),
                        Quantity = Convert.ToInt32(reader[4]),
                        Price = Convert.ToInt32(reader[5]),
                        TotalPerProductPrice = Convert.ToInt32(reader[6]),
                        RecipientName = Convert.ToString(reader[7]),
                        RecipientPhoneNumber = Convert.ToString(reader[8]),
                        ShippingAddress = Convert.ToString(reader[9])
                    });
                }
                return result;
            }
            else
            {
                return null;
                throw new ArgumentNullException("Null object");
            }
        }

        internal static int UpdateReturnOrder(int returnId, string updateStatus, User user)
        {

            if (returnId != 0 && (updateStatus == "APPROVE" || updateStatus =="CANCEL") )
            {
                string updatedStatus = updateStatus == "APPROVE" ? "APPROVED" : "CANCELLED";

                var sql = string.Format(@"
                    UPDATE ReturnList
                    SET ReturnStatus= '{0}',
                        UpdatedAt = GETDATE(),
                        UpdatedByUserId = {1}
                    WHERE ReturnStatus = 'PENDING'
                      AND Id = {2};",  updatedStatus, user.Id, returnId);

                var result = SqlHelper.ExecuteNonQuery(_connectionString, sql, CommandType.Text);
                return result;
            }
            else
            {
                return 0;
                throw new ArgumentNullException();
            }
        }
    }
}