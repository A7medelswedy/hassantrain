using assignmentt.Models;

namespace assignmentt.ViewModel
{
    public class OrderDetailsViewModel
    {
        public int OrderID { get; set; }
        public string CustomerName { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public List<ProductDetail> Products { get; set; }

        public class ProductDetail
        {
            public string ProductName { get; set; }
            public int Quantity { get; set; }
            public decimal Price { get; set; }
        }
    }

}
