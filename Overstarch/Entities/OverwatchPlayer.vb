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
        Public ReadOnly Property Username As String

        ''' <summary>
        ''' The numeric identifier for this player's Battletag.
        ''' <para>This value will be 0 for console players.</para>
        ''' </summary>
        ''' <returns>A <see cref="UShort"/> containing </returns>
        Public ReadOnly Property BattleTagCode As UShort

        ''' <summary>
        ''' The player ID internally used by Blizzard.
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property BlizzardId As String

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property Platform As OverwatchPlatform

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property IsProfilePrivate As Boolean

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property PlayerIconUrl As String

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property PlayerLevel As UShort

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property PlayerLevelImageUrl As String

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property CompetitiveRank As UShort

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property CompetitiveRankImageUrl As String

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property EndorsementLevel As UShort

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property Endorsements As Dictionary(Of OverwatchEndorsement, Decimal)

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property Stats As Dictionary(Of OverwatchGamemode, List(Of OverwatchPlayerStat))

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property AssociatedAccounts As List(Of OverwatchPlayer)

    End Class
End Namespace