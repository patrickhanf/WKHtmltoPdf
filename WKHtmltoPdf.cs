using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// https://stackoverflow.com/questions/10292689/wkhtmltopdf-outputstream-download-diaglog
/// </summary>
namespace Prh.Utilities.HtmltoPdf
{
    public class WKHtmltoPdf
    {
        public WKHtmltoPdf()
        {
        }

        public static async Task<byte[]> GeneratePdf(string html, Size pageSize)
        {
            var directory = AppContext.BaseDirectory; // 

            var toolFilepath = Path.Combine(directory, "Utilities", "HtmltoPdf", "wkhtmltopdf.exe");

            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = toolFilepath, // Path example: @"C:\PROGRA~1\WKHTML~1\wkhtmltopdf.exe"
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                Arguments = "-q -n --disable-smart-shrinking "
                    + (pageSize.IsEmpty ? "" : "--page-width " + pageSize.Width
                    + "mm --page-height " + pageSize.Height + "mm") + " - -",
            };

            using var p = Process.Start(psi);
            using var pdfSream = new MemoryStream();
            using (var utf8Writer = new StreamWriter(p.StandardInput.BaseStream, Encoding.UTF8))
            {
                await utf8Writer.WriteAsync(html);
                utf8Writer.Close();
                var tStandardOutput = p.StandardOutput.BaseStream.CopyToAsync(pdfSream);
                var tStandardError = p.StandardError.ReadToEndAsync();

                await tStandardOutput;
                string errors = await tStandardError;

                if (!string.IsNullOrEmpty(errors))
                {
                    //deal with errors
                }
                return pdfSream.ToArray();
            }

        }
    } 
} // name
