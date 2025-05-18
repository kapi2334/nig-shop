namespace clientApp.Models
{
    public class Invoice
    {
        public int id { get; set; }
        public int clientId { get; set; }
        public string clientName { get; set; } = string.Empty;
        public long? clientNIP { get; set; }
        public string clientStreet { get; set; } = string.Empty;
        public string clientCity { get; set; } = string.Empty;
        public int clientHouseNumber { get; set; }
        public int? clientApartmentNumber { get; set; }
        public string clientPostalCode { get; set; } = string.Empty;
        public string clientCountry { get; set; } = string.Empty;
        public string paymentType { get; set; } = string.Empty;
        public DateTime issueDate { get; set; }
        public DateTime paymentDeadline { get; set; }
        public int issuerId { get; set; }
    }
} 