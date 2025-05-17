using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InvoiceService.Models
{
    public class ProductDto
    {
        public int productId {get;set;}
        public int amount {get; set;}


    }
}
