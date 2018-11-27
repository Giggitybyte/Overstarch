Imports System.Globalization
Imports System.Runtime.CompilerServices
Imports System.Text

Namespace Extensions
    Friend Module StringExtensions

        ''' <summary>
        ''' Internal extension: replaces all characters that have accents with their non-accented equivalent.
        ''' </summary>
        <Extension()>
        Friend Function RemoveDiacritics(text As String) As String
            If String.IsNullOrWhiteSpace(text) Then Return text

            text = text.Normalize(NormalizationForm.FormD)
            Return New String(text.Where(Function(c) CharUnicodeInfo.GetUnicodeCategory(c) <> UnicodeCategory.NonSpacingMark).ToArray()).Normalize(NormalizationForm.FormC)
        End Function

        ''' <summary>
        ''' Internal extension: removes all non-alphanumeric characters sans whitespace and hyphens.
        ''' </summary>
        <Extension()>
        Friend Function RemoveNonAlphanumeric(text As String) As String
            Return New String(Array.FindAll(text.ToCharArray, (Function(c) (Char.IsLetterOrDigit(c) OrElse Char.IsWhiteSpace(c) OrElse c = "-"c))))
        End Function

        ''' <summary>
        ''' Internal extension: removes all whitespace characters.
        ''' </summary>
        <Extension()>
        Friend Function RemoveWhitespace(text As String) As String
            Return New String(Array.FindAll(text.ToCharArray, (Function(c) (Not Char.IsWhiteSpace(c)))))
        End Function
    End Module
End Namespace