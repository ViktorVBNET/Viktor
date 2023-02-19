Imports System
Imports System.Collections.Generic
Imports System.Text

Public Structure Constraint
    Public Sub New(ByVal _color As UInteger, ByVal _number As UInteger)
        Me.color = _color
        Me.number = _number
    End Sub

    Public color As UInteger
    Public number As UInteger

    Public Shared Operator =(ByVal constr0 As Constraint, ByVal constr1 As Constraint) As Boolean
        Return (constr0.color = constr1.color) AndAlso (constr0.number = constr1.number)
    End Operator

    Public Shared Operator <>(ByVal constr0 As Constraint, ByVal constr1 As Constraint) As Boolean
        Return (constr0.color <> constr1.color) OrElse (constr0.number <> constr1.number)
    End Operator

    Public Overrides Function Equals(ByVal obj As Object) As Boolean
        If obj.[GetType]() = GetType(Constraint) Then
            Dim constr As Constraint = CType(obj, Constraint)
            Return Me = constr
        End If

        Return False
    End Function

    Public Overrides Function GetHashCode() As Integer
        Return color.GetHashCode And number.GetHashCode '  MyBase.GetHashCode()
    End Function
End Structure