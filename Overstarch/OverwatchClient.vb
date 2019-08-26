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
    Private Shared ReadOnly _battletagRegex As New Regex("\w+#\d+")
    Private Shared ReadOnly _psnIdRegex As New Regex("[a-zA-Z]{1}[\w\d-]{2,12}")
    Private Shared ReadOnly _xblIdRegex As New Regex("[a-zA-Z0-9\s]{1,15}")

    ''' <summary>
    ''' Fetches data and stats for an Overwatch player.<para/>
    ''' If a platform is not specified, a search across all platforms will be attempted; this will increase the time to retrieve data.
    ''' </summary>
    ''' <param name="username">The username or battletag of the player to retrieve.</param>
    ''' <param name="platform">The platform for the provided username.</param>
    ''' <returns>An <see cref="OverwatchPlayer"/> object.</returns>
    Public Async Function GetPlayerAsync(username As String, Optional platform As OverwatchPlatform = 0) As Task(Of OverwatchPlayer)
        Select Case platform
            Case 0
                Return Await PlatformLookupAsync(username).ConfigureAwait(False)
            Case OverwatchPlatform.PC
                If Not _battletagRegex.IsMatch(username) Then Throw New ArgumentException("Provided battletag was not valid.")
            Case OverwatchPlatform.PSN
                If Not _psnIdRegex.IsMatch(username) Then Throw New ArgumentException("Provided PSN ID was not valid.")
            Case OverwatchPlatform.XBL
                If Not _xblIdRegex.IsMatch(username) Then Throw New ArgumentException("Provided gamertag was not valid.")
            Case Else
                Throw New ArgumentException("Provided platform was not valid.")
        End Select

        Return Await _profileParser.ParseAsync(username, platform).ConfigureAwait(False)
    End Function


    ''' <summary>
    ''' Internal method: performs a lookup of Overwatch players from a username.<para/>
    ''' Auto-detects battletags that are passed in and searches lookup results for an exact match and returns it. Non-battletag lookups will simply return the first player from the lookup results.
    ''' </summary>
    ''' <param name="username">The username or battletag to lookup.</param>
    ''' <returns>An <see cref="OverwatchPlayer"/> object.</returns>
    Private Async Function PlatformLookupAsync(username As String) As Task(Of OverwatchPlayer)
        Dim lookupUrl = $"{OverstarchUtilities.BaseUrl}/search/account-by-name/{Uri.EscapeUriString(username)}"
        Dim json = Await OverstarchUtilities.FetchWebPageAsync(lookupUrl).ConfigureAwait(False)
        Dim lookupResults = JsonConvert.DeserializeObject(Of List(Of OverwatchApiPlayer))(json)

        If Not lookupResults.Any Then Throw New ArgumentException("There are no Overwatch players with that username.")

        If _battletagRegex.IsMatch(username) Then
            Dim matchedPlayer As OverwatchApiPlayer = lookupResults.Where(Function(r) r.Username.ToLower = username.ToLower).FirstOrDefault
            If matchedPlayer Is Nothing Then Throw New ArgumentException("Provided battletag does not exist.")
            Return Await GetPlayerAsync(matchedPlayer.Username, matchedPlayer.Platform).ConfigureAwait(False)
        End If

        Dim result As OverwatchApiPlayer = lookupResults.First
        Return Await GetPlayerAsync(result.Username, result.Platform).ConfigureAwait(False)
    End Function
End Class
