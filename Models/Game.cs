namespace GameStore.Models;

public class Game
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateOnly ReleaseDate { get; set; }

    public ICollection<Product> Products { get; set; } = new List<Product>();
    public ICollection<GameGenre> GameGenres { get; set; } = new List<GameGenre>();
    public ICollection<GamePublisher> GamePublishers { get; set; } = new List<GamePublisher>();
    public ICollection<GameDeveloper> GameDevelopers { get; set; } = new List<GameDeveloper>();
    public ICollection<GameImage> Images { get; set; } = new List<GameImage>();
}