namespace GameStore.Models;

public class Platform
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Manufacturer { get; set; } = string.Empty;

    public ICollection<Product> Products { get; set; } = new List<Product>();
}