# Overstarch
*Overstarch* is a .Net Core library written in Visual Basic that allows access to player information and stats for [Overwatch](https://en.wikipedia.org/wiki/Overwatch_(video_game)) by scraping user profiles and using hidden, non-public Blizzard APIs.

This library is intended to be a replacement for the no longer maintained [Overwatch.Net](https://github.com/sirdoombox/Overwatch.Net).

[![NuGet Latest Build](https://img.shields.io/nuget/vpre/Overstarch.svg?label=Latest%20Build&style=for-the-badge)](https://nuget.org/packages/Overstarch) [![NuGet Release Build](https://img.shields.io/nuget/v/Overstarch.svg?label=Latest%20Release&style=for-the-badge)](https://nuget.org/packages/Overstarch) [![Build Status](https://img.shields.io/travis/Giggitybyte/Overstarch/development.svg?style=for-the-badge)](https://travis-ci.org/Giggitybyte/Overstarch)

## Getting Started
Each method in *Overstarch* has docstrings, so it should be fairly easy to understand what each method does and how to use them.
Nevertheless, below are some snippets of code that will give you the general idea on how to use this library.


#### C Sharp
```csharp
        OverwatchClient owClient = new OverwatchClient();
        OverwatchPlayer owPlayer = await owClient.GetPlayerAsync("giggitybyte#11965", OverwatchPlatform.PC);

        // Basic player information example.
        Console.WriteLine($"Level {owPlayer.PlayerLevel}");
        Console.WriteLine($"Support SR: {owPlayer.SkillRatings.GetRole(OverwatchRole.Support)}");
        Console.WriteLine($"Role With Highest SR: {owPlayer.SkillRatings.GetHighestRole.Key.ToString}");
        Console.WriteLine($"Private Profile: {owPlayer.IsProfilePrivate}");

        // Player stats example.
        OverwatchStat compGamesWon = owPlayer.Stats(OverwatchGamemode.Competitive).GetStatExact("All Heroes", "Game", "Games Won");
        OverwatchStat qpTotalElims = owPlayer.Stats(OverwatchGamemode.Quickplay).GetStatExact("All Heroes", "Combat", "Eliminations");
        OverwatchStat brigDmgDone = owPlayer.Stats(OverwatchGamemode.Quickplay).GetStatExact("Brigitte", "Combat", "All Damage Done");

        Console.WriteLine($"Competitive Games Won: {compGamesWon.Value}");
        Console.WriteLine($"Quick Play Elims: {qpTotalElims.Value}");
        Console.WriteLine($"Brigitte Damage Dealt: {brigDmgDone.Value}");

        // Player achievements example.
        OverwatchAchievement undyingAchievement = owPlayer.Achievements.GetByName("Undying");
        OverwatchAchievement shutoutAchievement = owPlayer.Achievements.GetByName("Shutout");
        List<OverwatchAchievement> mapAchievements = owPlayer.Achievements.FilterByCategory(OverwatchAchievementCategory.Maps);

        Console.WriteLine($"Undying Achievement Unlocked: {undyingAchievement.HasAchieved}");
        Console.WriteLine($"Shutout Achievement Description: {shutoutAchievement.Description}");
        Console.WriteLine($"Map Achievements Unlocked: {mapAchievements.GetAchieved.Count}");
```

#### Visual Basic
```vb
        Dim owClient As New OverwatchClient
        Dim owPlayer As OverwatchPlayer = Await owClient.GetPlayerAsync("giggitybyte#11965", OverwatchPlatform.PC)

        ' Basic player information example.
        Console.WriteLine($"Level {owPlayer.PlayerLevel}")
        Console.WriteLine($"Support SR: {owPlayer.SkillRatings.GetRole(OverwatchRole.Support)}")
        Console.WriteLine($"Role With Highest SR: {owPlayer.SkillRatings.GetHighestRole.Key.ToString}")
        Console.WriteLine($"Private Profile: {owPlayer.IsProfilePrivate}")

        ' Player stats example.
        Dim compGamesWon As OverwatchStat = owPlayer.Stats(OverwatchGamemode.Competitive).GetStatExact("All Heroes", "Game", "Games Won")
        Dim qpTotalElims As OverwatchStat = owPlayer.Stats(OverwatchGamemode.Quickplay).GetStatExact("All Heroes", "Combat", "Eliminations")
        Dim brigDmgDone As OverwatchStat = owPlayer.Stats(OverwatchGamemode.Quickplay).GetStatExact("Brigitte", "Combat", "All Damage Done")

        Console.WriteLine($"Competitive Games Won: {compGamesWon.Value}")
        Console.WriteLine($"Quick Play Elims: {qpTotalElims.Value}")
        Console.WriteLine($"Brigitte Damage Dealt: {brigDmgDone.Value}")

        ' Player achievements example.
        Dim undyingAchievement As OverwatchAchievement = owPlayer.Achievements.GetByName("Undying")
        Dim shutoutAchievement As OverwatchAchievement = owPlayer.Achievements.GetByName("Shutout")
        Dim mapAchievements As List(Of OverwatchAchievement) = owPlayer.Achievements.FilterByCategory(OverwatchAchievementCategory.Maps)

        Console.WriteLine($"Undying Achievement Unlocked: {undyingAchievement.HasAchieved}")
        Console.WriteLine($"Shutout Achievement Description: {shutoutAchievement.Description}")
        Console.WriteLine($"Map Achievements Unlocked: {mapAchievements.GetAchieved.Count}")
```

## Contributing
See a problem with my code? Want to add in a new feature? Then please feel free to open an issue or pull request. 
I am always open to suggestions and critique. 

## Questions and Help
If you have any questions about the library or need help for some reason, go ahead and either open an issue or [join my Discord server](https://discord.gg/yh2txuK).
