using System.ComponentModel.DataAnnotations.Schema;
using userService.Models.Interfaces;

namespace UserService.Models
{
    [Table("firmaadres")]
    internal class CompanyEntity: IEntity
    {
        [Column("firma_id")]
        public int id { get; set; }

        [ForeignKey("FirmaId")]
        public IUserType user { get; set; }

        [Column("adres_id")]
        public int addressId { get; set; }

        [ForeignKey("AdresId")]
        public Address address { get; set; }
    }
}
