using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace InvoiceService.Models
{
    internal class IssuerDto
    {
        public string name { get; set; }

       
        public string street { get; set; }

        
        public int houseNumber { get; set; }

        
        public int? apartmentNumber { get; set; }

        
        public string postalCode { get; set; }

        
        public string city { get; set; }

       
        public string country { get; set; }

        
        public string nip { get; set; }

		public Issuer MapToIssuer(){
			
			Issuer outIssuer = new Issuer();
			
			outIssuer.name = name;
            outIssuer.street = street;
            outIssuer.houseNumber = houseNumber;
            outIssuer.apartmentNumber = apartmentNumber;
            outIssuer.postalCode = postalCode;
            outIssuer.city = city;
            outIssuer.country = country;
            outIssuer.nip = nip;
            outIssuer.invoices = new List<Invoice>();
			return outIssuer;
		}
		

    }
}
