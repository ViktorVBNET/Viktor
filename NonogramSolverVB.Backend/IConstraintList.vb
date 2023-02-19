Imports System
Imports System.Collections.Generic
Imports System.Text

Friend Interface IConstraintList
    Sub CalculateEstimatedCost(ByVal boardView As IBoardView)
    Function ConstrainBoard(ByVal boardView As IBoardView) As ConstrainResult
End Interface