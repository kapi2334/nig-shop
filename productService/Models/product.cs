using ProduktService.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProduktService.Models;

[Table("produkt")]
public class Product
{
    [Key]
    [Column("id")]
    public int id { get; set; }

    [Column("nazwa")]
    public string name { get; set; }

    [Column("rodzaj")]
    public string type { get; set; }

    [Column("waga")]
    public double weight { get; set; }

    [Column("wymiary_id")]
    public int dimensionsId { get; set; }

    public Dimensions dimensions { get; set; }

    [Column("material_id")]
    public int materialId { get; set; }

    public Material material { get; set; }

    [Column("typ_nawierzchni_id")]
    public int surfaceTypeId { get; set; }

    public SurfaceType surfaceType { get; set; }
}
