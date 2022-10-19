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
        IHostingEnvironment _environment;
        public ReceiptRecognizer(IConfiguration configuration, IHostingEnvironment environment)
        {
            var key = configuration.GetValue<string>("FR.Settings:FR_KEY");
            var endpoint = configuration.GetValue<string>("FR.Settings:FR_ENDPOINT");
            _credential = new AzureKeyCredential(key);
            _client = new DocumentAnalysisClient(new Uri(endpoint), credential: _credential);
            _environment = environment;
        }
        public async Task<List<BoughtProduct>> AnalizeImage(FileStream fileStreams)
        {
            AnalyzeDocumentOperation operation = await _client.AnalyzeDocumentAsync(WaitUntil.Completed, "prebuilt-receipt", fileStreams);
            AnalyzeResult receipts = operation.Value;

            var boughtProducts = new List<BoughtProduct>();
            DateTime tranDate;

            foreach (AnalyzedDocument receipt in receipts.Documents)
            {
                var boughtProduct = new BoughtProduct();

                if (receipt.Fields.TryGetValue("MerchantName", out DocumentField merchantNameField))
                {
                    if (merchantNameField.FieldType == DocumentFieldType.String)
                    {
                        string merchantName = merchantNameField.Value.AsString();

                        Console.WriteLine($"Merchant Name: '{merchantName}', with confidence {merchantNameField.Confidence}");
                    }
                }

                if (receipt.Fields.TryGetValue("TransactionDate", out DocumentField transactionDateField))
                {
                    if (transactionDateField.FieldType == DocumentFieldType.Date)
                    {
                        DateTimeOffset transactionDate = transactionDateField.Value.AsDate();

                        Console.WriteLine($"Transaction Date: '{transactionDate}', with confidence {transactionDateField.Confidence}");
                    }
                }

                if (receipt.Fields.TryGetValue("Items", out DocumentField itemsField))
                {
                    if (itemsField.FieldType == DocumentFieldType.List)
                    {
                        foreach (DocumentField itemField in itemsField.Value.AsList())
                        {
                            Console.WriteLine("Item:");

                            if (itemField.FieldType == DocumentFieldType.Dictionary)
                            {
                                IReadOnlyDictionary<string, DocumentField> itemFields = itemField.Value.AsDictionary();

                                if (itemFields.TryGetValue("Description", out DocumentField itemDescriptionField))
                                {
                                    if (itemDescriptionField.FieldType == DocumentFieldType.String)
                                    {
                                        string itemDescription = itemDescriptionField.Value.AsString();

                                        Console.WriteLine($"  Description: '{itemDescription}', with confidence {itemDescriptionField.Confidence}");
                                    }
                                }

                                if (itemFields.TryGetValue("TotalPrice", out DocumentField itemTotalPriceField))
                                {
                                    if (itemTotalPriceField.FieldType == DocumentFieldType.Double)
                                    {
                                        double itemTotalPrice = itemTotalPriceField.Value.AsDouble();

                                        Console.WriteLine($"  Total Price: '{itemTotalPrice}', with confidence {itemTotalPriceField.Confidence}");
                                    }
                                }
                            }
                        }
                    }
                }

                if (receipt.Fields.TryGetValue("Total", out DocumentField totalField))
                {
                    if (totalField.FieldType == DocumentFieldType.Double)
                    {
                        double total = totalField.Value.AsDouble();

                        Console.WriteLine($"Total: '{total}', with confidence '{totalField.Confidence}'");
                    }
                }
            }
            return boughtProducts;
        }
    }
}
