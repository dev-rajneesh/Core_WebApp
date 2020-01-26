using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Core_WebApp.Models
{
   public class Category
    {
        [Key] // Primary Identity Key
        public int CategoryRowId { get; set; }

        [Required(ErrorMessage ="Category Id is must")]
        public string CategoryId { get; set; }

        [Required(ErrorMessage = "Category Name is must")]
        public string CategoryName { get; set; }

        [Required(ErrorMessage = "Base Price is must")]
        public int BasePrice { get; set; }

        public ICollection<Product> Products { get; set; }
    }

    public class Product
    {
        [Key] // Primary Identity Key
        public int ProductRowId { get; set; }

        [Required(ErrorMessage = "Product Id is must")]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "ProductName is must")]
        public string ProductName { get; set; }

        [Required(ErrorMessage = "Manufacturer is must")]
        public string Manufacturer { get; set; }

        [Required(ErrorMessage = "Description is must")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Product Price is must")]
        public int ProductPrice { get; set; }

        [Required(ErrorMessage = "CategoryRowId is must")]
        public int CategoryRowId { get; set; }

        // Foreign Key      
        public Category Category { get; set; }
    }
}


