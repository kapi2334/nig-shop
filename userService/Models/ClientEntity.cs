using System.ComponentModel.DataAnnotations.Schema;
using UserService.Models.Interfaces;
using UserService.Models.Abstract;

namespace UserService.Models
{
    [Table("osobafizycznaadres")]
    internal class ClientEntity: IEntity
    {
        [Column("osoba_id")]
        public int id { get; set; }

        [ForeignKey("OsobaId")]
        public UserType user { get; set; }

        [Column("adres_id")]
        public int addressId { get; set; }

        [ForeignKey("AdresId")]
        public Address address { get; set; }
    }
}
