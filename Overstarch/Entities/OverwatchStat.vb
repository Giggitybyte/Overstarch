Namespace Entities

    ''' <summary>
    ''' A representation of an Overwatch stat.
    ''' </summary>
    Public NotInheritable Class OverwatchStat

        ''' <summary>
        ''' The hero this stat is for.
        ''' </summary>
        ''' <returns>A <see cref="String"/></returns>
        Public Property Hero As String

        ''' <summary>
        ''' The category for this stat.
        ''' </summary>
        ''' <returns>A <see cref="String"/></returns>
        Public Property Category As String

        ''' <summary>
        ''' The name of this stat.
        ''' </summary>
        ''' <returns>A <see cref="String"/></returns>
        Public Property Name As String

        ''' <summary>
        ''' The value of this stat.
        ''' </summary>
        ''' <returns>A <see cref="Double"/></returns>
        Public Property Value As Double

    End Class
End Namespace