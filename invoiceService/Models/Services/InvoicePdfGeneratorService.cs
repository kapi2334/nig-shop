using System;
using System.IO;
using System.Linq;
using System.Globalization;
using PdfSharpCore.Pdf;
using PdfSharpCore.Drawing;

namespace InvoiceService.Models.Services
{
	internal class InvoicePdfGeneratorService
	{
		private const double PAGE_MARGIN = 40;
		private const double PAGE_MARGIN_BOTTOM = 50;
		private const double LINE_HEIGHT = 12;
		private const double SECTION_SPACING = 15;
		private const double TABLE_WIDTH = 500;
		private const double TABLE_START_X = PAGE_MARGIN;

		private static readonly double[] TABLE_COLUMNS = new[] { 40.0, 80.0, 130.0, 200.0, 250.0, 330.0, 400.0 };
		private static readonly string[] TABLE_HEADERS = new[] { "No.", "Quantity", "Net", "VAT %", "VAT Amount", "Gross", "Total" };

		public MemoryStream Generate(Invoice invoice)
		{
			try
			{
				Console.WriteLine("Starting PDF generation with configuration:");
				Console.WriteLine($"- TABLE_COLUMNS length: {TABLE_COLUMNS?.Length ?? 0}");
				Console.WriteLine($"- TABLE_HEADERS length: {TABLE_HEADERS?.Length ?? 0}");
				Console.WriteLine($"- Number of products: {invoice?.products?.Count ?? 0}");
				Console.WriteLine($"- Page dimensions: Margin={PAGE_MARGIN}, Bottom Margin={PAGE_MARGIN_BOTTOM}");

				// Ensure fonts are initialized
				PdfFontInitializer.Initialize();

				// Validate array configuration
				if (TABLE_COLUMNS == null || TABLE_COLUMNS.Length < 7)
				{
					throw new InvalidOperationException($"TABLE_COLUMNS array is not properly configured. Length: {TABLE_COLUMNS?.Length ?? 0}");
				}

				if (TABLE_HEADERS == null || TABLE_HEADERS.Length < 7)
				{
					throw new InvalidOperationException($"TABLE_HEADERS array is not properly configured. Length: {TABLE_HEADERS?.Length ?? 0}");
				}

				if (TABLE_HEADERS.Length != TABLE_COLUMNS.Length)
				{
					throw new InvalidOperationException($"TABLE_HEADERS and TABLE_COLUMNS must have the same length. Headers: {TABLE_HEADERS.Length}, Columns: {TABLE_COLUMNS.Length}");
				}

				// Podstawowa walidacja
				if (invoice == null)
					throw new ArgumentNullException(nameof(invoice), "Invoice cannot be null");
				if (invoice.issuer == null)
					throw new ArgumentNullException(nameof(invoice.issuer), "Invoice issuer cannot be null");
				if (invoice.products == null)
					throw new ArgumentNullException(nameof(invoice.products), "Invoice products cannot be null");

				// Dodatkowa walidacja
				var validProducts = invoice.products.Where(p => p != null).ToList();
				if (!validProducts.Any())
					throw new InvalidOperationException("Invoice must have at least one valid product");

				using var document = new PdfDocument();
				XGraphics gfx = null;
				PdfPage page = null;

				void CreateNewPage()
				{
					try
					{
						Console.WriteLine("Creating new page...");
						gfx?.Dispose();
						page = document.AddPage();
						page.Size = PdfSharpCore.PageSize.A4;
						gfx = XGraphics.FromPdfPage(page);
						Console.WriteLine($"New page created successfully. Size: {page.Width}x{page.Height}");
					}
					catch (Exception ex)
					{
						Console.WriteLine($"Error creating new page: {ex.Message}\nStack trace: {ex.StackTrace}");
						throw;
					}
				}

				double GetSafeY(double currentY)
				{
					if (page == null) return PAGE_MARGIN;
					var maxY = page.Height - PAGE_MARGIN_BOTTOM;
					var safeY = Math.Min(Math.Max(currentY, PAGE_MARGIN), maxY);
					if (safeY != currentY)
					{
						Console.WriteLine($"Adjusted Y coordinate from {currentY} to {safeY}");
					}
					return safeY;
				}

				bool NeedsNewPage(double currentY)
				{
					if (page == null) return true;
					var needsNew = currentY > page.Height - PAGE_MARGIN_BOTTOM;
					if (needsNew)
					{
						Console.WriteLine($"New page needed. Current Y: {currentY}, Page Height: {page.Height}, Margin: {PAGE_MARGIN_BOTTOM}");
					}
					return needsNew;
				}

				void EnsureValidGraphicsContext()
				{
					if (gfx == null || page == null)
					{
						Console.WriteLine("Graphics context or page is null, creating new page");
						CreateNewPage();
					}
				}

				void SafeDrawString(string text, XFont font, double x, double y, bool isBold = false)
				{
					try
					{
						EnsureValidGraphicsContext();
						if (y < PAGE_MARGIN || y > page.Height - PAGE_MARGIN_BOTTOM)
						{
							Console.WriteLine($"Warning: Attempting to draw outside safe area. Y: {y}, Text: {text}");
							y = GetSafeY(y);
						}
						gfx.DrawString(text ?? "---", font, XBrushes.Black, x, y);
					}
					catch (Exception ex)
					{
						Console.WriteLine($"Error drawing string '{text}' at ({x}, {y}): {ex.Message}");
						throw;
					}
				}

				void SafeDrawLine(double x1, double y1, double x2, double y2)
				{
					try
					{
						EnsureValidGraphicsContext();
						if (y1 < PAGE_MARGIN || y1 > page.Height - PAGE_MARGIN_BOTTOM ||
							y2 < PAGE_MARGIN || y2 > page.Height - PAGE_MARGIN_BOTTOM)
						{
							Console.WriteLine($"Warning: Attempting to draw line outside safe area. Y1: {y1}, Y2: {y2}");
							y1 = GetSafeY(y1);
							y2 = GetSafeY(y2);
						}
						gfx.DrawLine(XPens.Black, x1, y1, x2, y2);
					}
					catch (Exception ex)
					{
						Console.WriteLine($"Error drawing line from ({x1}, {y1}) to ({x2}, {y2}): {ex.Message}");
						throw;
					}
				}

				CreateNewPage(); // Create first page
				EnsureValidGraphicsContext();

				// Czcionki
				XFont regularFont, boldFont, headerFont;
				try
				{
					Console.WriteLine("Creating font instances...");
					
					// Try OpenSans first
					try
					{
						regularFont = new XFont("OpenSans", 10, XFontStyle.Regular);
						Console.WriteLine("Created regular font");
						
						boldFont = new XFont("OpenSans", 10, XFontStyle.Bold);
						Console.WriteLine("Created bold font");
						
						headerFont = new XFont("OpenSans", 16, XFontStyle.Bold);
						Console.WriteLine("Created header font");
						
						Console.WriteLine("Successfully created all OpenSans fonts");
					}
					catch (Exception ex)
					{
						Console.WriteLine($"Failed to create OpenSans fonts: {ex.Message}");
						Console.WriteLine("Falling back to Arial fonts");
						
						// Fallback to Arial
						regularFont = new XFont("Arial", 10, XFontStyle.Regular);
						Console.WriteLine("Created regular Arial font");
						
						boldFont = new XFont("Arial", 10, XFontStyle.Bold);
						Console.WriteLine("Created bold Arial font");
						
						headerFont = new XFont("Arial", 16, XFontStyle.Bold);
						Console.WriteLine("Created header Arial font");
						
						Console.WriteLine("Successfully created all Arial fonts");
					}

					// Verify fonts are usable
					using (var testDoc = new PdfDocument())
					{
						var testPage = testDoc.AddPage();
						using (var testGfx = XGraphics.FromPdfPage(testPage))
						{
							testGfx.DrawString("Test", regularFont, XBrushes.Black, 0, 10);
							testGfx.DrawString("Test", boldFont, XBrushes.Black, 0, 20);
							testGfx.DrawString("Test", headerFont, XBrushes.Black, 0, 30);
						}
					}
					Console.WriteLine("Font verification successful");
				}
				catch (Exception ex)
				{
					Console.WriteLine($"Critical error creating fonts: {ex.Message}");
					Console.WriteLine($"Stack trace: {ex.StackTrace}");
					throw new Exception("Failed to create usable fonts", ex);
				}

				double y = PAGE_MARGIN;

				// Print debug info
				Console.WriteLine($"Page dimensions: Width={page.Width}, Height={page.Height}, Orientation={page.Orientation}");
				Console.WriteLine($"Number of products: {validProducts.Count}");
				Console.WriteLine($"Table columns: {string.Join(", ", TABLE_COLUMNS)}");

				// Title
				var titleText = "VAT Invoice";
				var titleSize = gfx.MeasureString(titleText, headerFont);
				gfx.DrawString(titleText, headerFont, XBrushes.Black, 
					new XPoint((page.Width - titleSize.Width) / 2, y));
				y += SECTION_SPACING;

				// Seller data
				if (NeedsNewPage(y + LINE_HEIGHT * 6))
				{
					CreateNewPage();
					EnsureValidGraphicsContext();
				}
				y = GetSafeY(y);

				gfx.DrawString("Seller:", boldFont, XBrushes.Black, PAGE_MARGIN, y);
				y += LINE_HEIGHT;
				gfx.DrawString(invoice.issuer.name ?? "---", regularFont, XBrushes.Black, PAGE_MARGIN, y);
				y += LINE_HEIGHT;
				gfx.DrawString($"{invoice.issuer.street ?? "---"} {invoice.issuer.houseNumber.ToString()}/{(invoice.issuer.apartmentNumber.HasValue ? invoice.issuer.apartmentNumber.Value.ToString() : "")}", regularFont, XBrushes.Black, PAGE_MARGIN, y);
				y += LINE_HEIGHT;
				gfx.DrawString($"{invoice.issuer.postalCode ?? "---"} {invoice.issuer.city ?? "---"}", regularFont, XBrushes.Black, PAGE_MARGIN, y);
				y += LINE_HEIGHT;
				gfx.DrawString(invoice.issuer.country ?? "---", regularFont, XBrushes.Black, PAGE_MARGIN, y);
				y += LINE_HEIGHT;
				y += SECTION_SPACING;

				// Buyer data
				if (NeedsNewPage(y + LINE_HEIGHT * 6))
				{
					CreateNewPage();
					EnsureValidGraphicsContext();
				}
				y = GetSafeY(y);

				gfx.DrawString("Buyer:", boldFont, XBrushes.Black, 300, y);
				y += LINE_HEIGHT;
				gfx.DrawString((invoice.clientName ?? "---") , regularFont, XBrushes.Black, 300, y);
				y += LINE_HEIGHT;
				gfx.DrawString($"{invoice.clientStreet ?? "---"} {invoice.clientHouseNumber.ToString()}/{(invoice.clientApartmentNumber.HasValue ? invoice.clientApartmentNumber.Value.ToString() : "")}", regularFont, XBrushes.Black, 300, y);
				y += LINE_HEIGHT;
				gfx.DrawString($"{invoice.clientPostalCode ?? "---"} {invoice.clientCity ?? "---"}", regularFont, XBrushes.Black, 300, y);
				y += LINE_HEIGHT;
				gfx.DrawString(invoice.clientCountry ?? "---", regularFont, XBrushes.Black, 300, y);
				y += LINE_HEIGHT;

				// General data
				if (NeedsNewPage(y + LINE_HEIGHT * 3))
				{
					CreateNewPage();
					EnsureValidGraphicsContext();
				}
				y = GetSafeY(y);

				gfx.DrawString($"Issue Date: {invoice.issueDate:yyyy-MM-dd}", regularFont, XBrushes.Black, PAGE_MARGIN, y);
				y += LINE_HEIGHT;
				gfx.DrawString($"Due Date: {invoice.paymentDeadline:yyyy-MM-dd}", regularFont, XBrushes.Black, PAGE_MARGIN, y);
				y += LINE_HEIGHT;
				gfx.DrawString($"Payment Method: {invoice.paymentType ?? "---"}", regularFont, XBrushes.Black, PAGE_MARGIN, y);
				y += SECTION_SPACING;

				// Tabela produktów
				if (NeedsNewPage(y + LINE_HEIGHT * 2))
				{
					CreateNewPage();
					EnsureValidGraphicsContext();
				}
				y = GetSafeY(y);

				DrawTableHeader(gfx, boldFont, y);
				y += LINE_HEIGHT;

				// Produkty
				try {
					Console.WriteLine("Starting products section");
					foreach (var (product, index) in validProducts.Select((p, i) => (p, i + 1)))
					{
						if (product == null)
						{
							Console.WriteLine($"Warning: Skipping null product at index {index}");
							continue;
						}

						Console.WriteLine($"Processing product {index} at y-coordinate: {y}");
						Console.WriteLine($"Product details: ID={product.product_id}, Quantity={product.quantity}, Price={product.totalPrice}");
						
						if (NeedsNewPage(y + LINE_HEIGHT))
						{
							Console.WriteLine($"Page height: {page.Height}, Current y: {y}, Creating new page");
							CreateNewPage();
							EnsureValidGraphicsContext();
							y = PAGE_MARGIN;
							DrawTableHeader(gfx, boldFont, y);
							y += LINE_HEIGHT;
						}
						DrawProductRow(gfx, regularFont, y, product, index);
						y += LINE_HEIGHT;
					}
					Console.WriteLine("Finished processing products");
				} catch (Exception ex) {
					Console.WriteLine($"Error in products loop: {ex.Message}");
					Console.WriteLine($"Stack trace: {ex.StackTrace}");
					throw;
				}

				// Podsumowanie
				try {
					Console.WriteLine("Starting summary section");
					if (NeedsNewPage(y + LINE_HEIGHT * 4))
					{
						Console.WriteLine($"Page height: {page.Height}, Current y: {y}, Creating new page for summary");
						CreateNewPage();
						EnsureValidGraphicsContext();
						y = PAGE_MARGIN;
					}
					y += SECTION_SPACING;
					DrawSummary(gfx, validProducts, boldFont, regularFont, y);
					Console.WriteLine("Finished summary section");
				} catch (Exception ex) {
					Console.WriteLine($"Error in summary: {ex.Message}");
					Console.WriteLine($"Stack trace: {ex.StackTrace}");
					throw;
				}

				var stream = new MemoryStream();
				Console.WriteLine("Saving PDF document to stream...");
				document.Save(stream, false);
				stream.Position = 0;
				gfx?.Dispose();
				Console.WriteLine("PDF generation completed successfully");
				return stream;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"PDF Generation internal error: {ex.Message}");
				Console.WriteLine($"Stack trace: {ex.StackTrace}");
				throw new Exception($"Error generating PDF: {ex.Message}", ex);
			}
		}

