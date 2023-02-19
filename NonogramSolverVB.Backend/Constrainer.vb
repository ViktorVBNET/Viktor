Imports System
Imports System.Collections.Generic
Imports System.Diagnostics
Imports System.Linq
Imports System.Text
Imports System.Threading.Tasks

Class ConstraintState
    Public Sub New(ByVal boardSize As Integer, ByVal numConstraints As Integer)
        minValues = Enumerable.Repeat(0, numConstraints).ToArray()
        maxValues = Enumerable.Repeat(boardSize - 1, numConstraints).ToArray()
        CalculateCost()
    End Sub

    Private Sub New(ByVal mins As Integer(), ByVal maxs As Integer())
        minValues = CType(mins.Clone(), Integer())
        maxValues = CType(maxs.Clone(), Integer())
        CalculateCost()
    End Sub

    Public ReadOnly minValues As Integer()
    Public ReadOnly maxValues As Integer()
    Public Cost As Long

    Public Function Clone() As ConstraintState
        Return New ConstraintState(minValues, maxValues)
    End Function

    Public Sub CalculateCost()
        Dim _cost As Long = 1

        For i As Integer = 0 To minValues.Length - 1
            _cost *= (maxValues(i) - minValues(i)) + 1
        Next

        Cost = _cost
        Debug.Assert(cost >= 0)
    End Sub
End Class

