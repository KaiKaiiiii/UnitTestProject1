using System;

namespace UnitTestProject1
{
    internal class Order : Product
    {
        public int OrderId { get; internal set; }
        public DateTime OrderDate { get; internal set; }
        public string ChannelName { get; internal set; }
        public int ProductUnit { get; internal set; }
        public int TotalPrice { get; internal set; }
        public string ShipmentProvider { get; internal set; }
        public string OrderStatus { get; internal set; }
        public string Note { get; internal set; }
        public string OrderNumber { get; internal set; }
        public string TaxCode { get; internal set; }
        public string BuyerName { get; internal set; }
        public string ShippingAddress { get; internal set; }
        public string RecipientName { get; internal set; }
        public string RecipientPhoneNumber { get; internal set; }
        public string RecipientEmail { get;internal set; }
        public string City { get; internal set; }
        public string Region { get; internal set; }
        public string ZipCode { get; internal set;}
        public string Ward { get; internal set; }
        public string CustomerPaymentMethod { get; internal set; }
    }
}