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
        Private Shared ReadOnly _playerIdRegex As Regex = New Regex("\d+")
        Private Shared ReadOnly _webpageParser As BrowsingContext = BrowsingContext.[New](Configuration.Default.WithDefaultLoader.WithCss)

        Friend Async Function ParseAsync(username As String, platform As OverwatchPlatform) As Task(Of OverwatchPlayer)
            Dim profileUrl As String = $"{OverstarchUtilities.BaseUrl}/career/{platform.ToString.ToLower}/{username.Replace("#"c, "-"c)}"
            Dim profileWebpage As IDocument = Await _webpageParser.OpenAsync(profileUrl).ConfigureAwait(False)

            If profileWebpage.QuerySelector("h1.u-align-center")?.FirstChild.TextContent = "Profile Not Found" Then Throw New ArgumentException("Provided username does not exist on this platform.")

            Dim player As New OverwatchPlayer With {
                ._achievements = ParseAchievements(profileWebpage.QuerySelector("section[id='achievements-section']")),
                ._blizzardId = _playerIdRegex.Match(profileWebpage.QuerySelectorAll("script").Last.TextContent).Value,
                ._endorsements = ParseEndorsements(profileWebpage.QuerySelector("div.endorsement-level")),
                ._platform = platform,
                ._profileUrl = profileUrl,
                ._isPrivateProfile = profileWebpage.QuerySelector(".masthead-permission-level-text")?.TextContent = "Private Profile",
                ._iconUrl = DirectCast(profileWebpage.QuerySelector(".player-portrait"), IHtmlImageElement)?.Source,
                ._level = ParsePlayerLevel(profileWebpage.QuerySelector(".masthead-player-progression")),
                ._stats = ParseStats(profileWebpage),
                ._skillRatings = ParseSkillRatings(profileWebpage.QuerySelector(".show-for-lg")),
                ._username = profileWebpage.QuerySelector(".header-masthead").TextContent
            }

            Dim endorsementLevel As UShort = 0
            UShort.TryParse(profileWebpage.QuerySelector(".EndorsementIcon-tooltip .u-center")?.TextContent, endorsementLevel)
            player._endorsementLevel = endorsementLevel

            profileWebpage.Dispose()
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
                Dim uid As String = Path.GetFileName(New Uri(cssProperty.Value.Split(ChrW(34))(1)).LocalPath).Split("."c)(0)
                If Not String.IsNullOrEmpty(uid) Then prestigeModifier = OverstarchPrestige.PrestigeStars(uid)
            End If
            If prestigeModifier <> 0 Then prestigeModifier *= 100

            Return level + prestigeLevel + prestigeModifier
        End Function

        Private Function ParseEndorsements(endorsementContent As IElement) As IReadOnlyDictionary(Of OverwatchEndorsement, Decimal)
            Dim endorsements As New Dictionary(Of OverwatchEndorsement, Decimal) From {
                {OverwatchEndorsement.GoodTeammate, 0.0},
                {OverwatchEndorsement.Shotcaller, 0.0},
                {OverwatchEndorsement.Sportsmanship, 0.0}
            }

            If endorsementContent IsNot Nothing Then
                For Each endorsement As IElement In endorsementContent.QuerySelectorAll("svg")
                    Dim percentage As String = endorsement.GetAttribute("data-value")

                    If percentage IsNot Nothing Then
                        Dim endorsementName As String = endorsement.GetAttribute("class").Substring(endorsement.GetAttribute("class").IndexOf("--") + 2)
                        Dim endorsementEnum As OverwatchEndorsement

                        Select Case endorsementName
                            Case "teammate"
                                endorsementEnum = OverwatchEndorsement.GoodTeammate
                            Case "shotcaller"
                                endorsementEnum = OverwatchEndorsement.Shotcaller
                            Case "sportsmanship"
                                endorsementEnum = OverwatchEndorsement.Sportsmanship
                        End Select

                        endorsements(endorsementEnum) = Decimal.Parse(percentage)
                    End If
                Next
            End If

            Return endorsements
        End Function

        Private Function ParseAchievements(achievementContent As IElement) As IReadOnlyList(Of OverwatchAchievement)
            Dim achievements As New List(Of OverwatchAchievement)

            If achievementContent IsNot Nothing Then
                For Each categoryElement As IElement In achievementContent.QuerySelectorAll("select > option")
                    Dim categoryData As IElement = achievementContent.QuerySelector($"div[data-category-id='{categoryElement.GetAttribute("value")}']")

                    For Each achievementData As IHtmlDivElement In categoryData.QuerySelectorAll("div.achievement-card")
                        Dim category As OverwatchAchievementCategory
                        Dim description As String = categoryData.QuerySelector($"div[id='{achievementData.Dataset("tooltip")}']").QuerySelector("p[class='h6']").TextContent
                        Dim iconUrl As String = If(TryCast(achievementData.QuerySelector("img.media-card-fill"), IHtmlImageElement).Source, String.Empty)
                        Dim hasAchieved As Boolean = Not achievementData.GetAttribute("class").Contains("m-disabled")
                        Dim name As String = achievementData.QuerySelector("div.media-card-title").TextContent

                        [Enum].TryParse(categoryElement.GetAttribute("option-id").ToUpper, category)
                        achievements.Add(New OverwatchAchievement(category, description, hasAchieved, iconUrl, name))
                    Next
                Next
            End If

            Return achievements
        End Function

        Private Function ParseSkillRatings(playerProgression As IElement) As IReadOnlyDictionary(Of OverwatchRole, UShort)
            Dim roleProgressions = playerProgression.QuerySelectorAll(".competitive-rank .competitive-rank-role")
            Dim rankList As New Dictionary(Of OverwatchRole, UShort)

            If roleProgressions Is Nothing Then Return rankList

            For Each roleProgression In roleProgressions
                Dim roleSection = roleProgression.Children(1).QuerySelector(".competitive-rank-section .competitive-rank-tier")
                Dim role = [Enum].Parse(Of OverwatchRole)(roleSection.GetAttribute("data-ow-tooltip-text").Split(" ").First)

                Dim sr As UShort
                UShort.TryParse(roleProgression.Children(1).TextContent, sr)

                rankList.Add(role, sr)
            Next

            Return rankList
        End Function

        Private Function ParseStats(profile As IDocument) As IReadOnlyDictionary(Of OverwatchGamemode, IReadOnlyList(Of OverwatchStat))
            Dim playerStats As New Dictionary(Of OverwatchGamemode, IReadOnlyList(Of OverwatchStat))

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

                                    Dim category As String = dataTable.QuerySelector("thead").TextContent
                                    Dim hero As String = heroIdDict(categoryId)
                                    Dim name As String = dataRow.Children(0).TextContent
                                    Dim value As String = dataRow.Children(1).TextContent.ConvertValueToDouble

                                    gamemodeStats.Add(New OverwatchStat(category, hero, name, value))

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