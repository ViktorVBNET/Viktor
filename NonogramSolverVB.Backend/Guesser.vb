Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Threading.Tasks

Class Guesser
    Private mRow As Integer
    Private mCol As Integer
    Private mColor As UInteger
    Private ReadOnly mBoard As Board

    Public Class Guess
        Public Sub New(ByVal _x As Integer, ByVal _y As Integer, ByVal _color As UInteger)
            X = _x
            Y = _y
            Color = _color
        End Sub

        Public ReadOnly X As Integer
        Public ReadOnly Y As Integer
        Public ReadOnly Color As UInteger
    End Class

    Public Sub New(ByVal _board As Board)
        Me.mBoard = _board
        Reset()
    End Sub

    Public Function GenerateNext(ByVal _state As BoardState) As Guess
        Dim guess As Guess = Nothing

        While Not IsDone()

            If IsCompatible(_state) Then
                guess = New Guess(mRow, mCol, mColor)
                Increment()
                Exit While
            End If

            Increment()
        End While

        Return guess
    End Function

    Public Sub Reset()
        mRow = 0
        mCol = 0
        mColor = 0
    End Sub

    Private Sub Increment()
        mColor += 1

        If mColor = mBoard.ColorSpace.MaxColor Then
            mColor = 0
            mCol += 1

            If mCol = mBoard.NumColumns Then
                mCol = 0
                mRow += 1
            End If
        End If
    End Sub

    Private Function IsDone() As Boolean
        Return mRow >= mBoard.NumRows
    End Function

    Private Function IsCompatible(ByVal state As BoardState) As Boolean
        Dim val As ColorSet = state(mRow, mCol)
        Return val.HasColor(mColor) AndAlso Not val.IsSingle()
    End Function
End Class