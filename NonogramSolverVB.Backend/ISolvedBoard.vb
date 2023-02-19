Imports System
Imports System.Collections.Generic
Imports System.Text

Public Interface ISolvedBoard
    Default ReadOnly Property Item(ByVal x As Integer, ByVal y As Integer) As UInteger
End Interface

Public Class SolvedBoard
        Implements ISolvedBoard

        Private ReadOnly colors As UInteger(,)

        Public Sub New(ByVal numRows As Integer, ByVal numCols As Integer)
            colors = New UInteger(numRows - 1, numCols - 1) {}
        End Sub

    Default Public Property Item(ByVal x As Integer, ByVal y As Integer) As UInteger Implements ISolvedBoard.Item
        Get
            Return colors(x, y)
        End Get
        Set(ByVal value As UInteger)
            colors(x, y) = value
        End Set
    End Property
End Class