using Elements;
using Elements.Geometry;
using System.Collections.Generic;
using SkiaSharp;
using Svg.Skia;
using System;
using System.IO;

namespace PdfExporterSample
{
    public static class PdfExporterSample
    {
        /// <summary>
        /// The PdfExporterSample function.
        /// </summary>
        /// <param name="model">The input model.</param>
        /// <param name="input">The arguments to the execution.</param>
        /// <returns>A PdfExporterSampleOutputs instance containing computed results and the model with any new elements.</returns>
        public static PdfExporterSampleOutputs Execute(Dictionary<string, Model> inputModels, PdfExporterSampleInputs input)
        {
            var output = new PdfExporterSampleOutputs();

            var fileDest = input.DownloadPDF;

            var metadata = new SKDocumentPdfMetadata
            {
                Author = "Cool Developer",
                Creation = DateTime.Now,
                Creator = "Cool Developer Library",
                Keywords = "SkiaSharp, Sample, PDF, Developer, Library",
                Modified = DateTime.Now,
                Producer = "SkiaSharp",
                Subject = "SkiaSharp Sample PDF",
                Title = "Sample PDF",
            };
            // var pdfPath = System.IO.Path.GetTempFileName() + ".pdf";
            var stream = new MemoryStream(); // create a new stream
            using var document = SKDocument.CreatePdf(stream); //SKDocument.CreatePdf(pdfPath, metadata);

            using var paint = new SKPaint
            {
                TextSize = 64.0f,
                IsAntialias = true,
                Color = 0xFF9CAFB7,
                IsStroke = true,
                StrokeWidth = 3,
                TextAlign = SKTextAlign.Center
            };

            var pageWidth = 840;
            var pageHeight = 1188;

            using (var pdfCanvas = document.BeginPage(pageWidth, pageHeight))
            {
                // draw button
                using var nextPagePaint = new SKPaint
                {
                    IsAntialias = true,
                    TextSize = 16,
                    Color = SKColors.OrangeRed
                };
                var nextText = "Next Page >>";
                var btn = new SKRect(pageWidth - nextPagePaint.MeasureText(nextText) - 24, 0, pageWidth, nextPagePaint.TextSize + 24);
                pdfCanvas.DrawText(nextText, btn.Left + 12, btn.Bottom - 12, nextPagePaint);
                // make button link
                pdfCanvas.DrawLinkDestinationAnnotation(btn, "next-page");
                using (var svg = new SKSvg())
                {
                    if (svg.Load("lay and gray.svg") is { })
                    {
                        pdfCanvas.DrawPicture(svg.Picture);
                    }
                }
                // draw contents
                pdfCanvas.DrawText("...PDF 1/2...", pageWidth / 2, pageHeight / 4, paint);
                document.EndPage();
            }

            document.Close();

            fileDest.SetExportStream(stream);
            stream.Seek(0, SeekOrigin.Begin);
            return output;
        }
    }
}