using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using InvoiceService.Models.Services;
using InvoiceService.Models;
using InvoiceService.Models.Builders;

namespace InvoiceService.Models
{
    [Table("faktura")]
    internal class Invoice
    {
        [Key]
        [Column("id")]
        public int id { get; set; }

        [Column("klient_nazwa")]
        public string clientName { get; set; }

        [Column("klient_ulica")]
        public string clientStreet { get; set; }

        [Column("klient_nrdomu")]
        public int clientHouseNumber { get; set; }

        [Column("klient_nrmieszkania")]
        public int? clientApartmentNumber { get; set; }

        [Column("klient_kodpocztowy")]
        public string clientPostalCode { get; set; }

        [Column("klient_miasto")]
        public string clientCity { get; set; }

        [Column("klient_kraj")]
        public string clientCountry { get; set; }

        [Column("klient_nip")]
        public long? clientNIP { get; set; }

        [Column("datawystawienia")]
        public DateTime issueDate { get; set; }

        [Column("terminplatnosci")]
        public DateTime paymentDeadline { get; set; }

        [Column("rodzajplatnosci")]
        public string paymentType { get; set; }

        [Column("wystawca_id")]
        public int issuerId { get; set; }

        [ForeignKey("issuerId")]
        public Issuer issuer { get; set; }

        public List<ProductInfo> products { get; set; }


    public async Task BuildProductInfosAsync(List<ProductDto> productDtos, ApiService apiService){

    var productInfos = new List<ProductInfo>();

    foreach (var dto in productDtos){

        var product = await apiService.GetAsync<Product>($"http://product_service:5005/products/{dto.productId}");

        if (product == null){
            throw new Exception($"Api response error: product with id:{dto.productId} returned empty.");
        }
        //Calculating required variables
        double totalPrice = product.price * dto.amount;
        double net = totalPrice / (1 + (product.tax / 100.0));
        double taxAmount = totalPrice - net;
        
        //building productInfo
        var productInfo = new ProductInfoBuilder()
            .WithProductId(product.id)
            .WithQuantity(dto.amount.ToString())
            .WithTotalPrice(totalPrice)
            .WithNet(Math.Round(net, 2))
            .WithTax(product.tax)
            .WithTaxAmount(Math.Round(taxAmount, 2))
            .WithGross(Math.Round(totalPrice, 2))
            .Build();
        
        productInfos.Add(productInfo);
    }

    products = productInfos;
}




    }
}
