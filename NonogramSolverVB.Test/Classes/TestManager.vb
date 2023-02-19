Imports System
Imports System.Collections.Generic
Imports System.Diagnostics
Imports System.Linq
Imports System.Reflection
Imports System.Text
Imports System.Threading.Tasks

Class TestManager
    Public Sub New()

    End Sub

    Public Sub FindAllTests()
        My.Application.Log.WriteEntry("Searching for tests...")
        BuildTestGrid()
        Dim numTests As Integer = GetTestCount()
        My.Application.Log.WriteEntry($"Found {numTests} tests.")
    End Sub

    Public Sub RunAllTests(ByVal stats As TestStatistics)
        My.Application.Log.WriteEntry("Beginning test run")

        For Each testClass In m_testGrid
            RunTestClass(testClass, stats)
        Next

        My.Application.Log.WriteEntry("End of test run")
    End Sub

    Private Sub BuildTestGrid()
        Dim testClasses = FindAllTestsClasses()
        m_testGrid = testClasses.[Select](Function(tc) CreateTestData(tc)).ToList()
    End Sub

    Private Function GetTestCount() As Integer
        Return m_testGrid.Sum(Function(data) data.Methods.Count)
    End Function

    Private Shared Function FindAllTestsClasses() As IEnumerable(Of TypeInfo)
        Dim currentAssembly = GetType(TestManager).GetTypeInfo().Assembly

        'Dim lst As IEnumerable(Of TypeInfo) = currentAssembly.DefinedTypes.Where(
        '    Function(t)
        '        Return t.IsClass AndAlso Not t.IsAbstract
        '    End Function).Where(
        '    Function(t)
        '        Return t.IsDefined(GetType(TestClassAttribute), True)
        '    End Function)

        Return currentAssembly.DefinedTypes.Where(
            Function(t)
                Return t.IsClass AndAlso Not t.IsAbstract
            End Function).Where(
            Function(t)
                Return t.IsDefined(GetType(TestClassAttribute), True)
            End Function)
    End Function

    Private Shared Function FindAllTestMethods(ByVal typeInfo As TypeInfo) As IEnumerable(Of MethodInfo)
        Return typeInfo.DeclaredMethods.Where(Function(t) t.IsDefined(GetType(TestMethodAttribute), True))
    End Function

    Private Shared Function CreateTestData(ByVal typeInfo As TypeInfo) As TestClassData
        Dim testMethods = FindAllTestMethods(typeInfo).ToList()
        Dim disposeMethod As MethodInfo = Nothing

        If GetType(IDisposable).IsAssignableFrom(typeInfo.AsType()) Then
            disposeMethod = typeInfo.GetDeclaredMethod("Dispose")
        End If

        Return New TestClassData(typeInfo.AsType(), testMethods, disposeMethod)
    End Function

    Private Shared Sub RunTestClass(ByVal testData As TestClassData, ByVal stats As TestStatistics)
        Dim instance As Object = Activator.CreateInstance(testData.Type)

        For Each testMethod In testData.Methods
            InvokeTest(instance, testMethod, stats)
        Next

        testData.DisposeMethod?.Invoke(instance, Nothing)
    End Sub

    Private Shared Sub InvokeTest(ByVal instance As Object, ByVal method As MethodInfo, ByVal stats As TestStatistics)
        Dim testName As String = $"{method.DeclaringType.Name}.{method.Name}"
        My.Application.Log.WriteEntry("")
        My.Application.Log.WriteEntry($"-- Running test {testName}:")
        Dim failure As Exception = Nothing

        'Try
        method.Invoke(instance, Nothing)
        'Catch ex As Exception
        'failure = ex
        'End Try

        If failure Is Nothing Then
            My.Application.Log.WriteEntry("-- Test Passed --")
            stats.AddTestResult(True, testName)
        Else
            My.Application.Log.WriteEntry("!! Test Failed !!")
            My.Application.Log.WriteEntry($"An exception of type {failure.[GetType]().FullName} was thrown: {failure.Message}")
            My.Application.Log.WriteEntry(failure.StackTrace)

            stats.AddTestResult(False, testName)
        End If
    End Sub

    Private Class TestClassData
        Public Sub New(ByVal testClass As Type, ByVal testMethods As List(Of MethodInfo), ByVal disposeMethod As MethodInfo)
            Type = testClass
            Methods = testMethods
            disposeMethod = disposeMethod
        End Sub

        Public Property Type As Type
        Public Property Methods As List(Of MethodInfo)
        Public Property DisposeMethod As MethodInfo
    End Class

    Private m_testGrid As List(Of TestClassData)
End Class