Friend Class Constrainer
    Implements IConstraintList

    Private ReadOnly constraintSet As IConstraintSet

    Public Sub New(ByVal _constraints As IConstraintSet)
        constraintSet = _constraints
    End Sub

    Public Sub CalculateEstimatedCost(ByVal _boardView As IBoardView) Implements IConstraintList.CalculateEstimatedCost
        CalculateMinMaxRanges(_boardView)
        _boardView.ConstraintState.CalculateCost()
    End Sub

    Public Function ConstrainBoard(ByVal _boardView As IBoardView) As ConstrainResult Implements IConstraintList.ConstrainBoard
        Dim result = VerifyValid(_boardView)
        If result = ConstrainResult.NoSolution Then Return ConstrainResult.NoSolution
        Dim generator As AssignmentGenerator = New AssignmentGenerator(_boardView, constraintSet)
        Dim colorSets As ColorSet() = New ColorSet(_boardView.Count - 1) {}

        While generator.MoveNext()
            Dim assignment = generator.GetAssignment()
            Merge(assignment, colorSets)
        End While

        Return _boardView.IntersectAll(colorSets)
    End Function

    Private Sub CalculateMinMaxRanges(ByVal boardView As IBoardView)
        Dim state As ConstraintState = boardView.ConstraintState
        AdjustInitialMins(state)
        AdjustInitialMaxs(state)
        CalculateMin(boardView, state)
        CalculateMax(boardView, state)
        AdjustInitialMins(state)
        AdjustInitialMaxs(state)
    End Sub

    Private Sub AdjustInitialMins(ByVal state As ConstraintState)
        Debug.Assert(state.minValues.Length > 0)
        Dim prevMin As Integer = state.minValues(0)

        For i As Integer = 1 To state.minValues.Length - 1
            Dim forcedGapOffset As Integer = If(constraintSet(i - 1).color = constraintSet(i).color, 1, 0)
            Dim lowMargin As Long = prevMin + constraintSet(i - 1).number + forcedGapOffset

            If state.minValues(i) < lowMargin Then
                state.minValues(i) = CInt(lowMargin)
            End If

            prevMin = state.minValues(i)
        Next
    End Sub

    Private Sub AdjustInitialMaxs(ByVal state As ConstraintState)
        Debug.Assert(state.maxValues.Length > 0)
        Dim lastIndex As Integer = state.maxValues.Length - 1
        Dim prevMax As Integer = state.maxValues(lastIndex)

        For i As Integer = lastIndex - 1 To 0 Step -1
            Dim forcedGapOffset As Integer = If(constraintSet(i + 1).color = constraintSet(i).color, 1, 0)
            Dim highMargin As Long = prevMax - constraintSet(i + 1).number - forcedGapOffset

            If state.maxValues(i) > highMargin Then
                state.maxValues(i) = CInt(highMargin)
            End If

            prevMax = state.maxValues(i)
        Next
    End Sub

    Private Sub CalculateMin(ByVal boardView As IBoardView, ByVal state As ConstraintState)
        For constrIndex As Integer = 0 To state.minValues.Length - 1
            Dim constraint As Constraint = constraintSet(constrIndex)
            Dim max As Integer = state.maxValues(constrIndex)
            Dim startPoint As Integer = state.minValues(constrIndex)

            For boardIndex As Integer = startPoint To max

                If Not boardView(boardIndex).HasColor(constraint.color) Then
                    startPoint = boardIndex + 1
                ElseIf boardIndex - startPoint + 1 >= constraint.number Then
                    Exit For
                End If
            Next

            state.minValues(constrIndex) = startPoint
        Next
    End Sub

    Private Sub CalculateMax(ByVal boardView As IBoardView, ByVal state As ConstraintState)
        For constrIndex As Integer = state.minValues.Length - 1 To 0 Step -1
            Dim constraint As Constraint = constraintSet(constrIndex)
            Dim min As Integer = state.minValues(constrIndex)
            Dim startPoint As Integer = state.maxValues(constrIndex)

            For boardIndex As Integer = startPoint To min Step -1

                If Not boardView(boardIndex).HasColor(constraint.color) Then
                    startPoint = boardIndex - 1
                ElseIf startPoint - boardIndex + 1 >= constraint.number Then
                    Exit For
                End If
            Next

            state.maxValues(constrIndex) = startPoint
        Next
    End Sub

    Private Function Emit(ByVal boardView As IBoardView, ByVal state As ConstraintState) As ConstrainResult
        Dim colors As ColorSet() = New ColorSet(boardView.Count - 1) {}

        For boardIndex As Integer = 0 To boardView.Count - 1
            colors(boardIndex) = ColorSet.CreateSingle(ColorSpace.Empty)

            For constraintIndex As Integer = 0 To state.minValues.Length - 1
                Dim min As Integer = state.minValues(constraintIndex)
                Dim max As Integer = state.maxValues(constraintIndex)

                If boardIndex >= min AndAlso boardIndex <= max Then
                    Dim constraint As Constraint = constraintSet(constraintIndex)
                    Dim offset As Long = (max - min + 1) - constraint.number

                    If boardIndex >= min + offset AndAlso boardIndex <= max - offset Then
                        colors(boardIndex) = ColorSet.CreateSingle(constraint.color)
                        Exit For
                    End If

                    colors(boardIndex) = colors(boardIndex).AddColor(constraint.color)
                End If
            Next
        Next

        Return boardView.IntersectAll(colors)
    End Function

    Private Function VerifyValid(ByVal boardView As IBoardView) As ConstrainResult
        Dim constraintIndex As Integer = 0
        Dim colorCount As Integer = 0

        For i As Integer = 0 To boardView.Count - 1
            Dim color As ColorSet = boardView(i)
            If Not color.IsSingle() Then Return ConstrainResult.Success
            Dim currentColor As UInteger = color.GetSingleColor()

            If currentColor = 0 Then

                If colorCount <> 0 Then
                    Return ConstrainResult.NoSolution
                End If
            Else
                If constraintIndex >= constraintSet.Count Then Return ConstrainResult.NoSolution

                If currentColor = constraintSet(constraintIndex).color Then
                    colorCount += 1

                    If colorCount = constraintSet(constraintIndex).number Then
                        constraintIndex += 1
                        colorCount = 0
                    End If
                Else
                    Return ConstrainResult.NoSolution
                End If
            End If
        Next

        Return ConstrainResult.Success
    End Function

    Private Shared Sub Merge(ByVal from As IReadOnlyList(Of UInteger), ByVal into As ColorSet())
        Debug.Assert(from.Count = into.Length)

        For i As Integer = 0 To from.Count - 1
            into(i) = into(i).AddColor(from(i))
        Next
    End Sub
End Class