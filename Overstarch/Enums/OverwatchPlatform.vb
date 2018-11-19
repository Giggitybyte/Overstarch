Imports Newtonsoft.Json
Imports Newtonsoft.Json.Converters

Namespace Enums

    ''' <summary>
    ''' 
    ''' </summary>
    <Flags, JsonConverter(GetType(StringEnumConverter))>
    Public Enum OverwatchPlatform

        ''' <summary>
        ''' 
        ''' </summary>
        PC = 1

        ''' <summary>
        ''' 
        ''' </summary>
        XBL = 2

        ''' <summary>
        ''' 
        ''' </summary>
        PSN = 4
    End Enum
End Namespace