using System.ComponentModel.DataAnnotations.Schema;
using userService.Models.Interfaces;

namespace UserService.Models
{
    [Table("osobafizycznaadres")]
    internal class ClientEntity: IEntity
    {
        [Column("osoba_id")]
        public int id { get; set; }

        [ForeignKey("OsobaId")]
        public IUserType user { get; set; }

        [Column("adres_id")]
        public int addressId { get; set; }

        [ForeignKey("AdresId")]
        public Address address { get; set; }
    }
}
