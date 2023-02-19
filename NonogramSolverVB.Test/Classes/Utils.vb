Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Reflection
Imports System.Text
Imports System.Threading.Tasks

Friend Module Utils
    Function GetTypesWithAttribute(Of T As {Attribute, New})(ByVal assembly As Assembly) As IEnumerable(Of TypeInfo)
        Return GetTypesWithAttributeInternal(GetType(T), assembly)
    End Function

    Function GetTypesWithAttribute(ByVal attributeType As Type, ByVal assembly As Assembly) As IEnumerable(Of TypeInfo)
        If attributeType Is Nothing Then Throw New ArgumentNullException()
        If attributeType.IsAssignableFrom(GetType(Attribute)) Then Throw New ArgumentException($"The given type {attributeType.FullName} must be an attribute")
        Return GetTypesWithAttributeInternal(attributeType, assembly)
    End Function

    Private Function GetTypesWithAttributeInternal(ByVal attributeType As Type, ByVal assembly As Assembly) As IEnumerable(Of TypeInfo)
        Dim attributeUsage = attributeType.GetTypeInfo().GetCustomAttribute(Of AttributeUsageAttribute)(inherit:=True)

        If attributeUsage IsNot Nothing Then

            If Not attributeUsage.ValidOn.HasFlag(AttributeTargets.[Class]) Then
                Return Enumerable.Empty(Of TypeInfo)()
            End If
        End If

        Dim searchableTypes = assembly.DefinedTypes.Where(Function(t) t.IsClass)
        Return searchableTypes.Where(Function(t) t.IsDefined(attributeType))
    End Function
End Module