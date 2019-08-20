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
            Get
                Return _achievements
            End Get
        End Property

        ''' <summary>
        ''' Profiles from other platforms associated with this player.<para/>
        ''' This will be empty unless it is populated through <see cref="OverwatchPlayer.GetAliasesAsync()"/>
        ''' </summary>
        ''' <returns>A list of <see cref="OverwatchApiPlayer"/></returns>
        Public ReadOnly Property Aliases As IReadOnlyList(Of OverwatchApiPlayer)

        ''' <summary>
        ''' Blizzard player ID.
        ''' </summary>
        ''' <returns>A <see cref="String"/></returns>
        Public ReadOnly Property BlizzardId As String
            Get
                Return _blizzardId
            End Get
        End Property

        ''' <summary>
        ''' The endorsement level for this player profile.
        ''' </summary>
        ''' <returns>A <see cref="UShort"/></returns>
        Public ReadOnly Property EndorsementLevel As UShort
            Get
                Return _endorsementLevel
            End Get
        End Property

        ''' <summary>
        ''' A dictionary of decimal percentages for endorsements.
        ''' </summary>
        ''' <returns>A dictionary with <see cref="OverwatchEndorsement"/> as the key and a <see cref="Decimal"/> as the value.</returns>
        Public ReadOnly Property Endorsements As IReadOnlyDictionary(Of OverwatchEndorsement, Decimal)
            Get
                Return _endorsements
            End Get
        End Property

        ''' <summary>
        ''' Whether or not this player profile is viewable by the public.
        ''' </summary>
        ''' <returns>A <see cref="Boolean"/></returns>
        Public ReadOnly Property IsProfilePrivate As Boolean
            Get
                Return _isPrivateProfile
            End Get
        End Property

        ''' <summary>
        ''' The platform for this Overwatch player profile.
        ''' </summary>
        ''' <returns>An <see cref="OverwatchPlatform"/></returns>
        Public ReadOnly Property Platform As OverwatchPlatform
            Get
                Return _platform
            End Get
        End Property

        ''' <summary>
        ''' The profile URL for this player.
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property ProfileUrl As String
            Get
                Return _profileUrl
            End Get
        End Property

        ''' <summary>
        ''' The player icon for this player profile.
        ''' </summary>
        ''' <returns>A <see cref="String"/></returns>
        Public ReadOnly Property PlayerIconUrl As String
            Get
                Return _iconUrl
            End Get
        End Property

        ''' <summary>
        ''' The level, including prestige, for this player profile.
        ''' </summary>
        ''' <returns>A <see cref="UShort"/></returns>
        Public ReadOnly Property PlayerLevel As UShort
            Get
                Return _level
            End Get
        End Property

        ''' <summary>
        ''' A dictionary of skill ratings for each role.<para/>
        ''' This list will only be populated with the roles that the player has placed in.
        ''' </summary>
        ''' <returns>A list of <see cref="OverwatchRank"/></returns>
        Public ReadOnly Property SkillRatings As IReadOnlyDictionary(Of OverwatchRole, UShort)
            Get
                Return _skillRatings
            End Get
        End Property


        ''' <summary>
        ''' A dictionary containing Quick Play and Competitive stats.
        ''' </summary>
        ''' <returns>A dictionary with <see cref="OverwatchGamemode"/> as the key and a list of <see cref="OverwatchStat"/> as the value.</returns>
        Public ReadOnly Property Stats As IReadOnlyDictionary(Of OverwatchGamemode, IReadOnlyList(Of OverwatchStat))
            Get
                Return _stats
            End Get
        End Property

        ''' <summary>
        ''' The username for this player profile.
        ''' </summary>
        ''' <returns>A <see cref="String"/> containing a username.</returns>
        Public ReadOnly Property Username As String
            Get
                Return _username
            End Get
        End Property

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

        ''' <summary>
        ''' Internal backing field for user facing Property.
        ''' </summary>
        Friend _achievements As IReadOnlyList(Of OverwatchAchievement)

        ''' <summary>
        ''' Internal backing field for user facing Property.
        ''' </summary>
        Friend _blizzardId As String

        ''' <summary>
        ''' Internal backing field for user facing Property.
        ''' </summary>
        Friend _endorsements As IReadOnlyDictionary(Of OverwatchEndorsement, Decimal)

        ''' <summary>
        ''' Internal backing field for user facing Property.
        ''' </summary>
        Friend _endorsementLevel As UShort

        ''' <summary>
        ''' Internal backing field for user facing Property.
        ''' </summary>
        Friend _isPrivateProfile As Boolean

        ''' <summary>
        ''' Internal backing field for user facing Property.
        ''' </summary>
        Friend _platform As OverwatchPlatform

        ''' <summary>
        ''' Internal backing field for user facing Property.
        ''' </summary>
        Friend _iconUrl As String

        ''' <summary>
        ''' Internal backing field for user facing Property.
        ''' </summary>
        Friend _level As UShort

        ''' <summary>
        ''' Internal backing field for user facing Property.
        ''' </summary>
        Friend _profileUrl As String

        ''' <summary>
        ''' Internal backing field for user facing Property.
        ''' </summary>
        Friend _stats As IReadOnlyDictionary(Of OverwatchGamemode, IReadOnlyList(Of OverwatchStat))

        ''' <summary>
        ''' Internal backing field for user facing Property.
        ''' </summary>
        Friend _skillRatings As IReadOnlyDictionary(Of OverwatchRole, UShort)

        ''' <summary>
        ''' Internal backing field for user facing Property.
        ''' </summary>
        Friend _username As String
    End Class
End Namespace