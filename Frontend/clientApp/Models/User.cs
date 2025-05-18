using System.Collections.Generic;

namespace clientApp.Models
{
    public class User
    {
        public int id { get; set; }
        public string login { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
        public long? nip { get; set; }
        public char type { get; set; }
        public List<Address> Addresses { get; set; } = new List<Address>();
    }

    public class Address
    {
        public int id { get; set; }
        public string street { get; set; }
        public string city { get; set; }
        public string postalCode { get; set; }
        public string country { get; set; }
    }
} 