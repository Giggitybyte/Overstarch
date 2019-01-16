Imports System.Globalization
Imports System.Runtime.CompilerServices

Namespace Extensions
    Friend Module DoubleExtensions

        ''' <summary>
        ''' Internal extension. Converts an Overwatch stat value to a <see cref="Double"/>.
        ''' </summary>
        ''' <param name="value">The Overwatch value to convert.</param>
        ''' <returns>A <see cref="Double"/> containing a stat value.</returns>
        <Extension()>
        Friend Function ConvertValueToDouble(value As String) As Double
            Dim timeValue As TimeSpan = Nothing
            Dim doubleValue As Double = Nothing

            If value.ToLower.Contains("hour") Then
                Return TimeSpan.FromHours(Integer.Parse(value.Substring(0, value.IndexOf(" ")))).TotalSeconds

            ElseIf value.ToLower.Contains("minute") Then
                Return TimeSpan.FromMinutes(Integer.Parse(value.Substring(0, value.IndexOf(" ")))).TotalSeconds

            ElseIf value.Contains(":") = False Then
                Return If(Double.TryParse(value.Replace(",", "").Replace("%", ""), doubleValue), doubleValue, 0)

            ElseIf TimeSpan.TryParseExact(value, "mm\:ss", CultureInfo.CurrentCulture, timeValue) OrElse
                Timespan.TryParseExact(value, "hh\:mm\:ss", CultureInfo.CurrentCulture, timeValue) Then

                Return timeValue.TotalSeconds
            End If

            Return If(Double.TryParse(value.Replace(",", "").Replace("%", ""), doubleValue), doubleValue, 0)
        End Function

    End Module
End Namespace