namespace GameStore.ViewModels;

public class CartItemViewModel
{
    public int ProductId { get; set; }
    public string GameTitle { get; set; } = string.Empty;
    public string PlatformName { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Quantity { get; set; }

    public decimal Total => Price * Quantity;
}