Imports System.Collections.Generic
Imports System.Diagnostics
Imports System.Linq
Imports System.Text

Public Class UnsolvableBoardException
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

Friend Enum ConstrainResult
    Success
    NoSolution
End Enum

Friend Class Board
    Implements IBoard

    Private Const ENUMERATION_THRESHOLD As Integer = 1000
    Private mBoardManager As BoardManager
    Private mRowConstraintList As List(Of ConstraintListWrapper)
    Private mColConstraintList As List(Of ConstraintListWrapper)

    Private mDebugConstraintIndex As Integer
    Private mDebugConstraintIsRow As Boolean

    Private ReadOnly mDirtyConstraints As PriorityQueue(Of Long, ConstraintListWrapper)

    Public Sub New(ByVal _rowConstraints As IEnumerable(Of IConstraintSet),
                   ByVal _colConstraints As IEnumerable(Of IConstraintSet),
                   ByVal colors As ColorSpace)

        RowConstraints = _rowConstraints.ToList()
        ColumnConstraints = _colConstraints.ToList()
        NumRows = RowConstraints.Count
        NumColumns = ColumnConstraints.Count
        ColorSpace = colors
        mBoardManager = New BoardManager(Me)
        mDirtyConstraints = New PriorityQueue(Of Long, ConstraintListWrapper)()
        CreateConstraints()
    End Sub

    Public Property NumRows As Integer Implements IBoard.NumRows
    Public Property NumColumns As Integer Implements IBoard.NumColumns
    Public Property RowConstraints As IReadOnlyList(Of IConstraintSet) Implements IBoard.RowConstraints
    Public Property ColumnConstraints As IReadOnlyList(Of IConstraintSet) Implements IBoard.ColumnConstraints

    Public ReadOnly Property ProgressManager As IProgressManager Implements IBoard.ProgressManager
        Get
            'Throw New NotImplementedException()
            Return New NotImplementedException 'Nothing 'Throw(Of Object)(New NotImplementedException())
        End Get
    End Property

    Public Property ColorSpace As ColorSpace

    Public Function Solve() As ISolvedBoard Implements IBoard.Solve
        If mBoardManager Is Nothing Then
            mBoardManager = New BoardManager(Me)
        End If

        SolverLoop()
        Dim solved As ISolvedBoard = mBoardManager.CurrentLayer.ExtractSolvedBoard()
        mBoardManager = Nothing
        Return solved
    End Function

    Public Sub OnTileDirty(ByVal row As Integer, ByVal col As Integer)
        Dim rowConstr = mRowConstraintList(row)

        If Not rowConstr.IsDirty Then
            Enqueue(rowConstr)
        End If

        Dim colConstr = mColConstraintList(col)

        If Not colConstr.IsDirty Then
            Enqueue(colConstr)
        End If
    End Sub

    Private Sub CreateConstraints()
        Dim rowConstrainers = RowConstraints.[Select](
            Function(c)
                Return New Constrainer(c)
            End Function).ToList()
        Dim colConstrainers = ColumnConstraints.[Select](
            Function(c)
                Return New Constrainer(c)
            End Function).ToList()

        mRowConstraintList = Enumerable.Range(0, NumRows).[Select](
            Function(i)
                Return New ConstraintListWrapper(i, True, rowConstrainers(i))
            End Function).ToList()
        mColConstraintList = Enumerable.Range(0, NumColumns).[Select](
            Function(i)
                Return New ConstraintListWrapper(i, False, colConstrainers(i))
            End Function).ToList()

        Debug.Assert(mRowConstraintList.Count = NumRows)
        Debug.Assert(mColConstraintList.Count = NumColumns)
    End Sub

    Private Sub SolverLoop()
        SetAllConstraintsDirty()

        While True
            Dim result = ApplyConstraintsLoop()

            If result = ConstrainResult.NoSolution Then
                My.Application.Log.WriteEntry("No Solution hit")
                DoPopLayer()
                DoPushLayer()
                Continue While
            End If

            My.Application.Log.WriteEntry("Loop finished")
            If mBoardManager.CurrentLayer.CalculateIsSolved() Then Exit While
            DoPushLayer()
        End While
    End Sub

    Private Function ApplyConstraintsLoop() As ConstrainResult
        Dim tick As UInteger = 0
        Dim tickReport As UInteger = 1

        While mDirtyConstraints.Count > 0
            Dim constraint As ConstraintListWrapper = mDirtyConstraints.Dequeue()
            Debug.Assert(constraint.IsDirty)
            constraint.IsDirty = False
            mDebugConstraintIndex = constraint.Index
            mDebugConstraintIsRow = constraint.IsRow
            Dim result = ApplyConstraint(constraint)
            If result = ConstrainResult.NoSolution Then Return ConstrainResult.NoSolution
            tick += 1

            If tick = tickReport Then
                My.Application.Log.WriteEntry("TICK " & tick.ToString())

                tickReport = tickReport << 1
            End If
        End While

        Return ConstrainResult.Success
    End Function

    Private Function ApplyConstraint(ByVal _constraint As ConstraintListWrapper) As ConstrainResult
        Dim state As BoardState = mBoardManager.CurrentLayer
        Dim boardView As IBoardView = _constraint.CreateBoardView(state)
        Dim oldCost As Long = boardView.ConstraintState.Cost
        Debug.Assert(oldCost >= 0)
        _constraint.Constraint.CalculateEstimatedCost(boardView)
        Dim newCost As Long = boardView.ConstraintState.Cost
        Debug.Assert(newCost <= oldCost)

        If newCost <= ENUMERATION_THRESHOLD OrElse oldCost = newCost Then
            Return _constraint.Constraint.ConstrainBoard(boardView)
        Else
            _constraint.IsDirty = True
            mDirtyConstraints.Enqueue(newCost, _constraint)
            Return ConstrainResult.Success
        End If
    End Function

    Private Sub DoPushLayer()
        Dim boardState As BoardState = mBoardManager.CurrentLayer
        Dim guess As Guesser.Guess = boardState.Guesser.GenerateNext(boardState)

        'If guess Is null, we've tried everything for the current board state, go back one and try again
        'Repeat this in a loop until we succeed Or we've hit the end of our pushed layers
        While guess Is Nothing
            DoPopLayer()
            boardState = mBoardManager.CurrentLayer
            guess = boardState.Guesser.GenerateNext(boardState)
        End While

        mBoardManager.PushLayer()

        'Apply our guess to the New layer
        '(Not the current layer, Or we can't undo it...)
        Dim guessColor As ColorSet = ColorSet.CreateSingle(guess.Color)
        mBoardManager.CurrentLayer.SetColor(guess.X, guess.Y, guessColor)
    End Sub

    Private Sub DoPopLayer()
        'Popping a layer should _never_ fail. If it does, then we have exhausted all possibilities
        If Not mBoardManager.PopLayer() Then Throw New UnsolvableBoardException()

        'TODO: this is pretty inefficient, it's O(n log n) when it could be O(n)
        While mDirtyConstraints.Count > 0
            Dim constr = mDirtyConstraints.Dequeue()
            constr.IsDirty = False
        End While
    End Sub

    Private Sub SetAllConstraintsDirty()
        Dim boardState As BoardState = mBoardManager.CurrentLayer

        For Each constr In mRowConstraintList
            Dim boardView As IBoardView = constr.CreateBoardView(boardState)
            constr.Constraint.CalculateEstimatedCost(boardView)
            Enqueue(constr)
        Next

        For Each constr In mColConstraintList
            Dim boardView As IBoardView = constr.CreateBoardView(boardState)
            constr.Constraint.CalculateEstimatedCost(boardView)
            Enqueue(constr)
        Next
    End Sub

    Private Sub Enqueue(ByVal _constraint As ConstraintListWrapper)
        Dim state = mBoardManager.CurrentLayer
        Dim constraintStates = If(_constraint.IsRow, state.RowConstraintStates, state.ColConstraintStates)
        Dim cost As Long = constraintStates(_constraint.Index).Cost
        _constraint.IsDirty = True
        mDirtyConstraints.Enqueue(cost, _constraint)
    End Sub
End Class