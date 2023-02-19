Imports System.Collections.Generic
Imports System.Diagnostics
Imports System.Linq
Imports System.Text
Imports System.Threading.Tasks

<TestClass>
Class DummyTest
    Implements IDisposable
    Public Sub Dispose() Implements IDisposable.Dispose
        My.Application.Log.WriteEntry("And dispose works too :D")
    End Sub

    <TestMethod>
    Public Sub MyFirstTest()
        My.Application.Log.WriteEntry("Yay it actually ran!")
    End Sub
End Class