using System;
using System.IO;
using PdfSharpCore.Fonts;

namespace InvoiceService.Models.Services
{
    public static class PdfFontInitializer
    {
        private static bool _isInitialized = false;
        private static readonly object _lock = new object();

        public static void Initialize()
        {
            if (_isInitialized)
            {
                return;
            }

            lock (_lock)
            {
                if (_isInitialized)
                {
                    return;
                }

                try
                {
                    Console.WriteLine("Initializing PDF font system...");

                    var fontPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Fonts");
                    Console.WriteLine($"Font directory path: {fontPath}");

                    // Ensure font directory exists
                    if (!Directory.Exists(fontPath))
                    {
                        Console.WriteLine("Creating fonts directory...");
                        Directory.CreateDirectory(fontPath);
                    }

                    // Check for font files
                    var fontFiles = Directory.GetFiles(fontPath, "*.ttf");
                    Console.WriteLine($"Found {fontFiles.Length} font files:");
                    foreach (var file in fontFiles)
                    {
                        Console.WriteLine($"- {Path.GetFileName(file)}");
                    }

                    // Set the global font resolver
                    var resolver = new FontResolver();
                    GlobalFontSettings.FontResolver = resolver;

                    // Verify the resolver is set
                    if (GlobalFontSettings.FontResolver == null)
                    {
                        throw new InvalidOperationException("Font resolver was set but is still null");
                    }

                    // Test font resolution
                    try
                    {
                        var testFont = resolver.ResolveTypeface("OpenSans", false, false);
                        var fontBytes = resolver.GetFont(testFont.FaceName);
                        Console.WriteLine($"Successfully tested font resolution. Got {fontBytes.Length} bytes for test font.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Warning: Font resolution test failed: {ex.Message}");
                        Console.WriteLine("Will attempt to continue with default fonts.");
                    }

                    _isInitialized = true;
                    Console.WriteLine("PDF font system initialized successfully");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Critical error initializing PDF font system: {ex.Message}");
                    Console.WriteLine($"Stack trace: {ex.StackTrace}");
                    throw new InvalidOperationException("Failed to initialize PDF font system", ex);
                }
            }
        }
    }
} 