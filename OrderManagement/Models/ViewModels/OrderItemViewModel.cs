namespace OrderManagement.Models.ViewModels
{
    public class OrderItemViewModel
    {
        public int? ProductId { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal TotalPrice => UnitPrice * Quantity;

        public Product? Product { get; set; }
    }

}
