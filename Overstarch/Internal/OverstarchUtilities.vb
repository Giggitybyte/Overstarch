Imports System.Net

Namespace Internal

    ''' <summary>
    ''' Internal class containing utilites.
    ''' </summary>
    Friend NotInheritable Class OverstarchUtilities
        Friend Shared ReadOnly BaseUrl As String = "https://playoverwatch.com/en-us"

        ''' <summary>
        ''' Internal method: simply retrieves the source of a webpage.
        ''' </summary>
        ''' <param name="url">JSON URL.</param>
        ''' <returns>A <see cref="String"/> containing JSON.</returns>
        Friend Shared Async Function FetchWebPageAsync(url As String) As Task(Of String)
            Using webpage As New WebClient
                webpage.Headers(HttpRequestHeader.UserAgent) = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/70.0.3538.102 Safari/537.36"
                Return Await webpage.DownloadStringTaskAsync(url).ConfigureAwait(False)
            End Using
        End Function
    End Class
End Namespace