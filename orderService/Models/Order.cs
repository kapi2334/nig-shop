using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderService.Models
{
    [Table("zamowienie")] // mapa klasy do tabeli
    public class Order
    {
        [Column("id")]
        public int id { get; set; }

        [Column("id_klienta")]
        public int clientId { get; set; }

        [Column("dostawa")]
        public bool delivery { get; set; }

        [Column("id_zamowienia")]
        public int orderId { get; set; }

        [NotMapped]
        public List<OrderedProducts>? products { get; set; }
    }
}
