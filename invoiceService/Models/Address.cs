using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using InvoiceService.Models;

namespace InvoiceService.Models
{
    internal class Address
    {
        public string street { get; set; }

        public int buildingNo { get; set; }

        public int? localeNo { get; set; }

        public string postCode { get; set; }

        public string city { get; set; }

        public string country { get; set; }

    }
}
