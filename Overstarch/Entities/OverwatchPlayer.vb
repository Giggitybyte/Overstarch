Imports Overstarch.Enums

Namespace Entities

    ''' <summary>
    ''' A representation of an Overwatch player.
    ''' </summary>
    Public NotInheritable Class OverwatchPlayer

        ''' <summary>
        ''' The username for this player profile.
        ''' </summary>
        ''' <returns>A <see cref="String"/> containing a username.</returns>
        Public Property Username As String

        ''' <summary>
        ''' Internal Blizzard player ID.
        ''' </summary>
        ''' <returns>A <see cref="String"/></returns>
        Public Property BlizzardId As String

        ''' <summary>
        ''' Profiles from other platforms associated with this player.
        ''' </summary>
        ''' <returns>A list of <see cref="OverwatchApiPlayer"/></returns>
        Public Property Aliases As List(Of OverwatchApiPlayer)

        ''' <summary>
        ''' The platform for this Overwatch player profile.
        ''' </summary>
        ''' <returns>An <see cref="OverwatchPlatform"/></returns>
        Public Property Platform As OverwatchPlatform

        ''' <summary>
        ''' Whether or not this player profile is viewable by the public.
        ''' </summary>
        ''' <returns>A <see cref="Boolean"/></returns>
        Public Property IsProfilePrivate As Boolean

        ''' <summary>
        ''' The icon for this player profile.
        ''' </summary>
        ''' <returns>A <see cref="String"/></returns>
        Public Property PlayerIconUrl As String

        ''' <summary>
        ''' The level for this player profile including prestige.
        ''' </summary>
        ''' <returns>A <see cref="UShort"/></returns>
        Public Property PlayerLevel As UShort

        ''' <summary>
        ''' The URL of the portrait border image.
        ''' </summary>
        ''' <returns>A <see cref="String"/></returns>
        Public Property PlayerPortraitBorderImageUrl As String

        ''' <summary>
        ''' The skill rating for this player profile.
        ''' </summary>
        ''' <returns>A <see cref="UShort"/></returns>
        Public Property CompetitiveSkillRating As UShort

        ''' <summary>
        ''' The URL of the rank icon for the associated skill rating.
        ''' </summary>
        ''' <returns>A <see cref="String"/></returns>
        Public Property CompetitiveRankImageUrl As String

        ''' <summary>
        ''' The endorsement level for this player.
        ''' </summary>
        ''' <returns>A <see cref="UShort"/></returns>
        Public Property EndorsementLevel As UShort

        ''' <summary>
        ''' A dictionary of endorsement percentages.
        ''' </summary>
        ''' <returns>A dictionary with <see cref="OverwatchEndorsement"/> as the key and a <see cref="Decimal"/> as the value.</returns>
        Public Property Endorsements As Dictionary(Of OverwatchEndorsement, Decimal)

        ''' <summary>
        ''' A list of all achievements.
        ''' </summary>
        ''' <returns>A list of <see cref="OverwatchAchievement"/></returns>
        Public Property Achievements As List(Of OverwatchAchievement)

        ''' <summary>
        ''' A dictionary containing Quick Play and Competitive stats.
        ''' </summary>
        ''' <returns>A dictionary with <see cref="OverwatchGamemode"/> as the key and a list of <see cref="OverwatchStat"/> as the value.</returns>
        Public Property Stats As Dictionary(Of OverwatchGamemode, List(Of OverwatchStat))
    End Class
End Namespace