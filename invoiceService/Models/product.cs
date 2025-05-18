using InvoiceService.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InvoiceService.Models;

[Table("produkt")]
internal class Product
{
    [Key]
    [Column("id")]
    public int id { get; set; }

    [Column("nazwa")]
    public string name { get; set; }

    [Column("typ")]
    public string type { get; set; }

    [Column("waga")]
    public double weight { get; set; }

    [Column("cena")]
    public float price { get; set; }

    [Column("podatek")]
    public int tax { get; set; }
    
    [Column("wymiary_id")]
    public int dimensionsId { get; set; }

    [Column("material_id")]
    public int materialId { get; set; }

    [Column("powierzchnia_id")]
    public int surfaceTypeId { get; set; }
}
