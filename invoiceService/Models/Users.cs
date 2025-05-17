
namespace InvoiceService.Models
{
    internal class User
    {
        public List<Address> Addresses { get; set; }

        public  long? nip { get; set; }
		
		private char _type {get; set;} 

		public char type {get; set;}
		

        public string name { get; set; }

        public string? surname { get; set; }

    }
}
