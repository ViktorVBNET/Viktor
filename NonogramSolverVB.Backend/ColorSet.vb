Imports System
Imports System.Diagnostics

Public Structure ColorSet
    Public Sub New(ByVal val As UInteger)
        mValue = val
    End Sub

    Private ReadOnly mValue As UInteger

    Public ReadOnly Property IsEmpty As Boolean
        Get
            Return (mValue = 0)
        End Get
    End Property

    Public Function HasColor(ByVal color As UInteger) As Boolean
        Dim mask As UInteger = ColorToMask(color)
        Return (mValue And mask) <> 0
    End Function

    Public Function IsSingle() As Boolean
        Dim val As UInt32 = mValue

        While (val <> 0)
            'If we ever hit a point where val Is 1, then there must only be one bit
            If (val = 1) Then
                Return True
            End If

            'Val Is Not 1 (implicit from above) but the lowest bit Is 1 => more than one bit Is 1
            If (val And 1) <> 0 Then
                Return False
            End If

            val = val >> 1
        End While

        Return False
    End Function

    Public Shared ReadOnly Empty As ColorSet = New ColorSet(0)

    Public Shared Function CreateFullColorSet(ByVal maxColor As UInteger) As ColorSet
        'Lets suppose there are 8 bits
        'Max = 1111,1111
        'Suppose the max color Is 2
        'This means that the valid color indexes are:
        '001, 010, 100
        'The number of bits that Is used by colors Is 3
        'And then the full color set Is:
        '0000,0111 = Max >> 5
        Dim numbits As UInt32 = maxColor + 1
        Dim shiftby As Integer = CInt((32UI - numbits))
        Dim mask As UInt32 = UInt32.MaxValue >> shiftby
        Return New ColorSet(mask)
    End Function

    Public Shared Function CreateSingle(ByVal color As UInteger) As ColorSet
        Return New ColorSet(ColorToMask(color))
    End Function

    Public Function Union(ByVal other As ColorSet) As ColorSet
        Return New ColorSet(mValue Or other.mValue)
    End Function

    Public Function Intersect(ByVal other As ColorSet) As ColorSet
        Return New ColorSet(mValue And other.mValue)
    End Function

    Public Function RemoveColor(ByVal color As UInteger) As ColorSet
        Dim mask As UInteger = ColorToMask(color)
        Return New ColorSet(mValue And (Not mask))
    End Function

    Public Function GetSingleColor() As UInteger
        If Not IsSingle() Then
            Throw New InvalidOperationException("Cannot call 'GetSingleColor' on color set that has more than one color")
        End If

        Dim val As UInt32 = mValue
        Dim color As UInt32 = 0
        While val > 1
            color += 1
            val = val >> 1
        End While
        Return color
    End Function

    Public Function AddColor(ByVal color As UInteger) As ColorSet
        Dim mask As UInteger = ColorToMask(color)
        Return New ColorSet(mValue Or mask)
    End Function

    Public Shared Operator =(ByVal first As ColorSet, ByVal second As ColorSet) As Boolean
        Return first.mValue = second.mValue
    End Operator

    Public Shared Operator <>(ByVal first As ColorSet, ByVal second As ColorSet) As Boolean
        Return first.mValue <> second.mValue
    End Operator

    Public Overrides Function Equals(ByVal obj As Object) As Boolean
        If obj Is Nothing OrElse Not Me.GetType().Equals(obj.GetType()) Then
            Return False
        Else
            Dim p As ColorSet = DirectCast(obj, ColorSet)
            Return Me.mValue = p.mValue AndAlso IsSingle() = p.IsSingle
        End If

        'Return MyBase.Equals(obj)
    End Function

    Public Overrides Function GetHashCode() As Integer
        Return CInt(mValue)
    End Function

    Public Overrides Function ToString() As String
        Return mValue.ToString()
    End Function

    Private Shared Function ColorToMask(ByVal color As UInteger) As UInteger
        Return 1 << CInt(color)
    End Function
End Structure