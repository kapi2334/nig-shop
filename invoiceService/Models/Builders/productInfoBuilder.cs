namespace InvoiceService.Models.Builders{

    internal class ProductInfoBuilder{

        private readonly ProductInfo _productInfo;

        public ProductInfoBuilder()
        {
            _productInfo = new ProductInfo();
        }


        public ProductInfoBuilder WithQuantity(string quantity)
        {
            _productInfo.quantity = quantity;
            return this;
        }

        public ProductInfoBuilder WithTotalPrice(double totalPrice)
        {
            _productInfo.totalPrice = totalPrice;
            return this;
        }

        public ProductInfoBuilder WithTax(int tax)
        {
            _productInfo.tax = tax;
            return this;
        }

        public ProductInfoBuilder WithTaxAmount(double taxAmount)
        {
            _productInfo.taxAmount = taxAmount;
            return this;
        }

        public ProductInfoBuilder WithNet(double net)
        {
            _productInfo.net = net;
            return this;
        }

        public ProductInfoBuilder WithGross(double gross)
        {
            _productInfo.gross = gross;
            return this;
        }

        public ProductInfoBuilder WithInvoiceId(int invoiceId)
        {
            _productInfo.invoiceId = invoiceId;
            return this;
        }

        public ProductInfoBuilder WithProductId(int productId)
        {
            _productInfo.product_id = productId;
            return this;
        }

        public ProductInfo Build()
        {
            return _productInfo;
        }
    }
}

