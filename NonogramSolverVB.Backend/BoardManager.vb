Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.Diagnostics
Imports System.Linq
Imports System.Text

Class BoardManager
    Private mCurrentLayer As BoardState
    Private ReadOnly mPreviousStates As Stack(Of BoardState)

    Public Sub New(ByVal owner As Board)
        mCurrentLayer = New BoardState(owner)
        mPreviousStates = New Stack(Of BoardState)()
    End Sub

    Public ReadOnly Property CurrentLayer As BoardState
        Get
            Return mCurrentLayer
        End Get
    End Property

    Public Sub PushLayer()
        Dim newState As BoardState = mCurrentLayer.CreateNewLayer()
        mPreviousStates.Push(mCurrentLayer)
        mCurrentLayer = newState
    End Sub

    Public Function PopLayer() As Boolean
        If mPreviousStates.Count = 0 Then Return False
        mCurrentLayer = mPreviousStates.Pop()
        Return True
    End Function
End Class