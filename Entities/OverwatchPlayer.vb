Imports Newtonsoft.Json
Imports Overstarch.Enums
Imports Overstarch.Internal

Namespace Entities

    ''' <summary>
    ''' A representation of an Overwatch player.
    ''' </summary>
    Public NotInheritable Class OverwatchPlayer

        ''' <summary>
        ''' A list of all achievements.
        ''' </summary>
        ''' <returns>A list of <see cref="OverwatchAchievement"/></returns>
        Public ReadOnly Property Achievements As IReadOnlyList(Of OverwatchAchievement)

        ''' <summary>
        ''' Profiles from other platforms associated with this player.<para/>
        ''' This will be empty unless it is populated through <see cref="OverwatchPlayer.GetAliasesAsync()"/>
        ''' </summary>
        ''' <returns>A list of <see cref="OverwatchApiPlayer"/></returns>
        Public ReadOnly Property Aliases As IReadOnlyList(Of OverwatchApiPlayer)

        ''' <summary>
        ''' Internal Blizzard player ID.
        ''' </summary>
        ''' <returns>A <see cref="String"/></returns>
        Public ReadOnly Property BlizzardId As String

        ''' <summary>
        ''' The URL of the rank icon for the associated skill rating.
        ''' </summary>
        ''' <returns>A <see cref="String"/></returns>
        Public ReadOnly Property CompetitiveRankImageUrl As String

        ''' <summary>
        ''' The skill rating for this player profile.
        ''' </summary>
        ''' <returns>A <see cref="UShort"/></returns>
        Public ReadOnly Property CompetitiveSkillRating As UShort

        ''' <summary>
        ''' The endorsement level for this player profile.
        ''' </summary>
        ''' <returns>A <see cref="UShort"/></returns>
        Public ReadOnly Property EndorsementLevel As UShort

        ''' <summary>
        ''' A dictionary of decimal percentages for endorsements.
        ''' </summary>
        ''' <returns>A dictionary with <see cref="OverwatchEndorsement"/> as the key and a <see cref="Decimal"/> as the value.</returns>
        Public ReadOnly Property Endorsements As IReadOnlyDictionary(Of OverwatchEndorsement, Decimal)

        ''' <summary>
        ''' Whether or not this player profile is viewable by the public.
        ''' </summary>
        ''' <returns>A <see cref="Boolean"/></returns>
        Public ReadOnly Property IsProfilePrivate As Boolean

        ''' <summary>
        ''' The platform for this Overwatch player profile.
        ''' </summary>
        ''' <returns>An <see cref="OverwatchPlatform"/></returns>
        Public ReadOnly Property Platform As OverwatchPlatform

        ''' <summary>
        ''' The profile URL for this player.
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property ProfileUrl As String

        ''' <summary>
        ''' The player icon for this player profile.
        ''' </summary>
        ''' <returns>A <see cref="String"/></returns>
        Public ReadOnly Property PlayerIconUrl As String

        ''' <summary>
        ''' The level, including prestige, for this player profile.
        ''' </summary>
        ''' <returns>A <see cref="UShort"/></returns>
        Public ReadOnly Property PlayerLevel As UShort

        ''' <summary>
        ''' A dictionary containing Quick Play and Competitive stats.
        ''' </summary>
        ''' <returns>A dictionary with <see cref="OverwatchGamemode"/> as the key and a list of <see cref="OverwatchStat"/> as the value.</returns>
        Public ReadOnly Property Stats As IReadOnlyDictionary(Of OverwatchGamemode, IReadOnlyList(Of OverwatchStat))

        ''' <summary>
        ''' The username for this player profile.
        ''' </summary>
        ''' <returns>A <see cref="String"/> containing a username.</returns>
        Public ReadOnly Property Username As String

        ' Don't worry, Quahu said it's okay.
        Friend Sub New(achievements As IReadOnlyList(Of OverwatchAchievement), blizzardId As String, compRankUrl As String,
                       compSkillRating As UShort, endorsementLevel As UShort, endorsements As IReadOnlyDictionary(Of OverwatchEndorsement, Decimal),
                       isProfilePrivate As Boolean, platform As OverwatchPlatform, playerIconUrl As String, playerLevel As UShort,
                       profileUrl As String, stats As IReadOnlyDictionary(Of OverwatchGamemode, IReadOnlyList(Of OverwatchStat)), username As String)

            _Achievements = achievements
            _BlizzardId = blizzardId
            _CompetitiveRankImageUrl = compRankUrl
            _CompetitiveSkillRating = compSkillRating
            _EndorsementLevel = endorsementLevel
            _Endorsements = endorsements
            _IsProfilePrivate = isProfilePrivate
            _Platform = platform
            _PlayerIconUrl = playerIconUrl
            _PlayerLevel = playerLevel
            _ProfileUrl = profileUrl
            _Stats = stats
            _Username = username
        End Sub

        ''' <summary>
        ''' Populates the Aliases property of this <see cref="OverwatchPlayer"/> object with <see cref="OverwatchApiPlayer"/> objects.<para/>
        ''' Makes use of the non-public Blizzard APIs.
        ''' </summary>
        ''' <returns>A </returns>
        Public Async Function GetAliasesAsync() As Task
            Dim platformsUrl As String = $"{OverstarchUtilities.BaseUrl}/career/platforms/{BlizzardId}"
            Dim json As String = Await OverstarchUtilities.FetchJsonAsync(platformsUrl).ConfigureAwait(False)
            Dim accounts As List(Of OverwatchApiPlayer) = JsonConvert.DeserializeObject(Of List(Of OverwatchApiPlayer))(json)

            accounts.RemoveAll(Function(a) a.Platform = Platform)
            _Aliases = accounts
        End Function
    End Class
End Namespace