Imports System.IO

Public Class frmMain
    ' Used across multiple threads
    Private testManager As TestManager

    Public Sub New()
        ' Этот вызов является обязательным для конструктора.
        InitializeComponent()

        ' Добавить код инициализации после вызова InitializeComponent().
        testManager = New TestManager()
    End Sub

    Private Sub cmdTest_Click(sender As Object, e As EventArgs) Handles cmdTest.Click
        'C:\Users\...\AppData\Roaming\NonogramSolverVB\NonogramSolverVB.Test\1.0.0.0\NonogramSolverVB.Test.log
        My.Application.Log.DefaultFileLogWriter.Append = False

        Dim t As New TimeSpan(Now.Ticks)
        My.Application.Log.WriteEntry(t.Ticks)

        Test()

        Dim mPathBase As String = My.Application.Log.DefaultFileLogWriter.FullLogFileName
        Dim mPathNew As String = Path.Combine(Path.GetDirectoryName(mPathBase), "Logger.txt")
        My.Application.Log.DefaultFileLogWriter.Close()

        File.Copy(mPathBase, mPathNew, True)
        Application.DoEvents()

        rtbDebug.Text = ""
        Dim start As Boolean = False
        Using strm As New StreamReader(File.Open(mPathNew, FileMode.Open, FileAccess.Read))
            Do While strm.Peek() >= 0
                Dim line As String = strm.ReadLine
                If start Then
                    rtbDebug.AppendText(line & vbCrLf)
                Else
                    If line.Trim.EndsWith(t.Ticks.ToString) Then
                        start = True
                        rtbDebug.AppendText(line & vbCrLf)
                    End If
                End If
            Loop
        End Using
    End Sub

    'Async
    Private Sub Test()
        rtbDebug.Clear()

        Dim sw As New Stopwatch
        sw.Start()

        Dim stats As New TestStatistics()
        lblStatus.Text = "Finding tests..."
        Application.DoEvents()
        testManager.FindAllTests()
        Application.DoEvents()
        'Await Task.Run(Sub()
        '                   testManager.FindAllTests()
        '               End Sub)
        lblStatus.Text = "Running all tests..."
        Application.DoEvents()
        testManager.RunAllTests(stats)
        Application.DoEvents()
        'Await Task.Run(Sub()
        '                   testManager.RunAllTests(stats)
        '               End Sub)

        sw.Stop()
        Dim ts As TimeSpan = sw.Elapsed

        stats.PrintSummary()
        lblStatus.Text = $"Finished in {Math.Round(ts.TotalSeconds, 2).ToString} sec"
        lblSummary.Text = $"{stats.NumberTestsPassed}/{stats.NumberTestsRun} tests passed ({stats.PercentPassed}%)"

        failuresList.Items.Clear()
        For Each s As String In stats.FailedTests
            failuresList.Items.Add(s)
        Next

        'failuresList.ItemsSource = stats.FailedTests
    End Sub
End Class
