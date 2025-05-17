using InvoiceService.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InvoiceService.Models;


internal class Product
{
    
    public int id { get; set; }

   
    public string name { get; set; }


    public string type { get; set; }

    public double weight { get; set; }

    public float price { get; set; }
 
    public int tax { get; set; }
    
    public int dimensionsId { get; set; }



    
    public int materialId { get; set; }



    
    public int surfaceTypeId { get; set; }

}
