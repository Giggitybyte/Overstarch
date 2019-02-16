Namespace Entities

    ''' <summary>
    ''' A representation of an Overwatch stat.
    ''' </summary>
    Public NotInheritable Class OverwatchStat

        ''' <summary>
        ''' The category for this stat.
        ''' </summary>
        ''' <returns>A <see cref="String"/></returns>
        Public ReadOnly Property Category As String

        ''' <summary>
        ''' The hero this stat is for.
        ''' </summary>
        ''' <returns>A <see cref="String"/></returns>
        Public ReadOnly Property Hero As String

        ''' <summary>
        ''' The name of this stat.
        ''' </summary>
        ''' <returns>A <see cref="String"/></returns>
        Public ReadOnly Property Name As String

        ''' <summary>
        ''' The value of this stat.
        ''' </summary>
        ''' <returns>A <see cref="Double"/></returns>
        Public ReadOnly Property Value As Double

        Friend Sub New(category As String, hero As String,
                       name As String, value As String)

            _Category = category
            _Hero = hero
            _Name = name
            _Value = value

        End Sub

    End Class
End Namespace