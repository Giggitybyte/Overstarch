Imports Newtonsoft.Json
Imports Overstarch.Enums

Namespace Entities

    ''' <summary>
    ''' A represenation of an Overwatch player containing limited data pulled from one of the non-public Blizzard APIs.
    ''' </summary>
    Public NotInheritable Class OverwatchApiPlayer

        ''' <summary>
        ''' The Blizzard player ID for this player.
        ''' </summary>
        ''' <returns>A <see cref="String"/></returns>
        Public ReadOnly Property BlizzardId As String
            Get
                Return _blizzardId
            End Get
        End Property

        ''' <summary>
        ''' The platform for this player profile.
        ''' </summary>
        ''' <returns>An <see cref="OverwatchPlatform"/></returns>
        Public ReadOnly Property Platform As OverwatchPlatform
            Get
                Return _platform
            End Get
        End Property

        ''' <summary>
        ''' The username for this player profile.
        ''' </summary>
        ''' <returns>A <see cref="String"/></returns>
        Public ReadOnly Property Username As String
            Get
                Return _username
            End Get
        End Property

        ''' <summary>
        ''' Internal backing field for JSON deserialization.
        ''' </summary>
        <JsonProperty("id")>
        Friend _blizzardId As String

        ''' <summary>
        ''' Internal backing field for JSON deserialization.
        ''' </summary>
        <JsonProperty("platform")>
        Friend _platform As OverwatchPlatform

        ''' <summary>
        ''' Internal backing field for JSON deserialization.
        ''' </summary>
        <JsonProperty("name")>
        Friend _username As String

    End Class
End Namespace