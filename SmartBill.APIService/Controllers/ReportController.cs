﻿using Microsoft.AspNetCore.Mvc;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace SmartBill.APIService.Controllers
{
    [ApiController]
    [Route("SBILL/[controller]")]
    public class ReportController : ControllerBase
    {
        [HttpGet("TestReport")]
        public IActionResult GenerateAndShowPdf()
        {
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(20);

                    // Header
                    page.Header()
                        .Text("Sales Report")
                        .FontSize(20)
                        .SemiBold()
                        .AlignCenter();

                    // Table
                    page.Content()
                        .Column(column =>
                        {
                            column.Item().Text("");
                            column.Item().Table(table =>
                            {
                                // Table Columns
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn();
                                    columns.RelativeColumn();
                                    columns.RelativeColumn();
                                });

                                // Table Header
                                table.Header(header =>
                                {
                                    header.Cell().Element(HeaderStyle).Text("ID").Bold().FontSize(12);
                                    header.Cell().Element(HeaderStyle).Text("Name").Bold().FontSize(12);
                                    header.Cell().Element(HeaderStyle).Text("Amount").Bold().FontSize(12);
                                });

                                // Table Data
                                var data = new List<(int Id, string Name, decimal Amount)>
                                {
                                (1, "Product A", 100.50m),
                                (2, "Product B", 200.75m),
                                (3, "Product C", 50.00m)
                                };

                                foreach (var item in data)
                                {
                                    table.Cell().Element(CellStyle).Text(item.Id.ToString());
                                    table.Cell().Element(CellStyle).Text(item.Name);
                                    table.Cell().Element(CellStyle).Text($"${item.Amount:F2}").AlignRight();
                                }
                            });
                        });

                    // Footer
                    page.Footer()
                        .Row(row =>
                        {
                            row.RelativeItem().AlignLeft().AlignMiddle().Text("Generated by Atik").FontSize(9);
                            row.RelativeItem().AlignRight().AlignMiddle().Text($"Generated on {DateTime.Now:yyyy-MM-dd HH:mm:ss}").FontSize(9);
                        });
                });
            });

            //document.GeneratePdfAndShow();
            using var stream = new MemoryStream();
            document.GeneratePdf(stream);
            return File(stream.ToArray(), "application/pdf", $"report_{DateTime.Now:yyyyMMddHHmmss}.pdf");
        }

        private static QuestPDF.Infrastructure.IContainer HeaderStyle(IContainer container)
        {
            return container
                .Background(Colors.Grey.Lighten3)
                .Border(1)
                .BorderColor(Colors.Grey.Darken1)
                .AlignCenter()
                .AlignMiddle();
        }

        private static IContainer CellStyle(IContainer container)
        {
            return container
                .Border(1)
                .BorderColor(Colors.Grey.Darken1)
                .Padding(5);
        }
    }
}
