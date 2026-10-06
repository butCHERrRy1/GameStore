namespace GameStore.Models;

public class GameImage
{
    public int Id { get; set; }

    public int GameId { get; set; }
    public Game Game { get; set; } = null!;

    public string Url { get; set; } = string.Empty;
    public string Alt { get; set; } = string.Empty;
}