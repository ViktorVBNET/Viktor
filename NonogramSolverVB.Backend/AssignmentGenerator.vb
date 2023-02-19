Imports System
Imports System.Collections.Generic
Imports System.Diagnostics
Imports System.Linq
Imports System.Text
Imports System.Threading.Tasks
Imports System.Runtime.InteropServices

Friend Class AssignmentGenerator
    Private ReadOnly mBoardView As IBoardView
    Private ReadOnly mConstraints As IConstraintSet
    Private mFirstNode As GapNode
    Private mLastNode As GapNode

    Public Sub New(ByVal boardView As IBoardView, ByVal constraints As IConstraintSet)
        Me.mBoardView = boardView
        Me.mConstraints = constraints
    End Sub

    Public Function MoveNext() As Boolean
        If mLastNode Is Nothing Then
            Build(mFirstNode, mLastNode)
            Return mFirstNode.Reset(mBoardView)
        End If

        Dim currentNode As ColorNode = mLastNode.PrevNode

        While currentNode IsNot Nothing
            Dim success As Boolean = currentNode.Bump(mBoardView)
            If success Then Return True
            currentNode = currentNode.PrevNode.PrevNode
        End While

        Return False
    End Function

    Public Function GetAssignment() As IReadOnlyList(Of UInteger)
        If mFirstNode Is Nothing Then Throw New InvalidOperationException("MoveNext must be called before GetAssignment")
        Dim colors As UInteger() = New UInteger(mBoardView.Count - 1) {}
        mFirstNode.ExtractAssignment(colors)
        Return colors
    End Function

    Private Sub Build(<Out> ByRef firstGap As GapNode, <Out> ByRef lastGap As GapNode)
        firstGap = New GapNode(Nothing)
        Dim prevGap As GapNode = firstGap

        For i As Integer = 0 To mConstraints.Count - 1
            Dim min As Integer = mBoardView.ConstraintState.minValues(i)
            Dim max As Integer = mBoardView.ConstraintState.maxValues(i)
            Dim colorNode As ColorNode = New ColorNode(prevGap, mConstraints(i), min, max + 1)
            prevGap.SetNext(colorNode)
            prevGap = New GapNode(colorNode)
            colorNode.SetNext(prevGap)
        Next

        prevGap.MarkAsEnd(mBoardView.Count)
        lastGap = prevGap
    End Sub

    Private Class ColorNode
        Private ReadOnly mLength As Integer
        Private ReadOnly mColor As UInteger
        Private ReadOnly mMin As Integer
        Private ReadOnly mMax As Integer

        Public Sub New(ByVal prev As GapNode, ByVal constraint As Constraint, ByVal minValue As Integer, ByVal maxValue As Integer)
            mLength = CInt(constraint.number)
            mColor = constraint.color
            mMin = minValue
            mMax = maxValue
            PrevNode = prev
            NextNode = Nothing
            Start = mMin
        End Sub

        Public Property PrevNode As GapNode
        Public Property NextNode As GapNode
        Public Property Start As Integer

        Public ReadOnly Property [End] As Integer
            Get
                Return Start + mLength
            End Get
        End Property

        Public ReadOnly Property Color As UInteger
            Get
                Return mColor
            End Get
        End Property

        Public Sub SetNext(ByVal gap As GapNode)
            Debug.Assert(NextNode Is Nothing)
            Debug.Assert(gap IsNot Nothing)
            NextNode = gap
        End Sub

        Public Function Bump(ByVal boardView As IBoardView) As Boolean
            Debug.Assert(NextNode IsNot Nothing)
            Return MoveWorker(Start + 1, boardView)
        End Function

        Public Function Reset(ByVal boardView As IBoardView) As Boolean
            Debug.Assert(NextNode IsNot Nothing)
            Dim start As Integer = Math.Max(mMin, PrevNode.MinEnd)
            Return MoveWorker(start, boardView)
        End Function

        Public Function Verify(ByVal boardView As IBoardView) As Boolean
            For i As Integer = Start To [End] - 1
                Dim valid As Boolean = boardView(i).HasColor(mColor)
                If Not valid Then Return False
            Next

            Return True
        End Function

        Public Sub ExtractAssignment(ByVal colors As IList(Of UInteger))
            For i As Integer = Start To [End] - 1
                colors(i) = mColor
            Next

            NextNode.ExtractAssignment(colors)
        End Sub

        Private Function MoveWorker(ByVal minStartVal As Integer,
                                    ByVal boardView As IBoardView) As Boolean

            Start = minStartVal

            While [End] <= mMax

                If Verify(boardView) Then

                    If PrevNode.VerifyNextMoved(boardView) Then

                        If NextNode.Reset(boardView) Then
                            Return True
                        End If
                    End If
                End If

                Start += 1
            End While

            Return False
        End Function
    End Class

    Private Class GapNode
        Public Sub New(ByVal prev As ColorNode)
            PrevNode = prev
            NextNode = Nothing
            Start = If(PrevNode IsNot Nothing, PrevNode.[End], 0)
            [End] = Start
        End Sub

        Public Property PrevNode As ColorNode
        Public Property NextNode As ColorNode
        Public Property IsRequired As Boolean
        Public Property Start As Integer
        Public Property [End] As Integer

        Public ReadOnly Property MinEnd As Integer
            Get
                Return If(IsRequired, Start + 1, Start)
            End Get
        End Property

        Public ReadOnly Property Length As Integer
            Get
                Return [End] - Start
            End Get
        End Property

        Public Sub SetNext(ByVal _color As ColorNode)
            Debug.Assert(NextNode Is Nothing)
            Debug.Assert(Start = [End])
            Debug.Assert(_color IsNot Nothing)

            If PrevNode IsNot Nothing Then

                If PrevNode.Color = _color.Color Then
                    IsRequired = True
                End If
            End If

            NextNode = _color
            Debug.Assert(NextNode.Start >= MinEnd)
            [End] = NextNode.Start
        End Sub

        Public Sub MarkAsEnd(ByVal boardSize As Integer)
            Debug.Assert(NextNode Is Nothing)
            [End] = boardSize
        End Sub

        Public Function Reset(ByVal boardView As IBoardView) As Boolean
            Start = If(PrevNode IsNot Nothing, PrevNode.[End], 0)

            If NextNode Is Nothing Then
                Return Verify(boardView)
            Else
                [End] = MinEnd
            End If

            Return NextNode.Reset(boardView)
        End Function

        Public Function Verify(ByVal boardView As IBoardView) As Boolean
            For i As Integer = Start To [End] - 1
                Dim valid As Boolean = boardView(i).HasColor(ColorSpace.Empty)
                If Not valid Then Return False
            Next

            Return True
        End Function

        Public Function VerifyNextMoved(ByVal boardView As IBoardView) As Boolean
            Dim newEnd As Integer = NextNode.Start
            Dim newLength As Integer = newEnd - Start
            Dim minLength As Integer = If(IsRequired, 1, 0)
            If newLength < minLength Then Return False
            [End] = newEnd
            Return Verify(boardView)
        End Function

        Public Sub ExtractAssignment(ByVal colors As IList(Of UInteger))
            For i As Integer = Start To [End] - 1
                colors(i) = ColorSpace.Empty
            Next

            If NextNode IsNot Nothing Then
                NextNode.ExtractAssignment(colors)
            End If
        End Sub
    End Class
End Class