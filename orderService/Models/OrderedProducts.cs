using System.ComponentModel.DataAnnotations.Schema;

namespace OrderService.Models
{
    [Table("zamowienie_produkty")]
    public class OrderedProducts
    {
        [Column("id")]
        public int id { get; set; }

        [Column("zamowienie_id")]
        public int orderId { get; set; }

        [Column("produkt_id")]
        public int productId { get; set; }

        [Column("ilosc")]
        public int quantity { get; set; }
    }
}
