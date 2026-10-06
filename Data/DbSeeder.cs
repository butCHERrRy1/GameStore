using GameStore.Models;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(GameStoreDbContext db)
    {
        // при наличии данных ничего не делать
        if (await db.Platforms.AnyAsync())
            return;

        // платформы
        var ps5 = new Platform { Name = "PlayStation 5", Manufacturer = "Sony" };
        var xbox = new Platform { Name = "Xbox Series X", Manufacturer = "Microsoft" };
        var switchPlatform = new Platform { Name = "Nintendo Switch", Manufacturer = "Nintendo" };
        db.Platforms.AddRange(ps5, xbox, switchPlatform);

        // жагры
        var action = new Genre { Name = "Action" };
        var rpg = new Genre { Name = "RPG" };
        var adventure = new Genre { Name = "Adventure" };
        var shooter = new Genre { Name = "Shooter" };
        db.Genres.AddRange(action, rpg, adventure, shooter);

        // издатеои
        var sony = new Publisher { Name = "Sony Interactive Entertainment" };
        var cdpr = new Publisher { Name = "CD Projekt" };
        var rockstar = new Publisher { Name = "Rockstar Games" };
        var nintendo = new Publisher { Name = "Nintendo" };
        db.Publishers.AddRange(sony, cdpr, rockstar, nintendo);

        // разработчики
        var guerilla = new Developer { Name = "Guerrilla Games" };
        var cdprDev = new Developer { Name = "CD Projekt Red" };
        var rockstarDev = new Developer { Name = "Rockstar North" };
        var nintendoDev = new Developer { Name = "Nintendo EPD" };
        db.Developers.AddRange(guerilla, cdprDev, rockstarDev, nintendoDev);

        await db.SaveChangesAsync();

        // игры
        var horizon = new Game
        {
            Title = "Horizon Forbidden West",
            Description = "Открытый мир, роботы-динозавры и Элой на тропе войны.",
            ReleaseDate = new DateOnly(2022, 2, 18)
        };

        var cyberpunk = new Game
        {
            Title = "Cyberpunk 2077",
            Description = "Найт-Сити, импланты и погоня за бессмертием.",
            ReleaseDate = new DateOnly(2020, 12, 10)
        };

        var gta5 = new Game
        {
            Title = "Grand Theft Auto V",
            Description = "Три героя, Лос-Сантос и бесконечный онлайн.",
            ReleaseDate = new DateOnly(2013, 9, 17)
        };

        var zelda = new Game
        {
            Title = "The Legend of Zelda: Tears of the Kingdom",
            Description = "Хайрул, небесные острова и новые силы Линка.",
            ReleaseDate = new DateOnly(2023, 5, 12)
        };

        db.Games.AddRange(horizon, cyberpunk, gta5, zelda);
        await db.SaveChangesAsync();

        // связи Game <-> Genre 
        db.GameGenres.AddRange(
            new GameGenre { GameId = horizon.Id, GenreId = action.Id },
            new GameGenre { GameId = horizon.Id, GenreId = rpg.Id },
            new GameGenre { GameId = cyberpunk.Id, GenreId = rpg.Id },
            new GameGenre { GameId = cyberpunk.Id, GenreId = shooter.Id },
            new GameGenre { GameId = gta5.Id, GenreId = action.Id },
            new GameGenre { GameId = gta5.Id, GenreId = adventure.Id },
            new GameGenre { GameId = zelda.Id, GenreId = adventure.Id },
            new GameGenre { GameId = zelda.Id, GenreId = rpg.Id }
        );

        // связи Game <-> Publisher 
        db.GamePublishers.AddRange(
            new GamePublisher { GameId = horizon.Id, PublisherId = sony.Id },
            new GamePublisher { GameId = cyberpunk.Id, PublisherId = cdpr.Id },
            new GamePublisher { GameId = gta5.Id, PublisherId = rockstar.Id },
            new GamePublisher { GameId = zelda.Id, PublisherId = nintendo.Id }
        );

        // связи Game <-> Developer
        db.GameDevelopers.AddRange(
            new GameDeveloper { GameId = horizon.Id, DeveloperId = guerilla.Id },
            new GameDeveloper { GameId = cyberpunk.Id, DeveloperId = cdprDev.Id },
            new GameDeveloper { GameId = gta5.Id, DeveloperId = rockstarDev.Id },
            new GameDeveloper { GameId = zelda.Id, DeveloperId = nintendoDev.Id }
        );

        // изображения 
        db.GameImages.AddRange(
            new GameImage { GameId = horizon.Id, Url = "/images/horizon.jpg", Alt = "Horizon Forbidden West" },
            new GameImage { GameId = cyberpunk.Id, Url = "/images/cyberpunk.jpg", Alt = "Cyberpunk 2077" },
            new GameImage { GameId = gta5.Id, Url = "/images/gta5.jpg", Alt = "GTA V" },
            new GameImage { GameId = zelda.Id, Url = "/images/zelda.jpg", Alt = "Zelda TotK" }
        );

        // товары (Game + Platform + Price + Stock)
        db.Products.AddRange(
            new Product { GameId = horizon.Id, PlatformId = ps5.Id, Price = 4999m, Stock = 10, IsAvailable = true },
            new Product { GameId = cyberpunk.Id, PlatformId = ps5.Id, Price = 3499m, Stock = 8, IsAvailable = true },
            new Product { GameId = cyberpunk.Id, PlatformId = xbox.Id, Price = 3499m, Stock = 5, IsAvailable = true },
            new Product { GameId = gta5.Id, PlatformId = ps5.Id, Price = 2999m, Stock = 12, IsAvailable = true },
            new Product { GameId = gta5.Id, PlatformId = xbox.Id, Price = 2999m, Stock = 7, IsAvailable = true },
            new Product { GameId = zelda.Id, PlatformId = switchPlatform.Id, Price = 4499m, Stock = 15, IsAvailable = true }
        );

        await db.SaveChangesAsync();
    }
}