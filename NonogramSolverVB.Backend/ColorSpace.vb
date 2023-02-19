Imports System
Imports System.Collections.Generic
Imports System.Diagnostics
Imports System.Text

Public Class InvalidColorException
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

Friend Class ColorSpace
    Public Sub New(ByVal _maxColor As UInteger)
        If _maxColor = 0 Then Throw New ArgumentOutOfRangeException("maxColor must not be 0")
        MaxColor = _maxColor
    End Sub

    Public Property MaxColor As UInteger
        Public Const Empty As UInteger = 0

        Public Sub ValidateColor(ByVal color As UInteger)
            If color >= MaxColor Then Throw New InvalidColorException($"Invalid color number {color}")
        End Sub
    End Class