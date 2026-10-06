namespace GameStore.Models;

public class Product
{
    public int Id { get; set; }

    public int GameId { get; set; }
    public Game Game { get; set; } = null!;

    public int PlatformId { get; set; }
    public Platform Platform { get; set; } = null!;

    public decimal Price { get; set; }
    public int Stock { get; set; }
    public bool IsAvailable { get; set; } = true;
}