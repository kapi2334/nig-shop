using System;
using System.IO;
using System.Reflection;
using PdfSharpCore.Fonts;
using System.Collections.Generic;

namespace InvoiceService.Models.Services
{
    public class FontResolver : IFontResolver
    {
        private readonly string _fontDirectory;
        private const string DEFAULT_FONT = "OpenSans";

        public FontResolver()
        {
            _fontDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Fonts");
            EnsureFontDirectory();
        }

        public string DefaultFontName => DEFAULT_FONT;

        private void EnsureFontDirectory()
        {
            try
            {
                if (!Directory.Exists(_fontDirectory))
                {
                    Directory.CreateDirectory(_fontDirectory);
                    Console.WriteLine($"Created font directory: {_fontDirectory}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error ensuring font directory: {ex.Message}");
                throw;
            }
        }

        private byte[] LoadFontFromResources(string fontName)
        {
            try
            {
                var assembly = Assembly.GetExecutingAssembly();
                var resourceName = $"InvoiceService.Resources.Fonts.{fontName}";
                
                Console.WriteLine($"Attempting to load font from resources: {resourceName}");
                
                using (var stream = assembly.GetManifestResourceStream(resourceName))
                {
                    if (stream == null)
                    {
                        Console.WriteLine($"Resource not found: {resourceName}");
                        return null;
                    }

                    using (var ms = new MemoryStream())
                    {
                        stream.CopyTo(ms);
                        var fontBytes = ms.ToArray();
                        Console.WriteLine($"Successfully loaded font from resources: {resourceName}, Size: {fontBytes.Length} bytes");
                        return fontBytes;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading font from resources: {ex.Message}");
                return null;
            }
        }

        private byte[] LoadFontFromFile(string fontPath)
        {
            try
            {
                if (!File.Exists(fontPath))
                {
                    Console.WriteLine($"Font file not found: {fontPath}");
                    return null;
                }

                var fontBytes = File.ReadAllBytes(fontPath);
                Console.WriteLine($"Successfully loaded font from file: {fontPath}, Size: {fontBytes.Length} bytes");
                return fontBytes;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading font from file: {ex.Message}");
                return null;
            }
        }

        public byte[] GetFont(string faceName)
        {
            try
            {
                // First try loading from resources
                var fontBytes = LoadFontFromResources($"{faceName}.ttf");
                
                // If not found in resources, try loading from file
                if (fontBytes == null)
                {
                    var fontPath = Path.Combine(_fontDirectory, $"{faceName}.ttf");
                    fontBytes = LoadFontFromFile(fontPath);
                }

                if (fontBytes == null || fontBytes.Length < 1024) // Font files should be at least 1KB
                {
                    throw new FileNotFoundException($"Valid font file not found for {faceName}");
                }

                return fontBytes;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting font '{faceName}': {ex.Message}");
                throw;
            }
        }

        public FontResolverInfo ResolveTypeface(string familyName, bool isBold, bool isItalic)
        {
            try
            {
                string fontName;
                if (familyName.Equals("OpenSans", StringComparison.OrdinalIgnoreCase))
                {
                    fontName = isBold ? "OpenSans-Bold" : "OpenSans-Regular";
                }
                else
                {
                    Console.WriteLine($"Unknown font family '{familyName}', falling back to OpenSans");
                    fontName = isBold ? "OpenSans-Bold" : "OpenSans-Regular";
                }

                Console.WriteLine($"Resolved typeface: Family='{familyName}', Bold={isBold}, Italic={isItalic} -> '{fontName}'");
                return new FontResolverInfo(fontName);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error resolving typeface: {ex.Message}");
                throw;
            }
        }
    }
} 