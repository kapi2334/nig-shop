using InvoiceService.Models;
namespace InvoiceService.Models.Builders{

    internal class InvoiceBuilder
    {
        private readonly Invoice _invoice;

        public InvoiceBuilder()
        {
            _invoice = new Invoice();
        }

        public InvoiceBuilder WithDataFromUser(User inputUser){
           _invoice.clientName = inputUser.name;
           _invoice.clientNIP = inputUser.nip;
           return this;
        }

        public InvoiceBuilder WithDataFromAddress(Address inputAddress)
        {
            _invoice.clientStreet = inputAddress.street;
            _invoice.clientCity = inputAddress.city;
            _invoice.clientHouseNumber = inputAddress.buildingNo;
            _invoice.clientApartmentNumber = inputAddress.localeNo;
            _invoice.clientPostalCode = inputAddress.postCode;
            _invoice.clientCountry = inputAddress.country;
            return this;
        }

        public InvoiceBuilder WithPaymentType(string paymentType)
        {
            _invoice.paymentType = paymentType;
            return this;
        }

        public InvoiceBuilder WithIssueDate(DateTime time)
        {
            _invoice.issueDate = time;
            return this;
        }

        public InvoiceBuilder WithPaymentDeadline(DateTime time)
        {
            _invoice.paymentDeadline = time;
            return this;
        }

        public InvoiceBuilder WithIssuerId(int id){
            _invoice.issuerId = id;
            return this;
        }

        public InvoiceBuilder WithIssuer(Issuer issuer)
        {
            _invoice.issuer = issuer;
            return this;
        }

        public InvoiceBuilder WithProducts(List<ProductInfo> products)
        {
            _invoice.products = products;
            return this;
        }

        public Invoice Build()
        {
            return _invoice;
        }
    }


}
