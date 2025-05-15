using ProduktService.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProduktService.Models;

[Table("wymiary")]
public class Dimensions
{
    [Key]
    [Column("id")]
    public int id { get; set; }

    [Column("dlugosc")]
    public double length { get; set; }

    [Column("szerokosc")]
    public double width { get; set; }

    [Column("wysokosc")]
    public double height { get; set; }
}
