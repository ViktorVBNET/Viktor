Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Threading.Tasks
Imports System.Runtime.InteropServices

Public Module BoardFactory
    Public Function CreateBoard(ByVal rowConstraints As IEnumerable(Of IConstraintSet),
                                ByVal colConstraints As IEnumerable(Of IConstraintSet),
                                ByVal maxColor As UInteger) As IBoard
        Dim colors As ColorSpace = New ColorSpace(maxColor)
        Dim board As Board = New Board(rowConstraints, colConstraints, colors)
        Return board
    End Function

    Public Function CreateConstraintSet(ByVal constraints As IEnumerable(Of Constraint)) As IConstraintSet
        Return New ConstraintSet(constraints)
    End Function

    Public Sub CreateConstraintsForGrid(ByVal grid As UInteger(,),
                                 <Out> ByRef rowConstraints As IReadOnlyList(Of IConstraintSet),
                                 <Out> ByRef colConstraints As IReadOnlyList(Of IConstraintSet))

        Dim extractor As ConstraintExtractor = New ConstraintExtractor(grid)
        extractor.Extract()
        rowConstraints = extractor.RowConstraints
        colConstraints = extractor.ColumnConstraints
    End Sub
End Module