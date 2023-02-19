Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Threading.Tasks
Imports NonogramSolverVB.Backend

<TestClass>
Class ConstraintSegmentTest
    Private Class SegmentDescription
        Public Sub New(ByVal start As Integer, ByVal [end] As Integer,
                       ByVal _color As UInteger, ByVal _number As UInteger,
                       ByVal _isGap As Boolean)

            StartIndex = start
            EndIndex = [end]
            Color = _color
            Number = _number
            IsGap = _isGap
        End Sub

        Public Property StartIndex As Integer
        Public Property EndIndex As Integer
        Public Property Color As UInteger
        Public Property Number As UInteger
        Public Property IsGap As Boolean

        Public Function Match(ByVal segment As ConstraintSegment) As Boolean
            Return segment.StartIndex = StartIndex AndAlso segment.EndIndex = EndIndex AndAlso segment.Constraint.color = Color AndAlso segment.Constraint.number = Number AndAlso segment.IsGap = IsGap
        End Function
    End Class

    <TestMethod>
    Public Sub Basic()
        Dim constraints As List(Of Constraint) = New List(Of Constraint) From {
            New Constraint(1, 1),
            New Constraint(2, 2),
            New Constraint(3, 3)
        }
        Dim begin As ConstraintSegment = Nothing, [end] As ConstraintSegment = Nothing
        ConstraintSegment.CreateChain(constraints, begin, [end])
        Dim startIndices As List(Of Integer) = New List(Of Integer) From {
            0,
            1,
            3
        }
        Dim endIndices As List(Of Integer) = New List(Of Integer) From {
            1,
            3,
            6
        }
        Dim index As Integer = 0
        Dim current As ConstraintSegment = begin

        While Not Object.ReferenceEquals(current, [end])
            Assert.[True](current.IsGap = False)
            Assert.[True](current.StartIndex = startIndices(index))
            Assert.[True](current.EndIndex = endIndices(index))
            Assert.[True](current.Constraint = constraints(index))
            index += 1
            current = current.[Next]
        End While
    End Sub

    <TestMethod>
    Public Sub Gaps()
        Dim constraints As List(Of Constraint) = New List(Of Constraint) From {
            New Constraint(1, 2),
            New Constraint(1, 1),
            New Constraint(1, 2)
        }
        Dim descriptions As List(Of SegmentDescription) = New List(Of SegmentDescription) From {
            New SegmentDescription(0, 2, 1, 2, False),
            New SegmentDescription(2, 3, 0, 1, True),
            New SegmentDescription(3, 4, 1, 1, False),
            New SegmentDescription(4, 5, 0, 1, True),
            New SegmentDescription(5, 7, 1, 2, False)
        }
        AssertMatches(constraints, descriptions)
    End Sub

    <TestMethod>
    Public Sub Mixed()
        Dim constraints As List(Of Constraint) = New List(Of Constraint) From {
            New Constraint(1, 1),
            New Constraint(1, 1),
            New Constraint(2, 2),
            New Constraint(1, 3),
            New Constraint(3, 1)
        }
        Dim descriptions As List(Of SegmentDescription) = New List(Of SegmentDescription) From {
            New SegmentDescription(0, 1, 1, 1, False),
            New SegmentDescription(1, 2, 0, 1, True),
            New SegmentDescription(2, 3, 1, 1, False),
            New SegmentDescription(3, 5, 2, 2, False),
            New SegmentDescription(5, 8, 1, 3, False),
            New SegmentDescription(8, 9, 3, 1, False)
        }
        AssertMatches(constraints, descriptions)
    End Sub

    <TestMethod>
    Public Sub SimpleBump()
        Dim constraints As List(Of Constraint) = New List(Of Constraint) From {
            New Constraint(1, 1),
            New Constraint(2, 1)
        }
        Dim descriptions As List(Of SegmentDescription) = New List(Of SegmentDescription) From {
            New SegmentDescription(0, 1, 1, 1, False),
            New SegmentDescription(1, 2, 0, 1, True),
            New SegmentDescription(2, 3, 2, 1, False)
        }
        Dim begin As ConstraintSegment = Nothing, [end] As ConstraintSegment = Nothing
        ConstraintSegment.CreateChain(constraints, begin, [end])
        Dim success As Boolean = [end].Bump(7)
        Assert.[True](success)
        AssertMatches(begin, descriptions)
    End Sub

    <TestMethod>
    Public Sub GappedBump()
        Dim constraints As List(Of Constraint) = New List(Of Constraint) From {
            New Constraint(1, 1),
            New Constraint(1, 1)
        }
        Dim descriptions1 As List(Of SegmentDescription) = New List(Of SegmentDescription) From {
            New SegmentDescription(0, 1, 1, 1, False),
            New SegmentDescription(1, 3, 0, 2, True),
            New SegmentDescription(3, 4, 1, 1, False)
        }
        Dim begin As ConstraintSegment = Nothing, [end] As ConstraintSegment = Nothing
        ConstraintSegment.CreateChain(constraints, begin, [end])
        Dim success As Boolean = [end].Bump(4)
        Assert.[True](success)
        AssertMatches(begin, descriptions1)
    End Sub

    <TestMethod>
    Public Sub GappedBump2()
        Dim constraints As List(Of Constraint) = New List(Of Constraint) From {
            New Constraint(1, 1),
            New Constraint(1, 1)
        }
        Dim descriptions2 As List(Of SegmentDescription) = New List(Of SegmentDescription) From {
            New SegmentDescription(1, 2, 1, 1, False),
            New SegmentDescription(2, 3, 0, 1, True),
            New SegmentDescription(3, 4, 1, 1, False)
        }
        Dim begin As ConstraintSegment = Nothing, [end] As ConstraintSegment = Nothing
        ConstraintSegment.CreateChain(constraints, begin, [end])
        [end].Bump(4)
        Dim success As Boolean = [end].Bump(4)
        Assert.[True](success)
        AssertMatches(begin, descriptions2)
        success = [end].Bump(4)
        Assert.[False](success)
    End Sub

    Private Sub AssertMatches(ByVal constraints As IEnumerable(Of Constraint), ByVal segments As IEnumerable(Of SegmentDescription))
        Dim begin As ConstraintSegment = Nothing, [end] As ConstraintSegment = Nothing
        ConstraintSegment.CreateChain(constraints, begin, [end])
        AssertMatches(begin, segments)
    End Sub

    Private Sub AssertMatches(ByVal begin As ConstraintSegment, ByVal segments As IEnumerable(Of SegmentDescription))
        Dim segmentSequence = ExtractChainSequence(begin)
        Dim matches = segmentSequence.Zip(segments, Function(c, d) d.Match(c))
        Assert.[True](matches.All(Function(m) m))
    End Sub

    Private Iterator Function ExtractChainSequence(ByVal begin As ConstraintSegment) As IEnumerable(Of ConstraintSegment)
        Dim current As ConstraintSegment = begin

        While current IsNot Nothing
            Yield current
            current = current.[Next]
        End While
    End Function
End Class