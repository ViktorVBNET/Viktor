Imports System
Imports System.Collections.Generic
Imports System.Diagnostics
Imports System.Text
Imports System.Runtime.InteropServices

Public Class ConstraintSegment
    Private Sub New(ByVal _constraint As Constraint, ByVal _startIndex As Integer)
        [Next] = Nothing
        Prev = Nothing
        Constraint = _constraint
        StartIndex = _startIndex
    End Sub

    Public Property [Next] As ConstraintSegment
    Public Property Prev As ConstraintSegment
    Public Property Constraint As Constraint
    Public Property StartIndex As Integer

    Public ReadOnly Property EndIndex As Integer
        Get
            Return StartIndex + CInt(Constraint.number)
        End Get
    End Property

    Public ReadOnly Property IsGap As Boolean
        Get
            Return Constraint.color = ColorSpace.Empty
        End Get
    End Property

    Public Shared Sub CreateChain(ByVal constraints As IEnumerable(Of Constraint), <Out> ByRef begin As ConstraintSegment, <Out> ByRef [end] As ConstraintSegment)
        begin = Nothing
        [end] = Nothing
        Dim current As ConstraintSegment = Nothing

        For Each constr As Constraint In constraints

            If current Is Nothing Then
                current = New ConstraintSegment(constr, 0)
                begin = current
            Else
                current = current.CreateNext(constr)
            End If
        Next

        [end] = current
    End Sub

    Public Function CreateNext(ByVal constraint As Constraint) As ConstraintSegment
        Debug.Assert([Next] Is Nothing)

        If constraint.color = Me.Constraint.color Then
            Dim newConstraint As Constraint = New Constraint With {
                .color = ColorSpace.Empty,
                .number = 1}

            Dim gapSegment As ConstraintSegment = CreateNext(newConstraint)
            Debug.Assert(gapSegment.IsGap)
            Return gapSegment.CreateNext(constraint)
        End If

        Dim newSegment = New ConstraintSegment(constraint, EndIndex)
        InsertAfterSelf(newSegment)
        Return newSegment
    End Function

    Public Function Bump(ByVal maxIndex As Integer) As Boolean
        If IsGap Then
            Debug.Assert(maxIndex = EndIndex)

            If IsNecessaryGap() Then
                maxIndex = EndIndex - 1
            End If

            Return Prev.Bump(maxIndex)
        End If

        If EndIndex >= maxIndex Then
            Debug.Assert(EndIndex = maxIndex, "Overlapping ConstraintSegments!")
            If Prev Is Nothing Then Return False
            Return Prev.Bump(StartIndex)
        Else
            StartIndex += 1

            If [Next] IsNot Nothing Then
                Debug.Assert([Next].IsGap)
                [Next].ShrinkForBump()
            End If

            If Prev IsNot Nothing Then
                Prev.GrowForBump()
            End If

            ResetFolowing()
            Return True
        End If
    End Function

    Public Sub Validate(ByVal maxIndex As Integer)
        Debug.Assert(EndIndex <= maxIndex)

        If [Next] IsNot Nothing Then
            Debug.Assert(Constraint.color <> [Next].Constraint.color)
            Debug.Assert(EndIndex = [Next].StartIndex)
            [Next].Validate(maxIndex)
        End If
    End Sub

    Private Sub RemoveSelf()
        If Prev IsNot Nothing Then
            Prev.[Next] = [Next]
        End If

        If [Next] IsNot Nothing Then
            [Next].Prev = Prev
        End If
    End Sub

    Private Sub InsertAfterSelf(ByVal segment As ConstraintSegment)
        segment.Prev = Me
        segment.[Next] = [Next]

        If [Next] IsNot Nothing Then
            [Next].Prev = segment
        End If

        [Next] = segment
    End Sub

    Private Sub ShrinkForBump()
        Debug.Assert(IsGap)
        StartIndex += 1
        Dim constr = Constraint
        constr.number -= 1
        Constraint = constr
        Debug.Assert(StartIndex = Prev.EndIndex)

        If Constraint.number = 0 Then
            Debug.Assert(Not IsNecessaryGap(), "Necessary gap shrunk to 0!")
            Debug.Assert(Prev IsNot Nothing AndAlso [Next] IsNot Nothing, "Gap found not in middle of segments")
            RemoveSelf()
        End If
    End Sub

    Private Sub GrowForBump()
        If IsGap Then
            Dim con = constraint
            con.number += 1
            constraint = con
            Debug.Assert(EndIndex = [Next].StartIndex)
            Return
        End If

        Dim _constraint = New Constraint With {.color = ColorSpace.Empty, .number = 1}

        Dim gap As ConstraintSegment = New ConstraintSegment(_constraint, EndIndex)
        InsertAfterSelf(gap)
    End Sub

    Private Sub ResetFolowing()
        If [Next] Is Nothing Then Return

        If [Next].IsGap Then

            If [Next].IsNecessaryGap() Then
                Dim constr = [Next].Constraint
                constr.number = 1
                [Next].Constraint = constr
            Else
                [Next].RemoveSelf()
            End If
        End If

        [Next].StartIndex = EndIndex
        [Next].ResetFolowing()
    End Sub

    Private Function IsNecessaryGap() As Boolean
        If Not IsGap Then Return False
        Debug.Assert(Prev IsNot Nothing AndAlso [Next] IsNot Nothing)
        Return Prev.Constraint.color = [Next].Constraint.color
    End Function
End Class