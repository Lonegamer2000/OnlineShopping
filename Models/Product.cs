

using System.ComponentModel.DataAnnotations;
namespace onlineshopping.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Title is required")]
        [StringLength(50,ErrorMessage = "Max character is 50")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Price is required")]
        
        public int Price { get; set; }
        [StringLength(50, ErrorMessage = "Max character is 50")]
        public string? description { get; set; }

        public string? product_icon { get; set; }
        public int categoryid { get; set;}

        public virtual Category? Category { get; set; }  


    }
}
