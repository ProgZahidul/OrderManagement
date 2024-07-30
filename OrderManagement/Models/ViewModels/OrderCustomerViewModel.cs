using System.ComponentModel.DataAnnotations;

namespace OrderManagement.Models.ViewModels
{
    public class OrderCustomerViewModel
    {
        public int Id { get; set; }


        [Required]
        public string CustomerName { get; set; }

        public string? Address { get; set; }

        public string? Phone { get; set; }

        public List<OrderItemViewModel> Items { get; set; } = new List<OrderItemViewModel>();
    }

}
