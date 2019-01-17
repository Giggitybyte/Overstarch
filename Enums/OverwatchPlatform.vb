Imports Newtonsoft.Json
Imports Newtonsoft.Json.Converters

Namespace Enums

    ''' <summary>
    ''' Overwatch platform.
    ''' </summary>
    <JsonConverter(GetType(StringEnumConverter))>
    Public Enum OverwatchPlatform

        ''' <summary>
        ''' Battle.net.
        ''' </summary>
        PC = 1

        ''' <summary>
        ''' Xbox One.
        ''' </summary>
        XBL = 2

        ''' <summary>
        ''' Playstation 4.
        ''' </summary>
        PSN = 3
    End Enum
End Namespace