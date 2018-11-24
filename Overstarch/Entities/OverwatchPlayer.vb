Imports Overstarch.Enums

Namespace Entities

    ''' <summary>
    ''' A representation of an Overwatch player.
    ''' </summary>
    Public NotInheritable Class OverwatchPlayer

        ''' <summary>
        ''' The username for this player.
        ''' </summary>
        ''' <returns>A <see cref="String"/> containing a username.</returns>
        Public Property Username As String

        ''' <summary>
        ''' Internal Blizzard player ID.
        ''' </summary>
        ''' <returns></returns>
        Public Property Id As String

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Public Property Platform As OverwatchPlatform

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Public Property IsProfilePrivate As Boolean

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Public Property PlayerIconUrl As String

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Public Property PlayerLevel As UShort

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Public Property PlayerLevelImageUrl As String

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Public Property CompetitiveSkillRating As UShort

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Public Property CompetitiveRankImageUrl As String

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Public Property EndorsementLevel As UShort

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Public Property Endorsements As Dictionary(Of OverwatchEndorsement, Decimal)

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Public Property Stats As Dictionary(Of OverwatchGamemode, List(Of OverwatchPlayerStat))

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Public Property Achievements As List(Of OverwatchAchievement)
    End Class
End Namespace