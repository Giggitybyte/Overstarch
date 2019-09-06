Imports System.Runtime.CompilerServices

Namespace Extensions
    Public Module UShortExtensions
        <Extension()>
        Public Function ToRank(skillRating As UShort) As String
            Select Case skillRating
                Case 0
                    Return "Unranked"
                Case 1 To 1499
                    Return "Bronze"
                Case 1500 To 1999
                    Return "Silver"
                Case 2000 To 2499
                    Return "Gold"
                Case 2500 To 2999
                    Return "Platinum"
                Case 3000 To 3499
                    Return "Diamond"
                Case 3500 To 3999
                    Return "Master"
                Case > 4000
                    Return "Grandmaster"
                Case Else
                    Throw New ArgumentOutOfRangeException("skillRating", "Invalid number provided; this exception should never be thrown.")
            End Select
        End Function
    End Module
End Namespace