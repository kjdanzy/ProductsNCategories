using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ProductsNCategories.Models
{
    public class Association
    {
        [Key]
        public int AssociationId { get; set; }
        [Required]
        public int ProductId { get; set; }
        public Product product { get; set; }
        [Required]
        public int CategoryId { get; set; }
        public Category category { get; set; }

    }
}
