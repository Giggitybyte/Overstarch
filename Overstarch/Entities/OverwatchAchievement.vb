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
            Get
                Return _category
            End Get
        End Property

        ''' <summary>
        ''' The description for this achievement.
        ''' </summary>
        ''' <returns>A <see cref="String"/></returns>
        Public ReadOnly Property Description As String
            Get
                Return _description
            End Get
        End Property

        ''' <summary>
        ''' Whether or not this achievement has been achieved yet.
        ''' </summary>
        ''' <returns>A <see cref="Boolean"/></returns>
        Public ReadOnly Property HasAchieved As Boolean
            Get
                Return _hasAchieved
            End Get
        End Property

        ''' <summary>
        ''' The URL of the icon for this achievement.
        ''' </summary>
        ''' <returns>A <see cref="String"/></returns>
        Public ReadOnly Property IconUrl As String
            Get
                Return _iconUrl
            End Get
        End Property

        ''' <summary>
        ''' The name of this achievement.
        ''' </summary>
        ''' <returns>A <see cref="String"/></returns>
        Public ReadOnly Property Name As String
            Get
                Return _name
            End Get
        End Property

        ''' <summary>
        ''' Internal backing field for user facing Property.
        ''' </summary>
        Friend _category As OverwatchAchievementCategory

        ''' <summary>
        ''' Internal backing field for user facing Property.
        ''' </summary>
        Friend _description As String

        ''' <summary>
        ''' Internal backing field for user facing Property.
        ''' </summary>
        Friend _hasAchieved As Boolean

        ''' <summary>
        ''' Internal backing field for user facing Property.
        ''' </summary>
        Friend _iconUrl As String

        ''' <summary>
        ''' Internal backing field for user facing Property.
        ''' </summary>
        Friend _name As String
    End Class
End Namespace