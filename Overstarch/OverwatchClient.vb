Imports System.Net
Imports System.Text.RegularExpressions
Imports AngleSharp
Imports AngleSharp.Dom
Imports Newtonsoft.Json
Imports Overstarch.Entities
Imports Overstarch.Enums

''' <summary>
''' 
''' </summary>
Public NotInheritable Class OverwatchClient
    Private Const BaseUrl As String = "https://playoverwatch.com/en-us/"
    Private ReadOnly _webpageRetriever As BrowsingContext = BrowsingContext.[New](Configuration.Default.WithDefaultLoader)
    Private ReadOnly _battletagRegex As Regex = New Regex("\w+#\d+")
    Private ReadOnly _psnIdRegex As Regex = New Regex("^[a-zA-Z]{1}[\w\d-]{2,12}$")
    Private ReadOnly _xblIdRegex As Regex = New Regex("^[a-zA-Z0-9\s]{1,15}$")

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="username"></param>
    ''' <param name="platform"></param>
    ''' <returns></returns>
    Public Async Function GetPlayerAsync(username As String, Optional platform As OverwatchPlatform = 0) As Task(Of OverwatchPlayer)
        Dim player As OverwatchPlayer

        If platform = 0 Then
            player = Await PlatformLookupAsync(username)
        Else
            If platform = OverwatchPlatform.PC AndAlso Not _battletagRegex.IsMatch(username) Then Throw New ArgumentException("Provided battletag was not valid.")
            If platform = OverwatchPlatform.PSN AndAlso Not _psnIdRegex.IsMatch(username) Then Throw New ArgumentException("Provided PSN ID was not valid.")
            If platform = OverwatchPlatform.XBL AndAlso Not _xblIdRegex.IsMatch(username) Then Throw New ArgumentException("Provided gamertag was not valid.")

            Dim profile As IDocument = Await _webpageRetriever.OpenAsync($"{BaseUrl}/career/{platform.ToString.ToLower}/{username}")
            player = New OverwatchPlayer

            If profile.TextContent.Contains("Profile Not Found") Then
                Throw New ArgumentException("Provided username does not exist.")
            Else
                With player
                    ' Parsing stuff here.
                End With
            End If

        End If

        Return player
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="username"></param>
    ''' <returns></returns>
    Private Async Function PlatformLookupAsync(username As String) As Task(Of OverwatchPlayer)
        Dim lookupUrl As String = $"{BaseUrl}/search/account-by-name/{Uri.EscapeUriString(username)}"
        Dim lookupResults As New List(Of OverwatchSearchResult)

        Using webpage As New WebClient
            webpage.Headers(HttpRequestHeader.UserAgent) = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/70.0.3538.102 Safari/537.36"
            Dim response As String = webpage.DownloadString(lookupUrl)

            lookupResults = JsonConvert.DeserializeObject(Of List(Of OverwatchSearchResult))(response)
        End Using

        If lookupResults.Count = 0 Then
            Throw New ArgumentException("There are no Overwatch players with that username.")
        Else
            If _battletagRegex.IsMatch(username) Then
                Dim matchedPlayer As OverwatchSearchResult = lookupResults.Where(Function(r) r.Username.ToLower = username.ToLower).FirstOrDefault

                If matchedPlayer IsNot Nothing Then
                    Return Await GetPlayerAsync(matchedPlayer.Username, matchedPlayer.Platform)
                Else
                    Throw New ArgumentException("Provided battletag does not exist.")
                End If
            Else
                Dim result As OverwatchSearchResult = lookupResults.First
                Return Await GetPlayerAsync(result.Username, result.Platform)
            End If
        End If
    End Function
End Class
