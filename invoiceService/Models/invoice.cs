using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InvoiceService.Models
{
    [Table("faktura")]
    public class Invoice
    {
        [Key]
        [Column("id")]
        public int id { get; set; }

        [Column("klient_nazwa")]
        public string clientName { get; set; }

        [Column("klient_ulica")]
        public string clientStreet { get; set; }

        [Column("klient_nrdomu")]
        public int clientHouseNumber { get; set; }

        [Column("klient_nrmieszkania")]
        public int? clientApartmentNumber { get; set; }

        [Column("klient_kodpocztowy")]
        public string clientPostalCode { get; set; }

        [Column("klient_miasto")]
        public string clientCity { get; set; }

        [Column("klient_kraj")]
        public string clientCountry { get; set; }

        [Column("klient_nip")]
        public string clientNIP { get; set; }

        [Column("datawystawienia")]
        public DateTime issueDate { get; set; }

        [Column("terminplatnosci")]
        public DateTime paymentDeadline { get; set; }

        [Column("rodzajplatnosci")]
        public string paymentType { get; set; }

        [Column("wystawca_id")]
        public int issuerId { get; set; }

        [ForeignKey("issuerId")]
        public Issuer issuer { get; set; }

        public List<ProductInfo> products { get; set; }
    }
}
