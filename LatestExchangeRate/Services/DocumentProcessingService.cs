using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using LatestExchangeRate.Constans;
using LatestExchangeRate.Interfaces;
using LatestExchangeRate.Models;
using Newtonsoft.Json;

namespace LatestExchangeRate.Services
{
    public class DocumentProcessingService : IDocumentProcessing
    {
        private readonly IConsumerService _consumerService;

        public DocumentProcessingService(IConsumerService consumerService)
        {
            _consumerService = consumerService;
        }

        public void WriteResponseToFile()
        {
            try
            {
                var response = GetFixerRestClientResponse();
                string filePath = RestClientConstants.FilePathOfDocument;

                using (WordprocessingDocument myDocument = WordprocessingDocument.Create(filePath, WordprocessingDocumentType.Document))
                {
                    MainDocumentPart mainDocumentPart = myDocument.AddMainDocumentPart();
                    mainDocumentPart.Document = new Document();
                    Body body = mainDocumentPart.Document.AppendChild(new Body());

                    Paragraph p = new Paragraph();
                    
                    ParagraphProperties pp = new ParagraphProperties();
                    pp.Justification = new Justification() { Val = JustificationValues.Center };
                    p.AppendChild(pp);

                    Run r = new Run();
                    
                    RunProperties rp = new RunProperties();
                    rp.FontSize = new FontSize() { Val = "28" };
                    rp.Color = new Color() { Val = "FF0000" };
                    r.AppendChild(rp);
                    r.AppendChild(new Text("Latest Exchange Rate"));
                    p.AppendChild(r);
                    body.AppendChild(p);

                    Paragraph paragraph = new Paragraph();
                    Run run = new Run();
                    var date = response.Date.ToString("yyyy-MM-dd");
                    var textDate = $"This information has been fetched from Fixer API at {date}";
                    run.AppendChild(new Text(textDate));
                    paragraph.AppendChild(run);
                    body.AppendChild(paragraph);

                    Paragraph paragraph1 = new Paragraph();
                    Run run1 = new Run();
                    var textBase = $"Base Currency = {response.Base}";
                    run1.AppendChild(new Text(textBase));
                    paragraph1.AppendChild(run1);
                    body.AppendChild(paragraph1);

                    var table = new Table();
                    var tableProperties = new TableProperties();
                    var tableWidth = new TableWidth() { Width = "0", Type = TableWidthUnitValues.Auto };
                    tableProperties.Append(tableWidth);
                    table.AppendChild(tableProperties);

                    var count = 0;
                    var customColor = "00FF00";
                    var defaultColor = "";
                    var color = "";

                    foreach (var item in response.Rates)
                    {
                        if (count == 0)
                        {
                            color = defaultColor;
                            count++;
                        }
                        else
                        {
                            color = customColor;
                            count--;
                        }

                        var tr = new TableRow();
                        var tc1 = new TableCell();
                        var tc2 = new TableCell();

                        var p1 = new Paragraph();
                        var r1 = new Run();
                        RunProperties runprop = new RunProperties();
                        runprop.Color = new Color() { Val = color };
                        r1.AppendChild(runprop);
                        var t1 = new Text($"{item.Key} - ");
                        r1.AppendChild(t1);
                        p1.AppendChild(r1);
                        tc1.AppendChild(p1);

                        var p2 = new Paragraph();
                        var r2 = new Run();
                        RunProperties runp = new RunProperties();
                        runp.Color = new Color() { Val = color };
                        r2.AppendChild(runp);
                        var t2 = new Text(item.Value.ToString());
                        r2.AppendChild(t2);
                        

                        p2.AppendChild(r2);
                        tc2.AppendChild(p2);

                        tr.AppendChild(tc1);
                        tr.AppendChild(tc2);
                        table.AppendChild(tr);
                    }
                    body.AppendChild(table);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error writing response to file: {ex.Message}");
            }
        }

        private FixerRestClientResponse GetFixerRestClientResponse() 
        {
            var response = _consumerService.ReadMessage();

            return response;
        }
    }
}
