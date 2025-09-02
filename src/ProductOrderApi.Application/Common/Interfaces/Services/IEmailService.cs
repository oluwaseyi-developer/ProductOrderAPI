namespace ProductOrderApi.Application.Common.Interfaces.Services
{
    public interface IEmailService
    {
        Task SendOrderConfirmationAsync(string userId, int orderId);
        Task SendLowStockAlertAsync(int productId, string productName, int remainingStock);
    }
}
