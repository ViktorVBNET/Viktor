Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Threading.Tasks

Friend Class ConstraintListWrapper
    Public Sub New(ByVal _index As Integer,
                   ByVal _isRow As Boolean,
                   ByVal _constraint As IConstraintList)
        Constraint = _constraint
        Index = _index
        IsRow = _isRow
    End Sub

    Public Property Constraint As IConstraintList
    Public Property Index As Integer
    Public Property IsRow As Boolean
    Public Property IsDirty As Boolean = False

    Public Function CreateBoardView(ByVal boardState As BoardState) As IBoardView
        Return If(IsRow, boardState.CreateRowView(Index), boardState.CreateColView(Index))
    End Function
End Class