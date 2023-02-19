Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Threading.Tasks

<AttributeUsage(AttributeTargets.[Class], Inherited:=False, AllowMultiple:=False)>
Public NotInheritable Class TestClassAttribute
    Inherits Attribute
    Public Sub New()
    End Sub
End Class

<AttributeUsage(AttributeTargets.Method, Inherited:=False, AllowMultiple:=False)>
Public NotInheritable Class TestMethodAttribute
    Inherits Attribute
    Public Sub New()
    End Sub
End Class