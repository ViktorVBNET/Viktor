Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Threading.Tasks
Imports NonogramSolverVB.Backend

<TestClass>
Class PriorityQueueTest
    Private Class TestObj
        Public Property Value As Integer
    End Class

    Private Sub Push(ByVal queue As PriorityQueue(Of Integer, TestObj), ByVal val As Integer)
        queue.Enqueue(val, New TestObj() With {.Value = val})
    End Sub

    Private Sub Verify(ByVal queue As PriorityQueue(Of Integer, TestObj), ByVal correct As Integer)
        Dim obj = queue.Dequeue()
        Assert.[True](obj.Value = correct)
    End Sub

    Private Sub VerifyAll(ByVal queue As PriorityQueue(Of Integer, TestObj), ByVal answers As ICollection(Of Integer))
        Assert.[True](queue.Count = answers.Count)

        For Each correct As Integer In answers
            Verify(queue, correct)
        Next

        Assert.[True](queue.Count = 0)
    End Sub

    Private Sub VerifyFull(ParamArray values As Integer())
        Dim queue As PriorityQueue(Of Integer, TestObj) = New PriorityQueue(Of Integer, TestObj)()

        For Each val As Integer In values
            Push(queue, val)
        Next

        Assert.[True](queue.Count = values.Length)
        Array.Sort(values)
        VerifyAll(queue, values)
    End Sub

    <TestMethod>
    Public Sub Basic()
        VerifyFull(1, 3, 5, 2, 4)
    End Sub

    <TestMethod>
    Public Sub Basic2()
        VerifyFull(0, 2, 4, 6, 8)
    End Sub

    <TestMethod>
    Public Sub BasicEnqueueDequeue()
        Dim queue As PriorityQueue(Of Integer, TestObj) = New PriorityQueue(Of Integer, TestObj)()
        Assert.[True](queue.Count = 0)
        queue.Enqueue(1, New TestObj() With {.Value = 2})
        Assert.[True](queue.Count = 1)
        Dim key As Integer = Nothing
        Dim obj = queue.Dequeue(key)
        Assert.[True](obj.Value = 2)
        Assert.[True](key = 1)
        Assert.[True](queue.Count = 0)
    End Sub

    <TestMethod>
    Public Sub ThrowOnEmpty()
        Dim queue As PriorityQueue(Of Integer, TestObj) = New PriorityQueue(Of Integer, TestObj)()
        Push(queue, 1)
        Push(queue, -1)
        queue.Dequeue()
        queue.Dequeue()
        Dim success As Boolean = False

        Try
            queue.Dequeue()
        Catch ex As Exception
            success = True
        End Try

        Assert.[True](success)
    End Sub

    <TestMethod>
    Public Sub Complex()
        Dim queue As PriorityQueue(Of Integer, TestObj) = New PriorityQueue(Of Integer, TestObj)()
        Push(queue, 3)
        Push(queue, 0)
        Push(queue, 1)
        Push(queue, 4)
        Verify(queue, 0)
        Verify(queue, 1)
        Push(queue, 7)
        Push(queue, 2)
        Push(queue, 5)
        Verify(queue, 2)
        Verify(queue, 3)
        Push(queue, 6)
        VerifyAll(queue, {4, 5, 6, 7})
    End Sub

    <TestMethod>
    Public Sub Reversed()
        Dim queue As PriorityQueue(Of Integer, TestObj) = New PriorityQueue(Of Integer, TestObj)(Function(a, b) b - a)
        Push(queue, 17)
        Push(queue, 5)
        Push(queue, 9)
        Push(queue, 1)
        Push(queue, 2)
        Push(queue, 6)
        VerifyAll(queue, {17, 9, 6, 5, 2, 1})
    End Sub

    <TestMethod>
    Public Sub DoubleTake()
        Dim queue As PriorityQueue(Of Integer, TestObj) = New PriorityQueue(Of Integer, TestObj)()
        Dim correct = {0, 1, 2, 3, 4, 5}
        Push(queue, 0)
        Push(queue, 3)
        Push(queue, 2)
        Push(queue, 1)
        Push(queue, 4)
        Push(queue, 5)
        VerifyAll(queue, correct)
        Push(queue, 5)
        Push(queue, 4)
        Push(queue, 3)
        Push(queue, 2)
        Push(queue, 1)
        Push(queue, 0)
        VerifyAll(queue, correct)
    End Sub
End Class