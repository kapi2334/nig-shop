using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InvoiceService.Models
{
    [Table("wystawca")]
    public class Issuer
    {
        [Key]
        [Column("id")]
        public int id { get; set; }

        [Column("nazwa")]
        public string name { get; set; }

        [Column("ulica")]
        public string street { get; set; }

        [Column("nrdomu")]
        public int houseNumber { get; set; }

        [Column("nrmieszkania")]
        public int? apartmentNumber { get; set; }

        [Column("kodpocztowy")]
        public string postalCode { get; set; }

        [Column("miasto")]
        public string city { get; set; }

        [Column("kraj")]
        public string country { get; set; }

        [Column("nip")]
        public string nip { get; set; }

        public List<Invoice> invoices { get; set; }
    }
}