Imports System
Imports System.Collections.Generic
Imports System.Diagnostics
Imports System.Linq
Imports System.Text
Imports System.Threading.Tasks
Imports System.Runtime.InteropServices

Public Class PriorityQueue(Of TKey, TValue)
    Private Class Node
        Public key As TKey
        Public value As TValue
    End Class

    Private ReadOnly tree As List(Of Node)
    Private ReadOnly keyComparer As IComparer(Of TKey)

    Private Enum HeapifyCheck
        None
        Left
        Right
    End Enum

    Public Sub New()
        tree = New List(Of Node)()
        keyComparer = Comparer(Of TKey).[Default]
    End Sub

    Public Sub New(ByVal _comparer As Comparison(Of TKey))
        tree = New List(Of Node)()
        keyComparer = Comparer(Of TKey).Create(_comparer)
    End Sub

    Public Sub New(ByVal _comparer As IComparer(Of TKey))
        tree = New List(Of Node)()
        keyComparer = _comparer
    End Sub

    Public ReadOnly Property Count As Integer
        Get
            Return tree.Count
        End Get
    End Property

    Public Sub Enqueue(ByVal _key As TKey, ByVal _value As TValue)
        Dim node As Node = New Node With {.key = _key, .value = _value}

        Dim index As Integer = tree.Count
        tree.Add(node)
        HeapifyUp(index)
    End Sub

    Public Function Dequeue() As TValue
        Return DequeueWorker().value
    End Function

    Public Function Dequeue(<Out> ByRef _key As TKey) As TValue
        Dim node As Node = DequeueWorker()
        _key = node.key
        Return node.value
    End Function

    Private Function DequeueWorker() As Node
        If tree.Count = 0 Then Throw New InvalidOperationException("Cannot Dequeue: PriorityQueue is empty.")
        Dim lastIndex As Integer = tree.Count - 1
        Swap(0, lastIndex)
        Dim minNode As Node = tree(lastIndex)
        tree.RemoveAt(lastIndex)
        HeapifyDown()
        Return minNode
    End Function

    Private Sub HeapifyDown()
        If tree.Count = 0 Then Return
        Dim current As Integer = 0
        Dim left, right As Integer

        While True
            left = GetLeftChild(current)
            right = GetRightChild(current)
            Dim checkResult As HeapifyCheck = CheckHeapifyDown(current, left, right)

            If checkResult = HeapifyCheck.None Then
                Exit While
            ElseIf checkResult = HeapifyCheck.Left Then
                Swap(current, left)
                current = left
            Else
                Debug.Assert(checkResult = HeapifyCheck.Right)
                Swap(current, right)
                current = right
            End If
        End While
    End Sub

    Private Sub HeapifyUp(ByVal _index As Integer)
        Debug.Assert(_index < tree.Count)
        Dim current As Integer = _index
        Dim parent As Integer

        While current <> 0
            parent = GetParent(current)
            Dim currentKey As TKey = tree(current).key
            Dim parentKey As TKey = tree(parent).key
            If keyComparer.Compare(currentKey, parentKey) >= 0 Then Exit While
            Swap(current, parent)
            current = parent
        End While
    End Sub

    Private Function GetLeftChild(ByVal _current As Integer) As Integer
        Return _current * 2 + 1
    End Function

    Private Function GetRightChild(ByVal _current As Integer) As Integer
        Return _current * 2 + 2
    End Function

    Private Function GetParent(ByVal _current As Integer) As Integer
        Return (_current - 1) / 2
    End Function

    Private Sub Swap(ByVal first As Integer, ByVal second As Integer)
        If first = second Then Return
        Dim a As Node = tree(first)
        tree(first) = tree(second)
        tree(second) = a
    End Sub

    Private Function CheckHeapifyDown(ByVal _current As Integer,
                                      ByVal left As Integer, ByVal right As Integer) As HeapifyCheck

        Dim currentKey As TKey = tree(_current).key
        Dim currentVsLeft As Boolean = False
        Dim currentVsRight As Boolean = False

        If left < tree.Count Then
            Dim lKey As TKey = tree(left).key
            currentVsLeft = keyComparer.Compare(currentKey, lKey) > 0
        End If

        If right < tree.Count Then
            Dim rKey As TKey = tree(right).key
            currentVsRight = keyComparer.Compare(currentKey, rKey) > 0
        End If

        If currentVsLeft AndAlso currentVsRight Then
            Dim lKey As TKey = tree(left).key
            Dim rKey As TKey = tree(right).key
            Return If(keyComparer.Compare(lKey, rKey) > 0, HeapifyCheck.Right, HeapifyCheck.Left)
        ElseIf currentVsLeft Then
            Return HeapifyCheck.Left
        ElseIf currentVsRight Then
            Return HeapifyCheck.Right
        Else
            Return HeapifyCheck.None
        End If
    End Function
End Class