namespace OrderManagement.Models.ViewModels
{
    public class OrderViewModel
    {
        public Order Order { get; set; }
        public Customer Customer { get; set; }
        public List<Product> Products { get; set; }
    }

}
