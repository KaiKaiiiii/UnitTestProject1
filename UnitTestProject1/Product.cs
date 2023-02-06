using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject1
{
    internal class Product : Size
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public string ProductMainImg { get; set; }
        public string ProductGallery { get; set; }
        public string SKU { get; set; }
        public string Barcode { get; set; }
        public int Price { get; set; }
        public int Cost { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set;}
        public int ProviderId { get; set; }
    }
}
