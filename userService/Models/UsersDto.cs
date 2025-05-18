
namespace UserService.Models{

    internal class UserDto{

        public int id { get; set; }

        public string login { get; set; }

        public string passwordHash { get; set; }

        public int addressID { get; set; }
        public ICollection<AddressDto>? Addresses { get; set; }

        public  long? nip { get; set; }

        private char _type = 'u'; // default value - user

        public char type
        {
            get => _type;
            set
            {
                if (value == 'u' || value == 'c')
                {
                    _type = value;
                }
                else
                {
                    _type = 'u';
                    throw new ArgumentException("The type values ​​can only be set to \"u\" and \"c\". The default value \"u\" is used");
                }
            }
        }

        public string name { get; set; }

        public string? surname { get; set; }

    }
}
