namespace onlineshopping.Models
{
    public class Category
    {
        public int id { get; set; } 
        public string Name { get; set; }
        public virtual List<Product>? Products { get; set; } 

    }
}
