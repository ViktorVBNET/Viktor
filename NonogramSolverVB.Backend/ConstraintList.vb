Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.Diagnostics
Imports System.Linq
Imports System.Text

Friend Class ConstraintList
    Private ReadOnly mConstraintSet As IConstraintSet
    Private ReadOnly mBoardSize As Integer

    Public Sub New(ByVal _index As Integer, ByVal _isRow As Boolean, ByVal _size As Integer, ByVal _constraints As IConstraintSet)
        mConstraintSet = _constraints
        mBoardSize = _size
        Index = _index
        IsRow = _isRow
    End Sub

    Public Property Index As Integer
    Public Property IsRow As Boolean
    Public Property IsDirty As Boolean = False

    Public Function ConstrainBoard(ByVal mBoardView As IBoardView) As ConstrainResult
        Dim segmentBegin As ConstraintSegment = Nothing, segmentEnd As ConstraintSegment = Nothing
        ConstraintSegment.CreateChain(mConstraintSet, segmentBegin, segmentEnd)
        Dim finalColors As ColorSet() = New ColorSet(mBoardView.Count - 1) {}

        Do
            Dim segmentColors As IReadOnlyList(Of UInteger) = GetColorsFromSegments(segmentBegin)

            If AreCompatible(mBoardView, segmentColors) Then
                Merge(segmentColors, finalColors)
            End If
        Loop While segmentEnd.Bump(mBoardView.Count)

        Dim result = mBoardView.IntersectAll(finalColors)
        Return result
    End Function

    Private Function GetColorsFromSegments(ByVal _begin As ConstraintSegment) As IReadOnlyList(Of UInteger)
        Dim colorSets As UInteger() = New UInteger(mBoardSize - 1) {}
        Dim current As ConstraintSegment = _begin
        Dim emptyColor As UInteger = ColorSpace.Empty
        Dim i As Integer = 0

        While i <> current.StartIndex
            colorSets(i) = emptyColor
            i += 1
        End While

        While current IsNot Nothing
            Debug.Assert(i = current.StartIndex)
            Dim currentColor As UInteger = current.Constraint.color

            While i <> current.EndIndex
                colorSets(i) = currentColor
                i += 1
            End While

            current = current.[Next]
        End While

        While i <> mBoardSize
            colorSets(i) = emptyColor
            i += 1
        End While

        Return colorSets
    End Function

    Private Function AreCompatible(ByVal _boardColors As IBoardView, ByVal _segmentColors As IReadOnlyList(Of UInteger)) As Boolean
        Debug.Assert(_boardColors.Count = _segmentColors.Count)

        For i As Integer = 0 To _segmentColors.Count - 1
            If Not _boardColors(i).HasColor(_segmentColors(i)) Then Return False
        Next

        Return True
    End Function

    Private Sub Merge(ByVal _from As IReadOnlyList(Of UInteger), ByVal _into As ColorSet())
        Debug.Assert(_from.Count = _into.Length)

        For i As Integer = 0 To _from.Count - 1
            _into(i) = _into(i).AddColor(_from(i))
        Next
    End Sub
End Class