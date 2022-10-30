using MoneyManager.Models;

namespace MoneyManager.Services.Interfeces
{
    public interface IReceiptRecognizer
    {
        Task<List<BoughtProduct>> AnalizeImage(FileStream fileStreams);
    }
}