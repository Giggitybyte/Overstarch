Imports System.Net
Imports System.Text.RegularExpressions
Imports AngleSharp
Imports AngleSharp.Dom
Imports AngleSharp.Dom.Html
Imports Newtonsoft.Json
Imports Overstarch.Entities
Imports Overstarch.Enums

''' <summary>
''' Library entry point and main interface.
''' <para>All Overwatch player data is fetched and parsed within this class.</para>
''' </summary>
Public NotInheritable Class OverwatchClient
    Private Const BaseUrl As String = "https://playoverwatch.com/en-us"
    Private ReadOnly _webpageParser As BrowsingContext = BrowsingContext.[New](Configuration.Default.WithDefaultLoader)
    Private ReadOnly _playerIdRegex As Regex = New Regex("\d+")
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

            ' Scrape data from profile.
            Dim profileWebpage As IDocument = Await _webpageParser.OpenAsync($"{BaseUrl}/career/{platform.ToString.ToLower}/{username.Replace("#"c, "-"c)}")
            player = New OverwatchPlayer

            If profileWebpage.QuerySelector("h1.u-align-center")?.FirstChild.TextContent = "Profile Not Found" Then
                Throw New ArgumentException("Provided username does not exist on this platform.")
            Else
                With player
                    .CompetitiveSkillRating = If(UShort.TryParse(profileWebpage.QuerySelector("div.competitive-rank div")?.TextContent, .CompetitiveSkillRating), .CompetitiveSkillRating, 0)
                    .CompetitiveRankImageUrl = If(TryCast(profileWebpage.QuerySelector("div.competitive-rank img"), IHtmlImageElement)?.Source, String.Empty)
                    .EndorsementLevel = If(UShort.TryParse(profileWebpage.QuerySelector("div.endorsement-level div.u-center")?.TextContent, .EndorsementLevel), .EndorsementLevel, 0)
                    .Endorsements = ParseEndorsements(profileWebpage.QuerySelector("div.endorsement-level"))
                    .Id = _playerIdRegex.Match(profileWebpage.QuerySelectorAll("script").Last.TextContent).Value
                    .IsProfilePrivate = profileWebpage.QuerySelector(".masthead-permission-level-text")?.TextContent = "Private Profile"
                    .Platform = platform

                    Dim levelImageAttribute As String = profileWebpage.QuerySelector("div.player-level").GetAttribute("style")
                    Dim startIndex As Integer = levelImageAttribute.IndexOf("("c) + 1
                    .PlayerLevelImageUrl = levelImageAttribute.Substring(startIndex, levelImageAttribute.IndexOf(")"c) - startIndex)

                    ' Fetch remaining data from non-public API.
                    Dim platformsUrl As String = $"{BaseUrl}/career/platforms/{ .Id}"
                    Dim accounts As List(Of OverwatchAccountResult) = JsonConvert.DeserializeObject(Of List(Of OverwatchAccountResult))(FetchJson(platformsUrl))
                    Dim currentAccount As OverwatchAccountResult = accounts.Where(Function(p) p.Platform = player.Platform).FirstOrDefault

                    .PlayerIconUrl = $"https://d1u1mce87gyfbn.cloudfront.net/game/unlocks/{currentAccount.PlayerIcon}.png"
                    .PlayerLevel = currentAccount.Level
                    .Username = currentAccount.Username
                End With
            End If
        End If

        Return player
    End Function

    ''' <summary>
    ''' Fetches any associated accounts on other platforms for a given player.
    ''' <para>All data and stats will be retrieved for each associated account. As a result, it may take a moment for this method to return.</para>
    ''' </summary>
    ''' <param name="player">An <see cref="OverwatchPlayer"/> retrieved from <see cref="GetPlayerAsync(String, OverwatchPlatform)"/>.</param>
    ''' <returns>A list of <see cref="OverwatchPlayer"/> objects.</returns>
    Public Function GetAssociatedAccounts(player As OverwatchPlayer) As List(Of OverwatchPlayer)
        Throw New NotImplementedException
    End Function

    ''' <summary>
    ''' Internal method: performs a lookup of Overwatch players from a username.
    ''' <para>Auto-detects battletags that are passed in and searches lookup results for an exact match and returns it. Non-battletag lookups will simply return the first player from the lookup results.</para>
    ''' </summary>
    ''' <param name="username">The username or battletag to lookup.</param>
    ''' <returns>An <see cref="OverwatchPlayer"/> object.</returns>
    Private Async Function PlatformLookupAsync(username As String) As Task(Of OverwatchPlayer)
        Dim lookupUrl As String = $"{BaseUrl}/search/account-by-name/{Uri.EscapeUriString(username)}"
        Dim lookupResults As List(Of OverwatchAccountResult) = JsonConvert.DeserializeObject(Of List(Of OverwatchAccountResult))(FetchJson(lookupUrl))

        If lookupResults.Count = 0 Then
            Throw New ArgumentException("There are no Overwatch players with that username.")
        Else
            If _battletagRegex.IsMatch(username) Then
                Dim matchedPlayer As OverwatchAccountResult = lookupResults.Where(Function(r) r.Username.ToLower = username.ToLower).FirstOrDefault

                If matchedPlayer IsNot Nothing Then
                    Return Await GetPlayerAsync(matchedPlayer.Username, matchedPlayer.Platform)
                Else
                    Throw New ArgumentException("Provided battletag does not exist.")
                End If
            Else
                Dim result As OverwatchAccountResult = lookupResults.First
                Return Await GetPlayerAsync(result.Username, result.Platform)
            End If
        End If
    End Function

    ''' <summary>
    ''' Internal method: parses HTML content related to endorsement percentages.
    ''' </summary>
    ''' <param name="endorsementContent">Endorsement HTML content.</param>
    ''' <returns>A dictionary of <see cref="OverwatchEndorsement"/> and <see cref="Decimal"/>.</returns>
    Private Function ParseEndorsements(endorsementContent As IElement) As Dictionary(Of OverwatchEndorsement, Decimal)
        Dim endorsements As New Dictionary(Of OverwatchEndorsement, Decimal)

        If endorsementContent IsNot Nothing Then
            For Each endorsement As IElement In endorsementContent.QuerySelectorAll("svg")
                Dim percentage As String = endorsement.GetAttribute("data-value")

                If percentage IsNot Nothing Then
                    Dim endorsementName As String = endorsement.GetAttribute("class").Substring(endorsement.GetAttribute("class").IndexOf("--") + 2)
                    Dim endorsementEnum As OverwatchEndorsement

                    Select Case endorsementName
                        Case "teammate"
                            endorsementEnum = OverwatchEndorsement.GOODTEAMMATE
                        Case "shotcaller"
                            endorsementEnum = OverwatchEndorsement.SHOTCALLER
                        Case "sportsmanship"
                            endorsementEnum = OverwatchEndorsement.SPORTSMANSHIP
                    End Select

                    endorsements.Add(endorsementEnum, Decimal.Parse(percentage))
                End If
            Next
        End If

        Return endorsements
    End Function

    ''' <summary>
    ''' Internal method: simply retrieves the source of a webpage. Intended to download JSON for deserialization.
    ''' </summary>
    ''' <param name="url">JSON URL.</param>
    ''' <returns>A <see cref="String"/> containing JSON.</returns>
    Private Function FetchJson(url As String) As String
        Using webpage As New WebClient
            webpage.Headers(HttpRequestHeader.UserAgent) = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/70.0.3538.102 Safari/537.36"
            Return webpage.DownloadString(url)
        End Using
    End Function
End Class
