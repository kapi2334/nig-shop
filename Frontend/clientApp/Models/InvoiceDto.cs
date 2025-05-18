namespace clientApp.Models
{
    public class InvoiceDto
    {
        public int clientId { get; set; }
        public string paymentType { get; set; } = "card";
        public int addressId { get; set; }
        public List<InvoiceProductDto> products { get; set; } = new List<InvoiceProductDto>();
    }

    public class InvoiceProductDto
    {
        public int productId { get; set; }
        public int amount { get; set; }
    }
} 