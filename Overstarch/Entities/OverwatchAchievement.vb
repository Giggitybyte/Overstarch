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
        Public ReadOnly Property Category As OverwatchAchievementCategory

        ''' <summary>
        ''' The description for this achievement.
        ''' </summary>
        ''' <returns>A <see cref="String"/></returns>
        Public ReadOnly Property Description As String

        ''' <summary>
        ''' Whether or not this achievement has been achieved yet.
        ''' </summary>
        ''' <returns>A <see cref="Boolean"/></returns>
        Public ReadOnly Property HasAchieved As Boolean

        ''' <summary>
        ''' The URL of the icon for this achievement.
        ''' </summary>
        ''' <returns>A <see cref="String"/></returns>
        Public ReadOnly Property IconUrl As String

        ''' <summary>
        ''' The name of this achievement.
        ''' </summary>
        ''' <returns>A <see cref="String"/></returns>
        Public ReadOnly Property Name As String


        Friend Sub New(category As OverwatchAchievementCategory, description As String, hasAchieved As Boolean, iconUrl As String, name As String)
            _Category = category
            _Description = description
            _HasAchieved = hasAchieved
            _IconUrl = iconUrl
            _Name = name
        End Sub
    End Class
End Namespace