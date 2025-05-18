namespace clientApp.Models
{
    public class AddressDto
    {
        public int id { get; set; }
        public string street { get; set; } = string.Empty;
        public int buildingNo { get; set; }
        public int? localeNo { get; set; }
        public string postCode { get; set; } = string.Empty;
        public string city { get; set; } = string.Empty;
        public string country { get; set; } = string.Empty;
    }
} 