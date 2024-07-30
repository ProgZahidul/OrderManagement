using System.ComponentModel.DataAnnotations;

namespace OrderManagement.Models
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public string Address { get; set; }
       public ICollection<Order> Orders { get; set; } = new List<Order>();
    }


}
