﻿Imports System.IO
Imports System.Text.RegularExpressions
Imports AngleSharp
Imports AngleSharp.Dom
Imports AngleSharp.Dom.Css
Imports AngleSharp.Dom.Html
Imports Overstarch.Entities
Imports Overstarch.Enums
Imports Overstarch.Extensions

Namespace Internal

    ''' <summary>
    ''' Internal class for parsing an Overwatch profile.
    ''' </summary>
    Friend NotInheritable Class OverwatchProfileParser
        Private ReadOnly _playerIdRegex As Regex = New Regex("\d+")
        Private Shared ReadOnly _webpageParser As BrowsingContext = BrowsingContext.[New](Configuration.Default.WithDefaultLoader.WithCss)

        Friend Async Function ParseAsync(username As String, platform As OverwatchPlatform) As Task(Of OverwatchPlayer)
            ' Scrape data from profile.

            Dim profileUrl As String = $"{OverstarchUtilities.BaseUrl}/career/{platform.ToString.ToLower}/{username.Replace("#"c, "-"c)}"
            Dim profileWebpage As IDocument = Await _webpageParser.OpenAsync(profileUrl)
            Dim player As New OverwatchPlayer

            If profileWebpage.QuerySelector("h1.u-align-center")?.FirstChild.TextContent = "Profile Not Found" Then
                Throw New ArgumentException("Provided username does not exist On this platform.")
            Else
                With player
                    .Achievements = ParseAchievements(profileWebpage.QuerySelector("section[id='achievements-section']"))
                    .CompetitiveSkillRating = If(UShort.TryParse(profileWebpage.QuerySelector("div.competitive-rank div")?.TextContent, .CompetitiveSkillRating), .CompetitiveSkillRating, 0)
                    .CompetitiveRankImageUrl = If(TryCast(profileWebpage.QuerySelector("div.competitive-rank img"), IHtmlImageElement)?.Source, String.Empty)
                    .EndorsementLevel = If(UShort.TryParse(profileWebpage.QuerySelector("div.endorsement-level div.u-center")?.TextContent, .EndorsementLevel), .EndorsementLevel, 0)
                    .Endorsements = ParseEndorsements(profileWebpage.QuerySelector("div.endorsement-level"))
                    .BlizzardId = _playerIdRegex.Match(profileWebpage.QuerySelectorAll("script").Last.TextContent).Value
                    .IsProfilePrivate = profileWebpage.QuerySelector(".masthead-permission-level-text")?.TextContent = "Private Profile"
                    .ProfileUrl = profileUrl
                    .PlayerIconUrl = DirectCast(profileWebpage.QuerySelector(".player-portrait"), IHtmlImageElement)?.Source
                    .PlayerLevel = ParsePlayerLevel(profileWebpage.QuerySelector(".masthead-player-progression"))
                    .Platform = platform
                    .Stats = ParseStats(profileWebpage)
                    .Username = profileWebpage.QuerySelector(".header-masthead").TextContent
                End With
            End If

            Return player
        End Function

        Private Function ParsePlayerLevel(levelContent As IElement) As UShort
            Dim level As UShort
            Dim prestigeLevel As UShort
            Dim prestigeModifier As UShort
            Dim cssProperty As ICssProperty

            level = levelContent.QuerySelector(".u-vertical-center").TextContent

            cssProperty = levelContent.QuerySelector(".player-level").Style.Children.First
            prestigeLevel = OverstarchPrestige.PrestigeBorders(Path.GetFileName(New Uri(cssProperty.Value.Split(ChrW(34))(1)).LocalPath).Split("."c)(0))
            If prestigeLevel <> 0 Then prestigeLevel *= 100

            cssProperty = levelContent.QuerySelector(".player-level .player-rank")?.Style.Children.First
            If cssProperty IsNot Nothing Then
                prestigeModifier = OverstarchPrestige.PrestigeStars(Path.GetFileName(New Uri(cssProperty.Value.Split(ChrW(34))(1)).LocalPath).Split("."c)(0))
            End If
            If prestigeModifier <> 0 Then prestigeModifier *= 100

            Return level + prestigeLevel + prestigeModifier
        End Function

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

        Private Function ParseAchievements(achievementContent As IElement) As List(Of OverwatchAchievement)
            Dim achievements As New List(Of OverwatchAchievement)

            If achievementContent IsNot Nothing Then
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
            End If

            Return achievements
        End Function

        Private Function ParseStats(profile As IDocument) As Dictionary(Of OverwatchGamemode, List(Of OverwatchStat))
            Dim playerStats As New Dictionary(Of OverwatchGamemode, List(Of OverwatchStat))

            For Each gamemode In GetType(OverwatchGamemode).GetEnumNames
                Dim gamemodeContent As IElement = profile.QuerySelector($"div[id='{gamemode.ToLower}']")
                Dim heroIdDict As New Dictionary(Of String, String)
                Dim gamemodeStats As New List(Of OverwatchStat)

                If gamemodeContent IsNot Nothing Then
                    For Each hero As IHtmlOptionElement In gamemodeContent.QuerySelectorAll("option[value^='0x02E']")
                        Dim heroName As String = FormatHeroName(hero.TextContent)
                        If Not String.IsNullOrEmpty(heroName) Then heroIdDict.Add(hero.Value, heroName)
                    Next

                    For Each section In gamemodeContent.QuerySelectorAll("div[data-group-id='stats']")
                        Dim categoryId As String = section.GetAttribute("data-category-id")

                        If String.IsNullOrEmpty(categoryId) Then
                            Throw New FormatException("Blizzard returned invalid data. Please wait a moment before attempting to retrieve a player profile again.")
                        End If

                        If heroIdDict.ContainsKey(categoryId) Then
                            For Each dataTable In section.QuerySelectorAll($"div[data-category-id='{categoryId}'] table.DataTable")
                                For Each dataRow In dataTable.QuerySelectorAll("tbody tr")

                                    gamemodeStats.Add(New OverwatchStat With {
                                        .Category = dataTable.QuerySelector("thead").TextContent,
                                        .Hero = heroIdDict(categoryId),
                                        .Name = dataRow.Children(0).TextContent,
                                        .Value = dataRow.Children(1).TextContent.ConvertValueToDouble
                                    })

                                Next
                            Next
                        End If
                    Next
                End If

                If gamemodeStats.Count > 0 Then playerStats.Add([Enum].Parse(GetType(OverwatchGamemode), gamemode), gamemodeStats)
            Next

            Return playerStats
        End Function

        Private Function FormatHeroName(name As String) As String
            If name.ToLower.Contains("all heroes") Then
                Return "AllHeroes"
            Else
                Return name.Trim.RemoveDiacritics.RemoveNonAlphanumeric.RemoveWhitespace
            End If

        End Function
    End Class
End Namespace