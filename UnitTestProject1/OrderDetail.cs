namespace UnitTestProject1
{
    internal class OrderDetail : Order
    {
        public int TotalPerProductPrice { get; set; }
        public int Quantity { get; internal set; }

    }
}