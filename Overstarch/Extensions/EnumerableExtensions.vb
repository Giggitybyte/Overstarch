Imports System.Runtime.CompilerServices
Imports Overstarch.Entities
Imports Overstarch.Enums

Namespace Extensions
    Public Module EnumerableExtensions

        ''' <summary>
        ''' Get stats for a specific hero.
        ''' </summary>
        ''' <returns>A collection of filtered stats.</returns>
        <Extension()>
        Public Function FilterByHero(stats As IEnumerable(Of OverwatchStat), heroName As String) As IEnumerable(Of OverwatchStat)
            Return stats.Where(Function(s) s.Hero.ToLower = heroName.RemoveWhitespace.ToLower)
        End Function

        ''' <summary>
        ''' Get stats by category.
        ''' </summary>
        ''' <returns>A collection of filtered stats.</returns>
        <Extension()>
        Public Function FilterByCategory(stats As IEnumerable(Of OverwatchStat), categoryName As String) As IEnumerable(Of OverwatchStat)
            Return stats.Where(Function(s) s.Category = categoryName)
        End Function

        ''' <summary>
        ''' Get a stat by its name.
        ''' </summary>
        ''' <returns>A collection of filtered stats.</returns>
        <Extension()>
        Public Function FilterByName(stats As IEnumerable(Of OverwatchStat), statName As String) As OverwatchStat
            Return stats.Where(Function(s) s.Name = statName).FirstOrDefault
        End Function

        ''' <summary>
        ''' Get an exact stat for a specific hero.
        ''' </summary>
        ''' <returns>A collection of filtered stats.</returns>
        <Extension()>
        Public Function GetStatExact(stats As IEnumerable(Of OverwatchStat), heroName As String, categoryName As String, statName As String) As OverwatchStat
            Return stats.FilterByHero(heroName).FilterByCategory(categoryName).FilterByName(statName)
        End Function

        ''' <summary>
        ''' Get achievements by category.
        ''' </summary>
        ''' <returns>A collection of filtered achievements.</returns>
        <Extension()>
        Public Function FilterByCategory(achievements As IEnumerable(Of OverwatchAchievement), category As OverwatchAchievementCategory) As IEnumerable(Of OverwatchAchievement)
            Return achievements.Where(Function(a) a.Category = category)
        End Function

        ''' <summary>
        ''' Get an achievement by its name.
        ''' </summary>
        ''' <returns>A collection of filtered achievements.</returns>
        <Extension()>
        Public Function FilterByName(achievements As IEnumerable(Of OverwatchAchievement), achievementName As String) As OverwatchAchievement
            Return achievements.FirstOrDefault(Function(a) a.Name.ToLower = achievementName.ToLower)
        End Function
    End Module
End Namespace