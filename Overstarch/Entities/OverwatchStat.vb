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
            Get
                Return _category
            End Get
        End Property

        ''' <summary>
        ''' The hero this stat is for.
        ''' </summary>
        ''' <returns>A <see cref="String"/></returns>
        Public ReadOnly Property Hero As String
            Get
                Return _hero
            End Get
        End Property

        ''' <summary>
        ''' The name of this stat.
        ''' </summary>
        ''' <returns>A <see cref="String"/></returns>
        Public ReadOnly Property Name As String
            Get
                Return _name
            End Get
        End Property

        ''' <summary>
        ''' The value of this stat.
        ''' </summary>
        ''' <returns>A <see cref="Double"/></returns>
        Public ReadOnly Property Value As Double
            Get
                Return _value
            End Get
        End Property

        ''' <summary>
        ''' Internal backing field for user facing Property.
        ''' </summary>
        Friend _category As String

        ''' <summary>
        ''' Internal backing field for user facing Property.
        ''' </summary>
        Friend _hero As String

        ''' <summary>
        ''' Internal backing field for user facing Property.
        ''' </summary>
        Friend _name As String

        ''' <summary>
        ''' Internal backing field for user facing Property.
        ''' </summary>
        Friend _value As Double
    End Class
End Namespace