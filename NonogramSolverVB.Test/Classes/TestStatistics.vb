Imports System.Collections.Generic
Imports System.Diagnostics
Imports System.Linq
Imports System.Text
Imports System.Threading.Tasks

Class TestStatistics
    Public Sub New()
        numRun = 0
        numPass = 0
        failureNames = New List(Of String)()
    End Sub

    Public ReadOnly Property NumberTestsRun() As Integer
        Get
            Return numRun
        End Get
    End Property
    Public ReadOnly Property NumberTestsPassed() As Integer
        Get
            Return numPass
        End Get
    End Property
    Public ReadOnly Property PercentPassed() As Single
        Get
            Return (100 * CSng(numPass) / numRun)
        End Get
    End Property
    Public ReadOnly Property NumTestsFailed() As Integer
        Get
            Return numRun - numPass
        End Get
    End Property
    Public ReadOnly Property FailedTests() As IReadOnlyList(Of String)
        Get
            Return failureNames
        End Get
    End Property

    Public Sub AddTestResult(succeeded As Boolean, name As String)
        numRun += 1
        If succeeded Then
            numPass += 1
        Else
            failureNames.Add(name)
        End If
    End Sub

    Public Sub PrintSummary()
        My.Application.Log.WriteEntry("Summary:")
        My.Application.Log.WriteEntry($"{numPass}/{numRun} ({PercentPassed}%) succeeded")

        If failureNames.Count > 0 Then
            My.Application.Log.WriteEntry("Failed Tests:")

            For Each failure As String In failureNames
                My.Application.Log.WriteEntry("!!  {failure}")
            Next
        End If
    End Sub

    Private numRun As Integer
    Private numPass As Integer
    Private failureNames As List(Of String)
End Class