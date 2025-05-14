using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using userService.Models.Interfaces;
using UserService.Models;

namespace UserService.Models
{
    [Table("firma")]
    internal class Company: IUserType
    {
        [Key]
        [Column("id")]
        public int id { get; set; }

        [Column("login")]
        public string login { get; set; }

        [Column("passwordhash")]
        public string passwordHash { get; set; }
        public IEnumerable<IEntity> address { get; set; }

        [Column("nip")]
        public long? nip { get; set; }

        [Column("nazwa")]
        public string name { get; set; }

    }
}
