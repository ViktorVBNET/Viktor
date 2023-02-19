Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Threading.Tasks
Imports NonogramSolverVB.Backend

<TestClass>
Class FullTest
    <TestMethod>
    Public Sub Basic()
        Const BlackColor As UInteger = 1
        Dim row1 As List(Of Constraint) = New List(Of Constraint) From {New Constraint(BlackColor, 1)}
        Dim row2 As List(Of Constraint) = New List(Of Constraint) From {New Constraint(BlackColor, 2)}
        Dim row3 As List(Of Constraint) = New List(Of Constraint) From {New Constraint(BlackColor, 1)}
        Dim row1Set = BoardFactory.CreateConstraintSet(row1)
        Dim row2Set = BoardFactory.CreateConstraintSet(row2)
        Dim row3Set = BoardFactory.CreateConstraintSet(row3)
        Dim col1 As List(Of Constraint) = New List(Of Constraint) From {New Constraint(BlackColor, 1)}
        Dim col2 As List(Of Constraint) = New List(Of Constraint) From {New Constraint(BlackColor, 1)}
        Dim col3 As List(Of Constraint) = New List(Of Constraint) From {
            New Constraint(BlackColor, 1),
            New Constraint(BlackColor, 1)}

        Dim col1Set = BoardFactory.CreateConstraintSet(col1)
        Dim col2Set = BoardFactory.CreateConstraintSet(col2)
        Dim col3Set = BoardFactory.CreateConstraintSet(col3)
        Dim rowConstraints = {row1Set, row2Set, row3Set}
        Dim colConstraints = {col1Set, col2Set, col3Set}
        Dim board As IBoard = BoardFactory.CreateBoard(rowConstraints, colConstraints, BlackColor)
        Dim solved As ISolvedBoard = board.Solve()
        Assert.[True](solved(0, 0) = 0)
        Assert.[True](solved(0, 1) = 0)
        Assert.[True](solved(0, 2) = BlackColor)
        Assert.[True](solved(1, 0) = BlackColor)
        Assert.[True](solved(1, 1) = BlackColor)
        Assert.[True](solved(1, 2) = 0)
        Assert.[True](solved(2, 0) = 0)
        Assert.[True](solved(2, 1) = 0)
        Assert.[True](solved(2, 2) = BlackColor)
    End Sub

    <TestMethod>
    Public Sub Basic2()
        Const BlackColor As UInteger = 1
        Const RedColor As UInteger = 2
        Dim blackConstr As Constraint = New Constraint(BlackColor, 1)
        Dim blackConstr2 As Constraint = New Constraint(BlackColor, 2)
        Dim redConstr As Constraint = New Constraint(RedColor, 1)
        Dim redConstr2 As Constraint = New Constraint(RedColor, 2)
        Dim row0 = BoardFactory.CreateConstraintSet({blackConstr2, redConstr})
        Dim row1 = BoardFactory.CreateConstraintSet({blackConstr, redConstr2})
        Dim row2 = BoardFactory.CreateConstraintSet({redConstr2, blackConstr})
        Dim row3 = BoardFactory.CreateConstraintSet({redConstr, blackConstr2})
        Dim col0 = BoardFactory.CreateConstraintSet({blackConstr, redConstr2})
        Dim col1 = BoardFactory.CreateConstraintSet({blackConstr2, redConstr})
        Dim col2 = BoardFactory.CreateConstraintSet({redConstr, blackConstr2})
        Dim col3 = BoardFactory.CreateConstraintSet({redConstr2, blackConstr})
        Dim rowConstraints = {row0, row1, row2, row3}
        Dim colConstraints = {col0, col1, col2, col3}
        Dim board As IBoard = BoardFactory.CreateBoard(rowConstraints, colConstraints, RedColor)
        Dim solvedBoard As ISolvedBoard = board.Solve()
        Assert.[True](solvedBoard(0, 0) = BlackColor)
        Assert.[True](solvedBoard(0, 1) = BlackColor)
        Assert.[True](solvedBoard(0, 2) = 0)
        Assert.[True](solvedBoard(0, 3) = RedColor)
        Assert.[True](solvedBoard(1, 0) = 0)
        Assert.[True](solvedBoard(1, 1) = BlackColor)
        Assert.[True](solvedBoard(1, 2) = RedColor)
        Assert.[True](solvedBoard(1, 3) = RedColor)
        Assert.[True](solvedBoard(2, 0) = RedColor)
        Assert.[True](solvedBoard(2, 1) = RedColor)
        Assert.[True](solvedBoard(2, 2) = BlackColor)
        Assert.[True](solvedBoard(2, 3) = 0)
        Assert.[True](solvedBoard(3, 0) = RedColor)
        Assert.[True](solvedBoard(3, 1) = 0)
        Assert.[True](solvedBoard(3, 2) = BlackColor)
        Assert.[True](solvedBoard(3, 3) = BlackColor)
    End Sub

    <TestMethod>
    Public Sub Complex()
        Const GreenColor As UInteger = 1
        Const LightGreenColor As UInteger = 2
        Const BrownColor As UInteger = 3
        Dim greenConstr As Constraint = New Constraint(GreenColor, 1)
        Dim greenConstr2 As Constraint = New Constraint(GreenColor, 2)
        Dim greenConstr4 As Constraint = New Constraint(GreenColor, 4)
        Dim lgreenConstr As Constraint = New Constraint(LightGreenColor, 1)
        Dim lgreenConstr2 As Constraint = New Constraint(LightGreenColor, 2)
        Dim lgreenConstr3 As Constraint = New Constraint(LightGreenColor, 3)
        Dim lgreenConstr4 As Constraint = New Constraint(LightGreenColor, 4)
        Dim brownConstr2 As Constraint = New Constraint(BrownColor, 2)
        Dim brownConstr3 As Constraint = New Constraint(BrownColor, 3)
        Dim row1 = BoardFactory.CreateConstraintSet({greenConstr4})
        Dim row2 = BoardFactory.CreateConstraintSet({greenConstr, greenConstr})
        Dim row3 = BoardFactory.CreateConstraintSet({greenConstr, lgreenConstr2, greenConstr})
        Dim row4 = BoardFactory.CreateConstraintSet({greenConstr, lgreenConstr4, greenConstr})
        Dim row5 = BoardFactory.CreateConstraintSet({greenConstr, lgreenConstr2, greenConstr})
        Dim row6 = BoardFactory.CreateConstraintSet({greenConstr2, greenConstr2})
        Dim row7 = BoardFactory.CreateConstraintSet({greenConstr, brownConstr2, greenConstr})
        Dim row8 = BoardFactory.CreateConstraintSet({brownConstr2})
        Dim row9 = BoardFactory.CreateConstraintSet({brownConstr2})
        Dim col1 = BoardFactory.CreateConstraintSet({greenConstr2})
        Dim col2 = BoardFactory.CreateConstraintSet({greenConstr2, greenConstr})
        Dim col3 = BoardFactory.CreateConstraintSet({greenConstr, lgreenConstr, greenConstr2})
        Dim col4 = BoardFactory.CreateConstraintSet({greenConstr, lgreenConstr3, brownConstr3})
        Dim col5 = BoardFactory.CreateConstraintSet({greenConstr, lgreenConstr3, brownConstr3})
        Dim col6 = BoardFactory.CreateConstraintSet({greenConstr, lgreenConstr, greenConstr2})
        Dim col7 = BoardFactory.CreateConstraintSet({greenConstr2, greenConstr})
        Dim col8 = BoardFactory.CreateConstraintSet({greenConstr2})
        Dim rows = {row1, row2, row3, row4, row5, row6, row7, row8, row9}
        Dim cols = {col1, col2, col3, col4, col5, col6, col7, col8}
        Dim board As IBoard = BoardFactory.CreateBoard(rows, cols, BrownColor)
        Dim solved As ISolvedBoard = board.Solve()
        Assert.[True](solved(0, 0) = 0)
        Assert.[True](solved(0, 1) = 0)
        Assert.[True](solved(0, 2) = GreenColor)
        Assert.[True](solved(0, 3) = GreenColor)
        Assert.[True](solved(0, 4) = GreenColor)
        Assert.[True](solved(0, 5) = GreenColor)
        Assert.[True](solved(0, 6) = 0)
        Assert.[True](solved(0, 7) = 0)
        Assert.[True](solved(1, 0) = 0)
        Assert.[True](solved(1, 1) = GreenColor)
        Assert.[True](solved(1, 2) = 0)
        Assert.[True](solved(1, 3) = 0)
        Assert.[True](solved(1, 4) = 0)
        Assert.[True](solved(1, 5) = 0)
        Assert.[True](solved(1, 6) = GreenColor)
        Assert.[True](solved(1, 7) = 0)
        Assert.[True](solved(2, 0) = 0)
        Assert.[True](solved(2, 1) = GreenColor)
        Assert.[True](solved(2, 2) = 0)
        Assert.[True](solved(2, 3) = LightGreenColor)
        Assert.[True](solved(2, 4) = LightGreenColor)
        Assert.[True](solved(2, 5) = 0)
        Assert.[True](solved(2, 6) = GreenColor)
        Assert.[True](solved(2, 7) = 0)
        Assert.[True](solved(3, 0) = GreenColor)
        Assert.[True](solved(3, 1) = 0)
        Assert.[True](solved(3, 2) = LightGreenColor)
        Assert.[True](solved(3, 3) = LightGreenColor)
        Assert.[True](solved(3, 4) = LightGreenColor)
        Assert.[True](solved(3, 5) = LightGreenColor)
        Assert.[True](solved(3, 6) = 0)
        Assert.[True](solved(3, 7) = GreenColor)
        Assert.[True](solved(4, 0) = GreenColor)
        Assert.[True](solved(4, 1) = 0)
        Assert.[True](solved(4, 2) = 0)
        Assert.[True](solved(4, 3) = LightGreenColor)
        Assert.[True](solved(4, 4) = LightGreenColor)
        Assert.[True](solved(4, 5) = 0)
        Assert.[True](solved(4, 6) = 0)
        Assert.[True](solved(4, 7) = GreenColor)
        Assert.[True](solved(5, 0) = 0)
        Assert.[True](solved(5, 1) = GreenColor)
        Assert.[True](solved(5, 2) = GreenColor)
        Assert.[True](solved(5, 3) = 0)
        Assert.[True](solved(5, 4) = 0)
        Assert.[True](solved(5, 5) = GreenColor)
        Assert.[True](solved(5, 6) = GreenColor)
        Assert.[True](solved(5, 7) = 0)
        Assert.[True](solved(6, 0) = 0)
        Assert.[True](solved(6, 1) = 0)
        Assert.[True](solved(6, 2) = GreenColor)
        Assert.[True](solved(6, 3) = BrownColor)
        Assert.[True](solved(6, 4) = BrownColor)
        Assert.[True](solved(6, 5) = GreenColor)
        Assert.[True](solved(6, 6) = 0)
        Assert.[True](solved(6, 7) = 0)
        Assert.[True](solved(7, 0) = 0)
        Assert.[True](solved(7, 1) = 0)
        Assert.[True](solved(7, 2) = 0)
        Assert.[True](solved(7, 3) = BrownColor)
        Assert.[True](solved(7, 4) = BrownColor)
        Assert.[True](solved(7, 5) = 0)
        Assert.[True](solved(7, 6) = 0)
        Assert.[True](solved(7, 7) = 0)
        Assert.[True](solved(8, 0) = 0)
        Assert.[True](solved(8, 1) = 0)
        Assert.[True](solved(8, 2) = 0)
        Assert.[True](solved(8, 3) = BrownColor)
        Assert.[True](solved(8, 4) = BrownColor)
        Assert.[True](solved(8, 5) = 0)
        Assert.[True](solved(8, 6) = 0)
        Assert.[True](solved(8, 7) = 0)
    End Sub

    <TestMethod>
    Public Sub FlagTest()
        Const W As UInteger = 0
        Const B As UInteger = 1
        Const G As UInteger = 2
        Dim flagImage As UInteger(,) = New UInteger(4, 4) {
        {W, W, B, B, B},
        {W, B, B, B, W},
        {W, B, W, G, W},
        {W, G, G, G, W},
        {G, G, G, W, W}}
        TestImage(flagImage, G)
    End Sub

    <TestMethod>
    Public Sub LadybugTest()
        Const W As UInteger = 0
        Const B As UInteger = 1
        Const R As UInteger = 2
        Const G As UInteger = 3
        Dim ladybugImage As UInteger(,) = New UInteger(19, 19) {
        {W, W, W, W, B, B, W, W, W, B, B, B, W, W, W, W, B, B, W, W},
        {W, W, W, W, W, B, B, B, W, B, W, W, W, W, B, B, B, W, W, W},
        {W, W, W, W, W, W, W, B, R, R, R, R, B, B, B, W, B, B, W, B},
        {W, W, W, W, B, R, R, R, R, R, R, R, B, B, B, B, B, B, B, B},
        {W, W, W, B, R, R, R, R, R, R, R, W, B, B, B, B, B, W, B, W},
        {W, W, B, R, R, R, R, R, R, R, R, W, W, B, B, B, B, B, B, W},
        {W, W, B, R, R, R, B, B, R, R, R, B, B, B, B, B, B, B, W, W},
        {W, B, R, R, R, R, B, B, R, R, R, B, B, B, W, B, B, B, W, W},
        {W, B, R, R, R, R, R, R, R, R, R, B, B, B, W, W, R, R, W, B},
        {W, R, R, R, R, R, R, R, R, R, B, R, R, R, R, R, R, R, W, B},
        {W, R, R, R, R, R, R, R, R, B, R, R, R, R, R, R, R, R, B, B},
        {B, B, R, B, B, R, R, R, B, R, R, R, R, R, R, R, R, R, W, W},
        {B, W, R, B, B, R, R, B, R, R, R, R, B, B, R, R, R, B, W, W},
        {B, G, R, R, R, R, B, R, R, R, R, R, B, B, R, R, R, B, B, W},
        {W, G, G, R, R, B, R, R, R, R, R, R, R, R, R, R, R, W, B, W},
        {W, G, G, R, B, R, R, R, B, B, R, R, R, R, R, R, B, W, B, B},
        {W, G, W, W, R, R, R, R, B, B, R, R, R, R, R, B, W, W, W, W},
        {W, W, W, W, G, G, R, R, R, R, R, R, R, B, B, W, W, W, W, W},
        {W, W, W, G, G, G, G, W, B, R, R, B, B, W, W, W, W, W, W, W},
        {W, W, W, W, W, W, B, B, B, W, W, W, W, W, W, W, W, W, W, W}}
        TestImage(ladybugImage, G)
    End Sub

    <TestMethod>
    Public Sub SolvingTest()
        Dim Black As ConstraintHelper = New ConstraintHelper(1)
        Dim Orange As ConstraintHelper = New ConstraintHelper(2)
        Dim LightBlue As ConstraintHelper = New ConstraintHelper(3)
        Dim DarkBlue As ConstraintHelper = New ConstraintHelper(4)
        Dim rowConstraintHelpers As List(Of IConstraintHelper) = New List(Of IConstraintHelper)() From {
            9 * LightBlue + 4 * Black + 9 * LightBlue,
            5 * LightBlue + 3 * LightBlue + 3 * Black + 5 * Black + 7 * LightBlue,
            3 * LightBlue + 2 * LightBlue + 2 * Black + 10 * Black + 6 * LightBlue,
            6 * LightBlue + 15 * Black + 2 * LightBlue,
            3 * LightBlue + 17 * Black + 5 * LightBlue,
            5 * LightBlue + 18 * Black + 5 * LightBlue,
            4 * LightBlue + 7 * Black + 3 * Black + 4 * Black + 3 * LightBlue,
            6 * Black + Black + 3 * Black + 3 * LightBlue,
            7 * Black + 3 * Black,
            6 * Black + 2 * Black + 2 * Black + 2 * Black,
            6 * Black + Black + 2 * Black + Black + 2 * Black + 2 * Black,
            6 * Black + Black + Black + Black + Black + 3 * Black,
            5 * Black + 4 * Black + 2 * Orange + 2 * Black + 3 * Black,
            4 * Black + Black + 4 * Orange + 2 * Black,
            4 * Black + Black + 3 * Orange + 2 * Black,
            3 * Black + Black + Orange + 2 * Black,
            9 * DarkBlue + 2 * Black + 3 * Black + 6 * DarkBlue,
            10 * DarkBlue + 6 * Black + 4 * Black + 7 * DarkBlue,
            6 * DarkBlue + 6 * Black + 2 * Black,
            6 * Black + 2 * Black + 4 * DarkBlue,
            4 * DarkBlue + 6 * Black + 4 * Black + 2 * DarkBlue,
            3 * DarkBlue + 7 * Black + 7 * Black,
            3 * DarkBlue + 6 * Black + 6 * Black,
            7 * Black + 5 * Black + 2 * DarkBlue,
            2 * DarkBlue + LightBlue + 6 * Black + 3 * Black,
            3 * LightBlue + 6 * Black + 2 * Black + 2 * LightBlue,
            3 * LightBlue + 6 * Black + Black + 3 * LightBlue,
            2 * LightBlue + 4 * Black + 2 * Black + 2 * Black + 2 * Black + 2 * Black + 2 * LightBlue,
            LightBlue + 2 * Black + 2 * Black + Black + Orange + Black + Orange + 2 * Black + Black + Orange + Black + LightBlue,
            2 * LightBlue + Black + Black + 2 * Black + Black + 5 * Orange + Black + 3 * Orange + Black + 2 * LightBlue,
            2 * LightBlue + 5 * Black + Black + 4 * Orange + Black + Black + 4 * Orange + Black + 3 * LightBlue,
            4 * LightBlue + 6 * Black + 4 * Orange + Black + 2 * Black + 3 * Orange + Black + 3 * LightBlue,
            DarkBlue + 6 * LightBlue + 6 * Black + 2 * Orange + 4 * Black + 2 * Orange + 2 * Black + 3 * LightBlue + 2 * DarkBlue,
            3 * DarkBlue + 6 * LightBlue + 4 * Black + 2 * Orange + Black + 3 * Black + 4 * LightBlue + 3 * DarkBlue,
            4 * DarkBlue + 4 * LightBlue + 2 * Black + 2 * LightBlue + 4 * DarkBlue,
            6 * DarkBlue + 4 * LightBlue + LightBlue + 3 * DarkBlue,
            5 * DarkBlue + 2 * LightBlue + 2 * LightBlue + 3 * DarkBlue,
            3 * DarkBlue + 5 * LightBlue + 3 * LightBlue + 3 * DarkBlue,
            4 * DarkBlue + 8 * LightBlue + 5 * LightBlue + 2 * DarkBlue,
            6 * DarkBlue + 20 * LightBlue + 3 * DarkBlue,
            8 * DarkBlue + 14 * LightBlue + 4 * DarkBlue,
            7 * DarkBlue + 12 * LightBlue + 4 * DarkBlue,
            10 * DarkBlue + 3 * LightBlue + 6 * DarkBlue,
            19 * DarkBlue,
            14 * DarkBlue
        }
        Assert.[True](rowConstraintHelpers.Count = 45)
        Dim colConstraintHelpers As List(Of IConstraintHelper) = New List(Of IConstraintHelper)() From {
            LightBlue + 2 * DarkBlue + DarkBlue + DarkBlue + 3 * LightBlue + 3 * DarkBlue,
            2 * LightBlue + LightBlue + 3 * DarkBlue + DarkBlue + DarkBlue + DarkBlue + 2 * LightBlue + 4 * LightBlue + 3 * DarkBlue,
            3 * LightBlue + LightBlue + 3 * DarkBlue + DarkBlue + DarkBlue + LightBlue + 3 * LightBlue + 3 * DarkBlue,
            2 * LightBlue + LightBlue + 2 * LightBlue + 3 * DarkBlue + 2 * DarkBlue + 2 * LightBlue + 3 * LightBlue + 3 * DarkBlue,
            2 * LightBlue + 4 * LightBlue + 3 * DarkBlue + 2 * DarkBlue + LightBlue + 4 * LightBlue + 2 * DarkBlue + DarkBlue,
            2 * LightBlue + 4 * LightBlue + 3 * DarkBlue + DarkBlue + 2 * LightBlue + 3 * LightBlue + 2 * DarkBlue + 2 * DarkBlue,
            LightBlue + 2 * LightBlue + LightBlue + 3 * DarkBlue + 5 * Black + 3 * LightBlue + 5 * DarkBlue,
            LightBlue + 2 * LightBlue + 2 * DarkBlue + 8 * Black + 3 * LightBlue + 5 * DarkBlue,
            3 * LightBlue + 5 * Black + 2 * DarkBlue + 10 * Black + LightBlue + 2 * LightBlue + 4 * DarkBlue,
            2 * LightBlue + 10 * Black + DarkBlue + 9 * Black + 4 * LightBlue + 3 * DarkBlue,
            2 * LightBlue + 12 * Black + 9 * Black + 2 * Black + LightBlue + 3 * LightBlue + 2 * DarkBlue,
            LightBlue + 13 * Black + 12 * Black + 2 * Black + 3 * LightBlue + 2 * DarkBlue + DarkBlue,
            22 * Black + 6 * Black + 3 * LightBlue + 4 * DarkBlue,
            10 * Black + 6 * Black + 4 * Black + 3 * LightBlue + 5 * DarkBlue,
            Black + 6 * Black + 3 * Black + 4 * Black + 2 * LightBlue + 5 * DarkBlue,
            6 * Black + 2 * Black + 2 * Black + 3 * Black + 3 * LightBlue + 4 * DarkBlue,
            5 * Black + Black + Black + Black + 7 * Black + 4 * LightBlue + 3 * DarkBlue,
            Black + 4 * Black + 2 * Black + Black + Black + 4 * Orange + 2 * Black + 3 * LightBlue + 3 * DarkBlue,
            6 * Black + 4 * Black + Black + 5 * Orange + Black + 3 * LightBlue + 3 * DarkBlue,
            7 * Black + Black + Orange + Black + Black + 6 * Orange + Black + 3 * LightBlue + 3 * DarkBlue,
            8 * Black + 3 * Orange + Black + 2 * Black + 3 * Orange + 2 * Black + 3 * LightBlue + 3 * DarkBlue,
            6 * Black + 2 * Black + 4 * Orange + Black + Orange + 3 * Black + 3 * LightBlue + 3 * DarkBlue,
            5 * Black + Black + Black + 2 * Orange + Black + 4 * LightBlue + 2 * DarkBlue,
            4 * Black + 2 * Black + Black + 2 * Black + 3 * Black + 4 * LightBlue + 2 * DarkBlue,
            5 * Black + 2 * Black + 4 * Black + 2 * Black + Orange + Black + 4 * LightBlue + 2 * DarkBlue,
            LightBlue + 6 * Black + 2 * Black + 3 * Black + Black + 3 * Orange + Black + 4 * LightBlue + 3 * DarkBlue,
            LightBlue + 9 * Black + 3 * Black + 4 * Black + 2 * Black + 3 * Orange + Black + 4 * LightBlue + 3 * DarkBlue,
            LightBlue + LightBlue + 11 * Black + 8 * Black + 4 * Orange + Black + 5 * LightBlue + 2 * DarkBlue,
            4 * LightBlue + 4 * Black + DarkBlue + 5 * Black + Black + 2 * Orange + Black + LightBlue + 4 * LightBlue + 3 * DarkBlue,
            5 * LightBlue + 2 * DarkBlue + 4 * Black + LightBlue + 2 * Black + 3 * LightBlue + 4 * LightBlue + 4 * DarkBlue,
            3 * LightBlue + 2 * LightBlue + LightBlue + 2 * DarkBlue + DarkBlue + 3 * Black + 2 * LightBlue + 6 * LightBlue + 6 * DarkBlue,
            3 * LightBlue + 2 * LightBlue + LightBlue + 2 * DarkBlue + DarkBlue + 2 * Black + LightBlue + 4 * LightBlue + 8 * DarkBlue,
            3 * LightBlue + 4 * LightBlue + 2 * DarkBlue + DarkBlue + 2 * LightBlue + 3 * LightBlue + 6 * DarkBlue + 2 * DarkBlue,
            2 * LightBlue + 3 * LightBlue + 2 * DarkBlue + 2 * DarkBlue + DarkBlue + 4 * LightBlue + 5 * DarkBlue,
            LightBlue + 2 * LightBlue + 2 * DarkBlue + DarkBlue + DarkBlue + 2 * DarkBlue
        }
        Assert.[True](colConstraintHelpers.Count = 35)
        Dim rowConstraints As New List(Of IConstraintSet)
        For Each h As IConstraintHelper In rowConstraintHelpers
            rowConstraints.Add(h.ToSet)
        Next
        Dim colConstraints As New List(Of IConstraintSet)
        For Each h As IConstraintHelper In colConstraintHelpers
            colConstraints.Add(h.ToSet)
        Next

        'Dim rowConstraints1 = rowConstraintHelpers.Select(Function(h) h.ToSet())
        'Dim colConstraints1 = colConstraintHelpers.Select(Function(h) h.ToSet())

        Dim board As IBoard = BoardFactory.CreateBoard(rowConstraints, colConstraints, 4)
        rowConstraints = Nothing
        colConstraints = Nothing
        Dim solved As ISolvedBoard = board.Solve()
        Assert.[True](solved IsNot Nothing)
    End Sub

    Private Shared Sub TestImage(ByVal image As UInteger(,), ByVal maxColor As UInteger)
        Dim board As IBoard = CreateBoardFromImage(image, maxColor)
        Dim solved As ISolvedBoard = board.Solve()
        CompareSolution(image, solved)
    End Sub

    Private Shared Function CreateBoardFromImage(ByVal image As UInteger(,), ByVal maxColor As UInteger) As IBoard
        Dim rows As IReadOnlyList(Of IConstraintSet) = Nothing, cols As IReadOnlyList(Of IConstraintSet) = Nothing
        BoardFactory.CreateConstraintsForGrid(image, rows, cols)
        Return BoardFactory.CreateBoard(rows, cols, maxColor)
    End Function

    Private Shared Sub CompareSolution(ByVal image As UInteger(,), ByVal solvedBoard As ISolvedBoard)
        Dim numRows As Integer = image.GetLength(0)
        Dim numCols As Integer = image.GetLength(1)

        For row As Integer = 0 To numRows - 1

            For col As Integer = 0 To numCols - 1
                Assert.[True](image(row, col) = solvedBoard(row, col))
            Next
        Next
    End Sub
End Class