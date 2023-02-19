Imports System
Imports System.Collections.Generic
Imports System.Text

Public Interface IBoard
    ReadOnly Property NumRows As Integer
    ReadOnly Property NumColumns As Integer
    ReadOnly Property RowConstraints As IReadOnlyList(Of IConstraintSet)
    ReadOnly Property ColumnConstraints As IReadOnlyList(Of IConstraintSet)
    ReadOnly Property ProgressManager As IProgressManager
    Function Solve() As ISolvedBoard
End Interface