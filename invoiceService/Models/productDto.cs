using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InvoiceService.Models
{
    internal class ProductDto
    {
        public int productId {get;set;}
        public int amount {get; set;}


    }
}
