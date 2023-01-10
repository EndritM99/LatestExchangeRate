using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using LatestExchangeRate.Interfaces;
using LatestExchangeRate.Models;
using Newtonsoft.Json;

namespace LatestExchangeRate.Services
{
    public class DocumentProcessingService : IDocumentProcessing
    {
        private readonly IRabbitMq _rabbitMqServicer;

        public DocumentProcessingService(IRabbitMq rabbitMqServicer)
        {
            _rabbitMqServicer = rabbitMqServicer;
        }

        public void WriteResponseToFile()
        {
            try
            {
                var response = GetFixerRestClientResponse();

                string filePath = @"C:\Users\endri\Desktop\GitHub Repos\LatestExchangeRate\LatestExchangeRate\DocumentStorage\FixerResponseTESTingAGAINFF.docx";

                using (WordprocessingDocument myDocument = WordprocessingDocument.Create(filePath, WordprocessingDocumentType.Document))
                {
                    MainDocumentPart mainDocumentPart = myDocument.AddMainDocumentPart();

                    mainDocumentPart.Document = new Document();
                    Body body = mainDocumentPart.Document.AppendChild(new Body());
                    Paragraph paragraph = body.AppendChild(new Paragraph());
                    Run run = paragraph.AppendChild(new Run());
                    Run run1 = paragraph.AppendChild(new Run());

                    //ApplyStyleToParagraph(myDocument, "OverdueAmount", "Overdue Amount", p);

                    string responseString = JsonConvert.SerializeObject(response);
                    Text text = new Text(responseString);

                    run.AppendChild(text);
                    run.AppendChild(new Text("Hello, World! Test"));
                    run.AppendChild(new Text("Hello, World! Test"));
                    run1.AppendChild(new Text("Hello, World! Testing"));
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error writing response to file: {ex.Message}");
            }
        }

        private FixerRestClientResponse GetFixerRestClientResponse() 
        {
            var response = _rabbitMqServicer.ReceiveMessage();

            return response;
        }
    }
}
