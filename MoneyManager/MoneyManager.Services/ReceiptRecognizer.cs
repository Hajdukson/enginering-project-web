using Azure;
using Azure.AI.FormRecognizer.DocumentAnalysis;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using MoneyManager.Models;
using MoneyManager.Services.Interfeces;
using System.ComponentModel;

namespace MoneyManager.Services
{
    public class ReceiptRecognizer : IReceiptRecognizer
    {
        AzureKeyCredential _credential;
        DocumentAnalysisClient _client;
        public ReceiptRecognizer(IConfiguration configuration)
        {
            var key = configuration.GetValue<string>("FR.Settings:FR_KEY");
            var endpoint = configuration.GetValue<string>("FR.Settings:FR_ENDPOINT");
            _credential = new AzureKeyCredential(key);
            _client = new DocumentAnalysisClient(new Uri(endpoint), credential: _credential);
        }
        public async Task<List<BoughtProduct>> AnalizeImage(FileStream fileStreams)
        {
            AnalyzeDocumentOperation operation = await _client.AnalyzeDocumentAsync(WaitUntil.Completed, "prebuilt-receipt", fileStreams);
            AnalyzeResult receipts = operation.Value;

            var boughtProducts = new List<BoughtProduct>();
            DateTimeOffset transactionDate = DateTime.Now;

            AnalyzedDocument receipt = receipts.Documents[0];

            if (receipt.Fields.TryGetValue("Items", out DocumentField itemsField))
            {
                if (itemsField.FieldType == DocumentFieldType.List)
                {
                    if (receipt.Fields.TryGetValue("TransactionDate", out DocumentField transactionDateField))
                    {
                        if (transactionDateField.FieldType == DocumentFieldType.Date)
                        {
                            transactionDate = transactionDateField.Value.AsDate();

                            Console.WriteLine($"Transaction Date: '{transactionDate}', with confidence {transactionDateField.Confidence}");
                        }
                    }

                    foreach (DocumentField itemField in itemsField.Value.AsList())
                    {
                        var boughtProduct = new BoughtProduct();
                        Console.WriteLine("Item:");
                        boughtProduct.BoughtDate = transactionDate.UtcDateTime;

                        if (itemField.FieldType == DocumentFieldType.Dictionary)
                        {
                            IReadOnlyDictionary<string, DocumentField> itemFields = itemField.Value.AsDictionary();

                            if (itemFields.TryGetValue("Description", out DocumentField itemDescriptionField))
                            {
                                if (itemDescriptionField.FieldType == DocumentFieldType.String)
                                {
                                    string itemDescription = itemDescriptionField.Value.AsString();

                                    boughtProduct.Name = itemDescription;

                                    Console.WriteLine($"  Description: '{itemDescription}', with confidence {itemDescriptionField.Confidence}");
                                }
                            }

                            if (itemFields.TryGetValue("Price", out DocumentField itemTotalPriceField))
                            {
                                if (itemTotalPriceField.FieldType == DocumentFieldType.Double)
                                {
                                    double itemTotalPrice = itemTotalPriceField.Value.AsDouble();

                                    boughtProduct.Price = (decimal)itemTotalPrice;

                                    Console.WriteLine($"  Total Price: '{itemTotalPrice}', with confidence {itemTotalPriceField.Confidence}");
                                }
                            }
                            boughtProducts.Add(boughtProduct);
                        }
                    }
                }
            }
            return boughtProducts;
        }
    }
}