		private static void DrawTableHeader(XGraphics gfx, XFont font, double y)
		{
			if (gfx == null) throw new ArgumentNullException(nameof(gfx));
			if (font == null) throw new ArgumentNullException(nameof(font));

			try
			{
				if (TABLE_COLUMNS == null || TABLE_COLUMNS.Length < TABLE_HEADERS.Length)
				{
					throw new InvalidOperationException($"Table columns array is not properly initialized. Columns: {TABLE_COLUMNS?.Length ?? 0}, Headers needed: {TABLE_HEADERS.Length}");
				}

				for (int i = 0; i < TABLE_HEADERS.Length; i++)
				{
					if (i >= TABLE_COLUMNS.Length)
					{
						Console.WriteLine($"Warning: Skipping header {TABLE_HEADERS[i]} due to missing column position");
						continue;
					}
					gfx.DrawString(TABLE_HEADERS[i], font, XBrushes.Black, TABLE_COLUMNS[i], y);
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error drawing table header at y={y}: {ex.Message}");
				throw;
			}
		}

		private static void DrawProductRow(XGraphics gfx, XFont font, double y, ProductInfo product, int index)
		{
			if (gfx == null) throw new ArgumentNullException(nameof(gfx));
			if (font == null) throw new ArgumentNullException(nameof(font));
			if (product == null) throw new ArgumentNullException(nameof(product));

			try
			{
				if (TABLE_COLUMNS == null || TABLE_COLUMNS.Length < 7)
				{
					throw new InvalidOperationException($"Table columns array is not properly initialized. Length: {TABLE_COLUMNS?.Length ?? 0}");
				}

				var values = new[]
				{
					index.ToString(),
					product.quantity ?? "0",
					$"₪ {product.net.ToString("F2", CultureInfo.InvariantCulture)}",
					$"{product.tax}%",
					$"₪ {product.taxAmount.ToString("F2", CultureInfo.InvariantCulture)}",
					$"₪ {product.gross.ToString("F2", CultureInfo.InvariantCulture)}",
					$"₪ {product.totalPrice.ToString("F2", CultureInfo.InvariantCulture)}"
				};

				if (values.Length > TABLE_COLUMNS.Length)
				{
					Console.WriteLine($"Warning: Some product values will be skipped. Values: {values.Length}, Columns: {TABLE_COLUMNS.Length}");
				}

				for (int i = 0; i < Math.Min(values.Length, TABLE_COLUMNS.Length); i++)
				{
					try
					{
						gfx.DrawString(values[i], font, XBrushes.Black, TABLE_COLUMNS[i], y);
					}
					catch (Exception ex)
					{
						Console.WriteLine($"Error drawing value at column {i}: {ex.Message}");
						throw;
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error drawing product row at y={y}: {ex.Message}");
				throw;
			}
		}

		private static void DrawSummary(XGraphics gfx, List<ProductInfo> products, XFont boldFont, XFont regularFont, double y)
		{
			if (gfx == null) throw new ArgumentNullException(nameof(gfx));
			if (products == null) throw new ArgumentNullException(nameof(products));
			if (boldFont == null) throw new ArgumentNullException(nameof(boldFont));
			if (regularFont == null) throw new ArgumentNullException(nameof(regularFont));

			try
			{
				if (!products.Any()) return;

				var validProducts = products.Where(p => p != null).ToList();
				if (!validProducts.Any())
				{
					Console.WriteLine("Warning: No valid products for summary");
					return;
				}

				Console.WriteLine($"Drawing summary line at y={y}");
				gfx.DrawLine(XPens.Black, TABLE_START_X, y, TABLE_START_X + TABLE_WIDTH, y);
				y += 10;

				var sumNet = validProducts.Sum(p => p.net);
				var sumVat = validProducts.Sum(p => p.taxAmount);
				var sumGross = validProducts.Sum(p => p.totalPrice);

				Console.WriteLine($"Summary totals - Net: {sumNet:F2}, VAT: {sumVat:F2}, Gross: {sumGross:F2}");

				var summaryItems = new[]
				{
					(label: "Net Total:", value: $"₪ {sumNet.ToString("F2", CultureInfo.InvariantCulture)}"),
					(label: "VAT Total:", value: $"₪ {sumVat.ToString("F2", CultureInfo.InvariantCulture)}"),
					(label: "Gross Total:", value: $"₪ {sumGross.ToString("F2", CultureInfo.InvariantCulture)}")
				};

				foreach (var (label, value) in summaryItems)
				{
					gfx.DrawString(label, boldFont, XBrushes.Black, 300, y);
					gfx.DrawString(value, regularFont, XBrushes.Black, 400, y);
					y += LINE_HEIGHT;
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error drawing summary at y={y}: {ex.Message}");
				throw;
			}
		}
	}
}
