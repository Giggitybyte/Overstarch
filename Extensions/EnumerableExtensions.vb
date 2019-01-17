Imports System.Runtime.CompilerServices
Imports Overstarch.Entities
Imports Overstarch.Enums

Namespace Extensions
    Public Module EnumerableExtensions

        ''' <summary>
        ''' Get stats for a specific hero.
        ''' </summary>
        ''' <returns>A collection of <see cref="OverwatchStat"/> with stats for a specific hero.</returns>
        <Extension()>
        Public Function FilterByHero(stats As IEnumerable(Of OverwatchStat), heroName As String) As IEnumerable(Of OverwatchStat)
            Return stats.Where(Function(s) s.Hero.ToLower = heroName.RemoveWhitespace.ToLower).ToList
        End Function

        ''' <summary>
        ''' Get stats by category.
        ''' </summary>
        ''' <returns>A collection of <see cref="OverwatchStat"/> for stats in a specific catagory.</returns>
        <Extension()>
        Public Function FilterByCategory(stats As IEnumerable(Of OverwatchStat), categoryName As String) As IEnumerable(Of OverwatchStat)
            Return stats.Where(Function(s) s.Category.ToLower = categoryName.ToLower).ToList
        End Function

        ''' <summary>
        ''' Get stats by name.
        ''' </summary>
        ''' <returns>A collection of <see cref="OverwatchStat"/> with stats that match a specific name.</returns>
        <Extension()>
        Public Function FilterByName(stats As IEnumerable(Of OverwatchStat), statName As String) As IEnumerable(Of OverwatchStat)
            Return stats.Where(Function(s) s.Name.ToLower = statName.ToLower).ToList
        End Function

        ''' <summary>
        ''' Get an exact stat for a specific hero.
        ''' </summary>
        ''' <returns>The specified <see cref="OverwatchStat"/> for the specified hero.</returns>
        <Extension()>
        Public Function GetStatExact(stats As IEnumerable(Of OverwatchStat), heroName As String, categoryName As String, statName As String) As OverwatchStat
            Return stats.FilterByHero(heroName).FilterByCategory(categoryName).FilterByName(statName).FirstOrDefault
        End Function

        ''' <summary>
        ''' Get achievements by category.
        ''' </summary>
        ''' <returns>A collection of achievements under a specific <see cref="OverwatchAchievementCategory"/></returns>
        <Extension()>
        Public Function FilterByCategory(achievements As IEnumerable(Of OverwatchAchievement), category As OverwatchAchievementCategory) As IEnumerable(Of OverwatchAchievement)
            Return achievements.Where(Function(a) a.Category.Equals(category)).ToList
        End Function

        ''' <summary>
        ''' Get an achievement by its name.
        ''' </summary>
        ''' <returns>A specific <see cref="OverwatchAchievement"/>.</returns>
        <Extension()>
        Public Function FilterByName(achievements As IEnumerable(Of OverwatchAchievement), achievementName As String) As OverwatchAchievement
            Return achievements.FirstOrDefault(Function(a) a.Name.ToLower = achievementName.ToLower)
        End Function
    End Module
End Namespace