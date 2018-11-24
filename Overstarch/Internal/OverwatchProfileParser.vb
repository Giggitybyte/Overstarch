Imports System.Text.RegularExpressions
Imports AngleSharp
Imports AngleSharp.Dom
Imports AngleSharp.Dom.Html
Imports Newtonsoft.Json
Imports Overstarch.Entities
Imports Overstarch.Enums

Namespace Internal
    Friend Class OverwatchProfileParser
        Private ReadOnly _playerIdRegex As Regex = New Regex("\d+")
        Private ReadOnly _webpageParser As BrowsingContext = BrowsingContext.[New](Configuration.Default.WithDefaultLoader)

        Friend Async Function ParseAsync(username As String, platform As OverwatchPlatform) As Task(Of OverwatchPlayer)
            ' Scrape data from profile.
            Dim profileWebpage As IDocument = Await _webpageParser.OpenAsync($"{OverstarchUtilities.BaseUrl}/career/{platform.ToString.ToLower}/{username.Replace("#"c, "-"c)}")
            Dim player As New OverwatchPlayer

            If profileWebpage.QuerySelector("h1.u-align-center")?.FirstChild.TextContent = "Profile Not Found" Then
                Throw New ArgumentException("Provided username does Not exist On this platform.")
            Else
                With player
                    .Achievements = ParseAchievements(profileWebpage.QuerySelector("section[id='achievements-section']"))
                    .CompetitiveSkillRating = If(UShort.TryParse(profileWebpage.QuerySelector("div.competitive-rank div")?.TextContent, .CompetitiveSkillRating), .CompetitiveSkillRating, 0)
                    .CompetitiveRankImageUrl = If(TryCast(profileWebpage.QuerySelector("div.competitive-rank img"), IHtmlImageElement)?.Source, String.Empty)
                    .EndorsementLevel = If(UShort.TryParse(profileWebpage.QuerySelector("div.endorsement-level div.u-center")?.TextContent, .EndorsementLevel), .EndorsementLevel, 0)
                    .Endorsements = ParseEndorsements(profileWebpage.QuerySelector("div.endorsement-level"))
                    .Id = _playerIdRegex.Match(profileWebpage.QuerySelectorAll("script").Last.TextContent).Value
                    .IsProfilePrivate = profileWebpage.QuerySelector(".masthead-permission-level-text")?.TextContent = "Private Profile"
                    .Platform = platform
                    .PlayerLevelImageUrl = ParseLevelImageUrl(profileWebpage.QuerySelector("div.player-level"))

                    ' Fetch remaining data from non-public API.
                    Dim platformsUrl As String = $"{OverstarchUtilities.BaseUrl}/career/platforms/{ .Id}"
                    Dim accounts As List(Of OverwatchLookupResult) = JsonConvert.DeserializeObject(Of List(Of OverwatchLookupResult))(OverstarchUtilities.FetchJson(platformsUrl))
                    Dim currentAccount As OverwatchLookupResult = accounts.Where(Function(p) p.Platform = player.Platform).FirstOrDefault

                    .PlayerIconUrl = $"https://d1u1mce87gyfbn.cloudfront.net/game/unlocks/{currentAccount.PlayerIcon}.png"
                    .PlayerLevel = currentAccount.Level
                    .Username = currentAccount.Username
                End With
            End If

            Return Await Task.FromResult(player)
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="levelImageContent"></param>
        ''' <returns></returns>
        Private Function ParseLevelImageUrl(levelImageContent As IElement) As String
            Dim levelImageAttribute As String = levelImageContent.GetAttribute("style")
            Dim startIndex As Integer = levelImageAttribute.IndexOf("("c) + 1
            Return levelImageAttribute.Substring(startIndex, levelImageAttribute.IndexOf(")"c) - startIndex)
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
        ''' Internal method: parses HTML content related to achievements.
        ''' </summary>
        ''' <param name="achievementContent"></param>
        ''' <returns></returns>
        Private Function ParseAchievements(achievementContent As IElement) As List(Of OverwatchAchievement)
            Dim achievements As New List(Of OverwatchAchievement)

            For Each category As IElement In achievementContent.QuerySelectorAll("select > option")
                Dim categoryData As IElement = achievementContent.QuerySelector($"div[data-category-id='{category.GetAttribute("value")}']")

                For Each achievementData As IHtmlDivElement In categoryData.QuerySelectorAll("div.achievement-card")
                    Dim achievement As New OverwatchAchievement

                    With achievement
                        [Enum].TryParse(category.GetAttribute("option-id").ToUpper, .Category)
                        .Name = achievementData.QuerySelector("div.media-card-title").TextContent
                        .Description = categoryData.QuerySelector($"div[id='{achievementData.Dataset("tooltip")}']").QuerySelector("p[class='h6']").TextContent
                        .IconUrl = If(TryCast(achievementData.QuerySelector("img.media-card-fill"), IHtmlImageElement).Source, String.Empty)
                        .HasAchieved = Not achievementData.GetAttribute("class").Contains("m-disabled")
                    End With

                    achievements.Add(achievement)
                Next
            Next

            Return achievements
        End Function
    End Class
End Namespace