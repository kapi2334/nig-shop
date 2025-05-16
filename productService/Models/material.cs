using ProductService.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductService.Models;

[Table("material")]
public class Material
{
    [Key]
    [Column("id")]
    public int id { get; set; }

    [Column("nazwa")]
    public string name { get; set; }
}
