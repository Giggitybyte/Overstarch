﻿Namespace Entities

    ''' <summary>
    ''' 
    ''' </summary>
    Public NotInheritable Class OverwatchAchievement

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property Category As String

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property Name As String

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property IconUrl As String

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property IsEarned As Boolean

        ''' <summary>
        ''' 
        ''' </summary>
        Friend Sub New()
            Throw New NotImplementedException
        End Sub

    End Class
End Namespace