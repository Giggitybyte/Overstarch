Imports System.Text.RegularExpressions
Imports Newtonsoft.Json
Imports Overstarch.Entities
Imports Overstarch.Enums
Imports Overstarch.Internal

''' <summary>
''' Library entry point and main interface.
''' </summary>
Public NotInheritable Class OverwatchClient
    Private ReadOnly _profileParser As New OverwatchProfileParser
    Private ReadOnly _battletagRegex As Regex = New Regex("\w+#\d+")
    Private ReadOnly _psnIdRegex As Regex = New Regex("^[a-zA-Z]{1}[\w\d-]{2,12}$")
    Private ReadOnly _xblIdRegex As Regex = New Regex("^[a-zA-Z0-9\s]{1,15}$")

    ''' <summary>
    ''' Fetches data and stats for an Overwatch player.
    ''' <para>If a platform is not specified, a search across all platforms will be attempted; this will increase the time to retrieve data.</para>
    ''' </summary>
    ''' <param name="username">The username or battletag of the player to retrieve.</param>
    ''' <param name="platform">The platform for the provided username.</param>
    ''' <returns>An <see cref="OverwatchPlayer"/> object.</returns>
    Public Async Function GetPlayerAsync(username As String, Optional platform As OverwatchPlatform = 0) As Task(Of OverwatchPlayer)
        Dim player As OverwatchPlayer

        If platform = 0 Then
            player = Await PlatformLookupAsync(username)
        Else
            If platform = OverwatchPlatform.PC AndAlso Not _battletagRegex.IsMatch(username) Then
                Throw New ArgumentException("Provided battletag was not valid.")
            ElseIf platform = OverwatchPlatform.PSN AndAlso Not _psnIdRegex.IsMatch(username) Then
                Throw New ArgumentException("Provided PSN ID was not valid.")
            ElseIf platform = OverwatchPlatform.XBL AndAlso Not _xblIdRegex.IsMatch(username) Then
                Throw New ArgumentException("Provided gamertag was not valid.")
            End If

            player = Await _profileParser.ParseAsync(username, platform)
        End If

        Return player
    End Function

    ''' <summary>
    ''' Internal method: performs a lookup of Overwatch players from a username.
    ''' <para>Auto-detects battletags that are passed in and searches lookup results for an exact match and returns it. Non-battletag lookups will simply return the first player from the lookup results.</para>
    ''' </summary>
    ''' <param name="username">The username or battletag to lookup.</param>
    ''' <returns>An <see cref="OverwatchPlayer"/> object.</returns>
    Private Async Function PlatformLookupAsync(username As String) As Task(Of OverwatchPlayer)
        Dim lookupUrl As String = $"{OverstarchUtilities.BaseUrl}/search/account-by-name/{Uri.EscapeUriString(username)}"
        Dim lookupResults As List(Of OverwatchApiPlayer) = JsonConvert.DeserializeObject(Of List(Of OverwatchApiPlayer))(OverstarchUtilities.FetchJson(lookupUrl))

        If lookupResults.Count = 0 Then
            Throw New ArgumentException("There are no Overwatch players with that username.")
        Else
            If _battletagRegex.IsMatch(username) Then
                Dim matchedPlayer As OverwatchApiPlayer = lookupResults.Where(Function(r) r.Username.ToLower = username.ToLower).FirstOrDefault

                If matchedPlayer IsNot Nothing Then
                    Return Await GetPlayerAsync(matchedPlayer.Username, matchedPlayer.Platform)
                Else
                    Throw New ArgumentException("Provided battletag does not exist.")
                End If
            Else
                Dim result As OverwatchApiPlayer = lookupResults.First
                Return Await GetPlayerAsync(result.Username, result.Platform)
            End If
        End If
    End Function
End Class
