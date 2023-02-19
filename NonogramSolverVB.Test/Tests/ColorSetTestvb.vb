Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Threading.Tasks
Imports NonogramSolverVB.Backend

<TestClass>
Class ColorSetTest
    <TestMethod>
    Public Sub Basic()
        Dim colorSet As ColorSet = New ColorSet()
        Assert.[True](colorSet.IsEmpty)
        Assert.[True](Not colorSet.HasColor(0))
    End Sub

    <TestMethod>
    Public Sub HasColor()
        Dim colorSet As ColorSet = New ColorSet(3)
        Assert.[True](colorSet.HasColor(0))
        Assert.[True](colorSet.HasColor(1))
        Assert.[True](Not colorSet.HasColor(2))
        Assert.[True](Not colorSet.HasColor(3))
    End Sub

    <TestMethod>
    Public Sub HasColor2()
        Dim colorSet As ColorSet = New ColorSet(5)
        Assert.[True](colorSet.HasColor(0))
        Assert.[True](Not colorSet.HasColor(1))
        Assert.[True](colorSet.HasColor(2))
        Assert.[True](Not colorSet.HasColor(3))
    End Sub

    <TestMethod>
    Public Sub AddColor()
        Dim colorSet As ColorSet = New ColorSet(6)
        Assert.[True](Not colorSet.HasColor(0))
        Dim newColorSet As ColorSet = colorSet.AddColor(0)
        Assert.[True](newColorSet.HasColor(0))
        Assert.[True](Not colorSet.HasColor(0))
    End Sub

    <TestMethod>
    Public Sub AddColor2()
        Dim colorSet As ColorSet = New ColorSet(4)
        Dim cs2 As ColorSet = colorSet.AddColor(0)
        Dim cs3 As ColorSet = cs2.AddColor(3)
        Dim final As ColorSet = New ColorSet(1 + 4 + 8)
        Assert.[True](colorSet <> final)
        Assert.[True](cs2 <> final)
        Assert.[True](cs3 = final)
    End Sub

    <TestMethod>
    Public Sub CreateFull()
        Dim full As ColorSet = ColorSet.CreateFullColorSet(2)
        Dim correct As ColorSet = New ColorSet(7)
        Assert.[True](full = correct)
    End Sub

    <TestMethod>
    Public Sub CreateFull2()
        Dim full As ColorSet = ColorSet.CreateFullColorSet(4)
        Dim correct As ColorSet = New ColorSet(31)
        Assert.[True](full = correct)
    End Sub

    <TestMethod>
    Public Sub IsSingle()
        Dim colorSet As ColorSet = New ColorSet(2)
        Assert.[True](colorSet.IsSingle())
        Assert.[True](colorSet.GetSingleColor() = 1)
    End Sub

    <TestMethod>
    Public Sub IsSingle2()
        Dim colorSet As ColorSet = New ColorSet(5)
        Assert.[True](Not colorSet.IsSingle())
        Dim cs2 As ColorSet = colorSet.RemoveColor(2)
        Assert.[True](cs2.IsSingle())
        Assert.[True](cs2.GetSingleColor() = 0)
    End Sub

    <TestMethod>
    Public Sub IsSingle3()
        Dim colorSet As ColorSet = New ColorSet()
        Assert.[True](Not colorSet.IsSingle())
    End Sub

    <TestMethod>
    Public Sub Union()
        Dim cs1 As ColorSet = New ColorSet(5)
        Dim cs2 As ColorSet = New ColorSet(10)
        Dim final As ColorSet = ColorSet.CreateFullColorSet(3)
        Dim union As ColorSet = cs1.Union(cs2)
        Assert.[True](union = final)
    End Sub

    <TestMethod>
    Public Sub Intersect()
        Dim cs1 As ColorSet = New ColorSet(5)
        Dim cs2 As ColorSet = New ColorSet(10)
        Dim final As ColorSet = New ColorSet()
        Dim intersect As ColorSet = cs1.Intersect(cs2)
        Assert.[True](intersect.IsEmpty)
        Assert.[True](intersect = final)
    End Sub

    <TestMethod>
    Public Sub Intersect2()
        Dim cs1 As ColorSet = New ColorSet(3)
        Dim cs2 As ColorSet = New ColorSet(6)
        Dim final As ColorSet = New ColorSet(2)
        Dim intersect As ColorSet = cs1.Intersect(cs2)
        Assert.[True](intersect = final)
    End Sub
End Class