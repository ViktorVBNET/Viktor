Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.Diagnostics
Imports System.Linq
Imports System.Text
Imports System.Threading.Tasks

Interface IBoardView
    Default ReadOnly Property Item(ByVal index As Integer) As ColorSet
    ReadOnly Property Count As Integer
    ReadOnly Property ConstraintState As ConstraintState
    Function IntersectAll(ByVal colorSets As IReadOnlyList(Of ColorSet)) As ConstrainResult
End Interface

Class BoardState
    Private mColors As ColorSet(,)
    Private mRowConstraints As ConstraintState()
    Private mColConstraints As ConstraintState()
    Private mOwningBoard As Board
    Private ReadOnly mGuesser As Guesser

    Public Enum IntersectResult
        Changed
        NoChange
        NoSolution
    End Enum

    Public Sub New(ByVal _board As Board)
        mOwningBoard = _board
        mGuesser = New Guesser(_board)
        CreateTiles()
        CreateConstraintStates(_board)
    End Sub

    Private Sub New(ByVal other As BoardState)
        mColors = CType(other.mColors.Clone(), ColorSet(,))
        mRowConstraints = other.mRowConstraints.[Select](Function(c) c.Clone()).ToArray()
        mColConstraints = other.mColConstraints.[Select](Function(c) c.Clone()).ToArray()
        mOwningBoard = other.mOwningBoard
        mGuesser = New Guesser(other.mOwningBoard)
    End Sub

    Default Public ReadOnly Property Item(ByVal x As Integer, ByVal y As Integer) As ColorSet
        Get
            Return mColors(x, y)
        End Get
    End Property

    Public ReadOnly Property Guesser As Guesser
        Get
            Return mGuesser
        End Get
    End Property

    Public ReadOnly Property RowConstraintStates As IReadOnlyList(Of ConstraintState)
        Get
            Return mRowConstraints
        End Get
    End Property

    Public ReadOnly Property ColConstraintStates As IReadOnlyList(Of ConstraintState)
        Get
            Return mColConstraints
        End Get
    End Property

    Public Function ExtractSolvedBoard() As ISolvedBoard
        Dim numRows As Integer = mOwningBoard.NumRows
        Dim numCols As Integer = mOwningBoard.NumColumns
        Dim solvedBoard As SolvedBoard = New SolvedBoard(numRows, numCols)

        For i As Integer = 0 To numRows - 1
            For j As Integer = 0 To numCols - 1
                Dim tile As ColorSet = mColors(i, j)
                Debug.Assert(tile.IsSingle())
                solvedBoard(i, j) = tile.GetSingleColor()
            Next
        Next

        Return solvedBoard
    End Function

    Public Function CreateRowView(ByVal index As Integer) As IBoardView
        Debug.Assert(index >= 0 AndAlso index < mOwningBoard.NumRows)
        Return New RowView(index, Me)
    End Function

    Public Function CreateColView(ByVal index As Integer) As IBoardView
        Debug.Assert(index >= 0 AndAlso index < mOwningBoard.NumColumns)
        Return New ColView(index, Me)
    End Function

    Public Function CreateNewLayer() As BoardState
        Return New BoardState(Me)
    End Function

    Public Sub SetColor(ByVal x As Integer, ByVal y As Integer, ByVal value As ColorSet)
        Dim original As ColorSet = mColors(x, y)
        mColors(x, y) = value

        If original <> value Then
            mOwningBoard.OnTileDirty(x, y)
        End If
    End Sub

    Public Function CalculateIsSolved() As Boolean
        For i As Integer = 0 To mOwningBoard.NumRows - 1

            For j As Integer = 0 To mOwningBoard.NumColumns - 1
                Dim val As ColorSet = mColors(i, j)
                If Not val.IsSingle() Then Return False
            Next
        Next

        Return True
    End Function

    Private Sub CreateTiles()
        Dim numRows As Integer = mOwningBoard.NumRows
        Dim numCols As Integer = mOwningBoard.NumColumns
        mColors = New ColorSet(numRows - 1, numCols - 1) {}
        Dim fullColor = ColorSet.CreateFullColorSet(mOwningBoard.ColorSpace.MaxColor)

        For i As Integer = 0 To numRows - 1
            For j As Integer = 0 To numCols - 1
                mColors(i, j) = fullColor
            Next
        Next
    End Sub

    Private Sub CreateConstraintStates(ByVal board As Board)
        mRowConstraints = board.RowConstraints.[Select](Function(c) New ConstraintState(board.NumColumns, c.Count)).ToArray()
        mColConstraints = board.ColumnConstraints.[Select](Function(c) New ConstraintState(board.NumRows, c.Count)).ToArray()
    End Sub

    Private Class RowView
        Implements IBoardView

        Private ReadOnly mRowIndex As Integer
        Private ReadOnly mOwner As BoardState

        Public Sub New(ByVal rowIndex As Integer, ByVal owner As BoardState)
            Me.mRowIndex = rowIndex
            Me.mOwner = owner
        End Sub

        Default Public ReadOnly Property Item(ByVal index As Integer) As ColorSet Implements IBoardView.Item
            Get
                Return mOwner.mColors(mRowIndex, index)
            End Get
        End Property

        Public ReadOnly Property Count As Integer Implements IBoardView.Count
            Get
                Return mOwner.mOwningBoard.NumColumns
            End Get
        End Property

        Public ReadOnly Property ConstraintState As ConstraintState Implements IBoardView.ConstraintState
            Get
                Return mOwner.mRowConstraints(mRowIndex)
            End Get
        End Property

        Public Function IntersectAll(ByVal colorSets As IReadOnlyList(Of ColorSet)) As ConstrainResult Implements IBoardView.IntersectAll
            For i As Integer = 0 To Count - 1
                Dim color = mOwner.mColors(mRowIndex, i)
                Dim newColor As ColorSet = color.Intersect(colorSets(i))
                If newColor.IsEmpty Then Return ConstrainResult.NoSolution

                If newColor <> color Then
                    mOwner.mColors(mRowIndex, i) = newColor
                    mOwner.mOwningBoard.OnTileDirty(mRowIndex, i)
                End If
            Next

            Return ConstrainResult.Success
        End Function
    End Class

    Private Class ColView
        Implements IBoardView

        Private ReadOnly mColIndex As Integer
        Private ReadOnly mOwner As BoardState

        Public Sub New(ByVal colIndex As Integer, ByVal owner As BoardState)
            Me.mColIndex = colIndex
            Me.mOwner = owner
        End Sub

        Default Public ReadOnly Property Item(ByVal index As Integer) As ColorSet Implements IBoardView.Item
            Get
                Return mOwner.mColors(index, mColIndex)
            End Get
        End Property

        Public ReadOnly Property Count As Integer Implements IBoardView.Count
            Get
                Return mOwner.mOwningBoard.NumRows
            End Get
        End Property

        Public ReadOnly Property ConstraintState As ConstraintState Implements IBoardView.ConstraintState
            Get
                Return mOwner.mColConstraints(mColIndex)
            End Get
        End Property

        Public Function IntersectAll(ByVal colorSets As IReadOnlyList(Of ColorSet)) As ConstrainResult Implements IBoardView.IntersectAll
            For i As Integer = 0 To Count - 1
                Dim color = mOwner.mColors(i, mColIndex)
                Dim newColor As ColorSet = color.Intersect(colorSets(i))
                If newColor.IsEmpty Then Return ConstrainResult.NoSolution

                If newColor <> color Then
                    mOwner.mColors(i, mColIndex) = newColor
                    mOwner.mOwningBoard.OnTileDirty(i, mColIndex)
                End If
            Next

            Return ConstrainResult.Success
        End Function
    End Class
End Class