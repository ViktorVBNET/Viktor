Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Threading.Tasks

Public Interface IConstraintSet
    Inherits IReadOnlyList(Of Constraint)
End Interface

Public Class ConstraintSet
    Implements IConstraintSet

    Private ReadOnly constraints As List(Of Constraint)

    Public Sub New(ByVal constraints As IEnumerable(Of Constraint))
        Me.constraints = constraints.ToList()
    End Sub

    Default Public ReadOnly Property Item(ByVal index As Integer) As Constraint Implements IConstraintSet.Item
        Get
            Return constraints(index)
        End Get
    End Property

    Public ReadOnly Property Count As Integer Implements IConstraintSet.Count
        Get
            Return constraints.Count
        End Get
    End Property

    Public Function GetEnumerator() As IEnumerator(Of Constraint) Implements IEnumerable(Of Constraint).GetEnumerator
        Return constraints.GetEnumerator()
    End Function

    Private Function IEnumerable_GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
        Return constraints.GetEnumerator()
    End Function
End Class