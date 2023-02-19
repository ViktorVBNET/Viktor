Imports NonogramSolverVB.Backend
Imports NonogramSolverVB.Test

Interface IConstraintHelper
    Function ToSet() As IConstraintSet
End Interface
Public Class ConstraintHelper
    Implements IConstraintHelper

    Public ReadOnly constraint As Constraint

    Public Sub New(ByVal color As UInteger)
        constraint = New Constraint(color, 1)
    End Sub

    Public Function ToSet() As IConstraintSet Implements IConstraintHelper.ToSet
        Return BoardFactory.CreateConstraintSet({constraint})
    End Function

    Private Sub New(ByVal color As UInteger, ByVal number As UInteger)
        constraint = New Constraint(color, number)
    End Sub

    Public Shared Operator *(val As UInteger, helper As ConstraintHelper) As ConstraintHelper
        Return New ConstraintHelper(helper.constraint.color, helper.constraint.number * val)
    End Operator

    Public Shared Operator *(helper As ConstraintHelper, val As UInteger) As ConstraintHelper
        Return val * helper
    End Operator

    Public Shared Operator +(h1 As ConstraintHelper, h2 As ConstraintHelper) As ConstraintHelperList
        Return New ConstraintHelperList(New List(Of Constraint) From {h1.constraint, h2.constraint})
    End Operator

    Class ConstraintHelperList
        Implements IConstraintHelper
        Private ReadOnly mConstraints As IReadOnlyList(Of Constraint)
        Public Sub New(constraints As IReadOnlyList(Of Constraint))
            Me.mConstraints = constraints
        End Sub

        Public Function ToSet() As IConstraintSet Implements IConstraintHelper.ToSet
            Return BoardFactory.CreateConstraintSet(mConstraints)
        End Function

        Public Shared Operator +(list As ConstraintHelperList, helper As ConstraintHelper) As ConstraintHelperList
            Dim clone As List(Of Constraint) = list.mConstraints.ToList()
            clone.Add(helper.constraint)
            Return New ConstraintHelperList(clone)
        End Operator

        Public Shared Operator +(helper As ConstraintHelper, list As ConstraintHelperList) As ConstraintHelperList
            Return (list + helper)
        End Operator

        Public Shared Operator +(l1 As ConstraintHelperList, l2 As ConstraintHelperList) As ConstraintHelperList
            Dim clone As List(Of Constraint) = l1.mConstraints.ToList()
            clone.AddRange(l2.mConstraints)
            Return New ConstraintHelperList(clone)
        End Operator
    End Class
End Class
