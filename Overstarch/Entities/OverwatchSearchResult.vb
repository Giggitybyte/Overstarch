Imports Newtonsoft.Json
Imports Overstarch.Enums

Namespace Entities

    ''' <summary>
    ''' 
    ''' </summary>
    Friend Class OverwatchSearchResult

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        <JsonProperty("platform")>
        Public Property Platform As OverwatchPlatform

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        <JsonProperty("name")>
        Public Property Username As String

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        <JsonProperty("urlName")>
        Public Property UriFriendlyUsername As String

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        <JsonProperty("level")>
        Public Property Level As Integer

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        <JsonProperty("portrait")>
        Public Property PlayerIcon As String

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        <JsonProperty("isPublic")>
        Public Property IsProfilePublic As Boolean
    End Class
End Namespace