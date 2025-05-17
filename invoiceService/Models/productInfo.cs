using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InvoiceService.Models
{
    [Table("produktinfo")]
    internal class ProductInfo
    {
        [Key]
        [Column("id")]
        public int id { get; set; }

        [Column("ilosc")]
        public string quantity { get; set; }

        [Column("cenaogolem")]
        public double totalPrice { get; set; }

        [Column("podatek")]
        public int tax { get; set; }

        [Column("kwotapodatku")]
        public double taxAmount { get; set; }

        [Column("netto")]
        public double net { get; set; }

        [Column("brutto")]
        public double gross { get; set; }

        [Column("faktura_id")]
        public int invoiceId { get; set; }

        [Column("produkt_id")]
        public int product_id { get; set; }
    }
}
