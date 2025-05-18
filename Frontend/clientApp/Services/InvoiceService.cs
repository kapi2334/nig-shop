using clientApp.Models;
using System.Text.Json;

namespace clientApp.Services
{
    public class InvoiceService
    {
        private readonly ApiService _apiService;
        private const string INVOICE_SERVICE_URL = "http://localhost:3002/invoices"; // Direct microservice URL

        public InvoiceService(ApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<(bool Success, int? Id)> CreateInvoice(InvoiceDto invoice)
        {
            try
            {
                Console.WriteLine("[Debug] Creating invoice with data:");
                Console.WriteLine($"[Debug] Client ID: {invoice.clientId}");
                Console.WriteLine($"[Debug] Payment Type: {invoice.paymentType}");
                Console.WriteLine($"[Debug] Address ID: {invoice.addressId}");
                Console.WriteLine($"[Debug] Number of products: {invoice.products.Count}");

                foreach (var product in invoice.products)
                {
                    Console.WriteLine($"[Debug] Product ID: {product.productId}, Amount: {product.amount}");
                }

                var invoiceId = await _apiService.PostAsync<int>(INVOICE_SERVICE_URL, invoice);
                
                if (invoiceId > 0)
                {
                    Console.WriteLine($"[Debug] Invoice created successfully with ID: {invoiceId}");
                    return (true, invoiceId);
                }
                
                Console.WriteLine("[Error] Failed to create invoice: Invalid invoice ID returned");
                return (false, null);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Error] Failed to create invoice: {ex.Message}");
                Console.WriteLine($"[Error] Stack trace: {ex.StackTrace}");
                return (false, null);
            }
        }
    }
} 