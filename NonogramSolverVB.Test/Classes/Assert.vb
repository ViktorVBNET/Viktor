Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Threading.Tasks

Public Class TestFailureException
    Inherits Exception

    Public Sub New()
    End Sub

    Public Sub New(ByVal message As String)
        MyBase.New(message)
    End Sub

    Public Sub New(ByVal message As String, ByVal inner As Exception)
        MyBase.New(message, inner)
    End Sub
End Class

Module Assert
        Sub [True](ByVal value As Boolean, ByVal message As String)
            If Not value Then Throw New TestFailureException(message)
        End Sub

        Sub [True](ByVal value As Boolean)
            If Not value Then Throw New TestFailureException("Value was false.")
        End Sub

        Sub [False](ByVal value As Boolean, ByVal message As String)
            [True](Not value, message)
        End Sub

        Sub [False](ByVal value As Boolean)
            [True](Not value)
        End Sub
    End Module