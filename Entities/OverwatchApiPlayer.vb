Imports Newtonsoft.Json
Imports Overstarch.Enums

Namespace Entities

    ''' <summary>
    ''' A represenation of an Overwatch player containing limited data pulled from one of the non-public Blizzard APIs.
    ''' </summary>
    Public NotInheritable Class OverwatchApiPlayer

        ''' <summary>
        ''' The platform for this player profile.
        ''' </summary>
        ''' <returns>An <see cref="OverwatchPlatform"/></returns>
        <JsonProperty("platform")>
        Public Property Platform As OverwatchPlatform

        ''' <summary>
        ''' The Blizzard player ID for this player.
        ''' </summary>
        ''' <returns>A <see cref="String"/></returns>
        <JsonProperty("id")>
        Public Property BlizzardId As String

        ''' <summary>
        ''' The username for this player profile.
        ''' </summary>
        ''' <returns>A <see cref="String"/></returns>
        <JsonProperty("name")>
        Public Property Username As String

        ''' <summary>
        ''' The level for this player profile.
        ''' </summary>
        ''' <returns>An <see cref="Integer"/></returns>
        <JsonProperty("playerLevel")>
        Friend Property Level As Integer

        ''' <summary>
        ''' The player icon for this player profile.
        ''' </summary>
        ''' <returns>A <see cref="String"/></returns>
        <JsonProperty("portrait")>
        Friend Property PlayerIcon As String

        ''' <summary>
        ''' Whether or not this player profile is viewable by the public.
        ''' </summary>
        ''' <returns>A <see cref="Boolean"/></returns>
        <JsonProperty("isPublic")>
        Friend Property IsProfilePublic As Boolean
    End Class
End Namespace