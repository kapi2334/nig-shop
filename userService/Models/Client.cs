using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UserService.Models.Interfaces;
using UserService.Models.Abstract;
using UserService.Models;

namespace UserService.Models
{
    [Table("osobafizyczna")]
    internal class Client: UserType
    {
        [Key]
        [Column("id")]
        public override int id { get; set; }

        [Column("login")]
        public override string login { get; set; }

        [Column("passwordhash")]
        public override string passwordHash { get; set; }

        public override IEnumerable<IEntity> address { get; set; }

        [Column("nip")]
        public override long? nip { get; set; }

        [Column("imie")]
        public string name { get; set; }

        [Column("nazwisko")]
        public string surname { get; set; }

    }
}
