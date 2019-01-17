Imports Overstarch.Enums

Namespace Entities

    ''' <summary>
    ''' A represenation of an Overwatch achievement.
    ''' </summary>
    Public NotInheritable Class OverwatchAchievement

        ''' <summary>
        ''' The category this achievement is under.
        ''' </summary>
        ''' <returns>An <see cref="OverwatchAchievementCategory"/></returns>
        Public Property Category As OverwatchAchievementCategory

        ''' <summary>
        ''' The name of this achievement.
        ''' </summary>
        ''' <returns>A <see cref="String"/></returns>
        Public Property Name As String

        ''' <summary>
        ''' The description for this achievement.
        ''' </summary>
        ''' <returns>A <see cref="String"/></returns>
        Public Property Description As String

        ''' <summary>
        ''' The URL of the icon for this achievement.
        ''' </summary>
        ''' <returns>A <see cref="String"/></returns>
        Public Property IconUrl As String

        ''' <summary>
        ''' Whether or not this achievement has been achieved yet.
        ''' </summary>
        ''' <returns>A <see cref="Boolean"/></returns>
        Public Property HasAchieved As Boolean
    End Class
End Namespace