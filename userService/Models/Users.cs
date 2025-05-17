using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UserService.Models;

namespace UserService.Models{
    [Table("podmiot")]
    internal class User{
        [Key]
        [Column("id")]
        public int id { get; set; }

        [Column("login")]
        public string login { get; set; }

        [Column("passwordhash")]
        public string passwordHash { get; set; }

        public int addressID { get; set; }
        public ICollection<Address> Addresses { get; set; }

        [Column("nip")]
        public  long? nip { get; set; }
		
		private char _type = 'u'; // default value - user

		public char type{
			get => _type;
			set{
				if (value == 'u' || value == 'c'){
					_type = value;
				}
				else{
					_type = 'u';
					throw new ArgumentException("The type values ​​can only be set to \"u\" and \"c\". The default value \"u\" is used");
				}
			}
		}

        [Column("imie")]
        public string name { get; set; }

        [Column("nazwisko")]
        public string? surname { get; set; }

    }
}
