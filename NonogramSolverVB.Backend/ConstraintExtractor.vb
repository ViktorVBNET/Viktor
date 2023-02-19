Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Threading.Tasks

''' <summary>
''' This class takes a 2 dimensional grid and extracts row / column constraints out of it
''' </summary>
Friend Class ConstraintExtractor
    Private ReadOnly grid As UInteger(,)
    Private ReadOnly numRows As Integer
    Private ReadOnly numCols As Integer
    Private current As Constraint
    Private currentSet As List(Of Constraint)

    Public Sub New(ByVal _grid As UInteger(,))
        Me.grid = _grid
        numRows = grid.GetLength(0)
        numCols = grid.GetLength(1)
        current = New Constraint()
        currentSet = New List(Of Constraint)()
    End Sub

    Public Property RowConstraints As IReadOnlyList(Of IConstraintSet)
    Public Property ColumnConstraints As IReadOnlyList(Of IConstraintSet)

    Public Sub Extract()
        RowConstraints = Enumerable.Range(0, numRows).[Select](Function(i) CreateRowConstraint(i)).ToList()
        ColumnConstraints = Enumerable.Range(0, numCols).[Select](Function(i) CreateColConstraint(i)).ToList()
    End Sub

    Private Function CreateRowConstraint(ByVal index As Integer) As IConstraintSet
        Dim getter As Func(Of Integer, UInteger) = Function(col) grid(index, col)
        Return CreateGenericConstraint(getter, numCols)
    End Function

    Private Function CreateColConstraint(ByVal index As Integer) As IConstraintSet
        Dim getter As Func(Of Integer, UInteger) = Function(row) grid(row, index)
        Return CreateGenericConstraint(getter, numRows)
    End Function

    ''' <summary>
    ''' If we were accumulating when we hit the end of a row,
    ''' the current constraint hasn't been pushed yet
    ''' </summary>
    ''' <param name="_getter"></param>
    ''' <param name="_count"></param>
    ''' <returns></returns>
    Public Function CreateGenericConstraint(ByVal _getter As Func(Of Integer, UInteger),
                                            ByVal _count As Integer) As IConstraintSet
        currentSet = New List(Of Constraint)()
        current = New Constraint()

        For i As Integer = 0 To _count - 1
            Dim color As UInteger = _getter(i)
            Accumulate(color)
        Next

        Push()
        Return BoardFactory.CreateConstraintSet(currentSet)
    End Function

    Private Sub Accumulate(ByVal _color As UInteger)
        If current.color <> _color Then
            Push()

            If _color <> 0 Then
                current.color = _color
                current.number = 1
            End If
        Else

            If _color <> 0 Then
                current.number += 1
            End If
        End If
    End Sub

    Private Sub Push()
        If current.number <> 0 Then
            currentSet.Add(current)
            current = New Constraint()
        End If
    End Sub
End Class