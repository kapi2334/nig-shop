namespace InvoiceService.Models{
   internal class InvoiceDto{
        public int clientId {get; set;}
        public string paymentType {get; set;}
		public int addressId {get; set;}
        public List<ProductDto> products {get; set;}



   } 



}
