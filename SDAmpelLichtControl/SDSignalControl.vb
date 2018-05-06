Imports System.ComponentModel

Public Class SDSignalControl
    Inherits Control

#Region "Declaration"

    Public Enum TextPosition
        Top_Left = 0
        Top_Right
        Middle_Left
        Middle_Right
        Botton_Left
        Botton_Right
    End Enum

    Private m_backColor As Color = Color.White
    Private m_ForeColor As Color = Color.Black
    Private m_CircleBorderColor As Color = Color.Black
    Private m_TextAlign As TextPosition = TextPosition.Middle_Left
    Private Const BorderWidthDefault As Integer = 1
    Private m_BorderWidth As Integer = 1
    Private m_blnAutoSize As Boolean = False
    Private m_blnCircleAnchorOnEdge As Boolean = False
    Private m_RectangleCircle As Rectangle = Rectangle.Empty
    Private m_intMiniFontSize As Integer
    Private m_intCircleRadius As Integer = -1

#End Region

#Region "Events"

    Public Event CircleDoubleClick(sender As Object, e As MouseEventArgs)
    Public Event CircleClick(sender As Object, e As MouseEventArgs)

#End Region

#Region "Eigenschaften"

    Public Property CircleRadius As Integer
        Get
            Return Me.m_intCircleRadius
        End Get
        Set(value As Integer)
            Me.m_intCircleRadius = value
        End Set
    End Property

    Public Property CircleAnchorOnEdge As Boolean
        Get
            Return Me.m_blnCircleAnchorOnEdge
        End Get
        Set(value As Boolean)
            Me.m_blnCircleAnchorOnEdge = value
        End Set
    End Property

    Public Property MinimumFontSize As Integer
        Get
            Return Me.m_intMiniFontSize
        End Get
        Set(value As Integer)
            Me.m_intMiniFontSize = value
            Me.Font = New Font(Me.Font.FontFamily, value, Me.Font.Style)
        End Set
    End Property

    Public Shadows Property BackColor As Color
        Get
            Return Me.m_backColor
        End Get
        Set(value As Color)
            Me.m_backColor = value
        End Set
    End Property

    Public Property CircleBorderWidth As Integer
        Get
            Return Me.m_BorderWidth
        End Get
        Set(value As Integer)
            If value >= Me.Size.Height / 4 AndAlso Me.Size <> Size.Empty Then
                Throw New Exception("CircleBorderWidth muss vier mal kleiner als der Wert in Size.Height sein")
            End If
            Me.m_BorderWidth = value
        End Set
    End Property

    Public Property CircleBorderColor As Color
        Get
            Return Me.m_CircleBorderColor
        End Get
        Set(value As Color)
            Me.m_CircleBorderColor = value
        End Set
    End Property

    Public Shadows Property ForeColor As Color
        Get
            Return Me.m_ForeColor
        End Get
        Set(value As Color)
            Me.m_ForeColor = value
        End Set
    End Property

    Public Property TextAlign As TextPosition
        Get
            Return Me.m_TextAlign
        End Get
        Set(value As TextPosition)
            Me.m_TextAlign = value
        End Set
    End Property



    Public Shadows Property AutoSize As Boolean
        Get
            Return Me.m_blnAutoSize
        End Get
        Set(value As Boolean)
            Me.m_blnAutoSize = value

            If Not value Then
                Exit Property
            End If
            Me.SetAutoSize()
        End Set
    End Property

    Public Overrides Property Text As String
        Get
            Return MyBase.Text
        End Get
        Set(value As String)
            MyBase.Text = value
            Me.SetAutoSize()
        End Set
    End Property

#End Region

#Region "Private Methoden"

    Private Function GetTextPosition(TextAlign As TextPosition, TotalSize As Size, TextSize As Size) As Point
        Select Case TextAlign
            Case TextPosition.Botton_Left
                Return New Point(0, TotalSize.Height - TextSize.Height)
            Case TextPosition.Botton_Right
                Return New Point(TotalSize.Width - TextSize.Width, TotalSize.Height - TextSize.Height)
            Case TextPosition.Middle_Left
                Return New Point(0, (TotalSize.Height / 2) - (TextSize.Height / 2))
            Case TextPosition.Middle_Right
                Return New Point(TotalSize.Width - TextSize.Width, (TotalSize.Height / 2) - (TextSize.Height / 2))
            Case TextPosition.Top_Left
                Return New Point(0, 0)
            Case TextPosition.Top_Right
                Return New Point(TotalSize.Width - TextSize.Width, 0)
        End Select
    End Function

    Private Function GetCirclePosition(TextAlign As TextPosition, TotalSize As Size, TextSize As Size) As Point
        Select Case TextAlign
            Case TextPosition.Botton_Left, TextPosition.Middle_Left, TextPosition.Top_Left
                Return New Point(TextSize.Width, 0)
            Case TextPosition.Botton_Right, TextPosition.Middle_Right, TextPosition.Top_Right
                Return New Point(0, 0)
        End Select
    End Function

    Private Sub SetAutoSize()

        If Not Me.AutoSize Then
            Return
        End If
        Using Graphic = Me.CreateGraphics()
            Dim sizeStringSize = Graphic.MeasureString(Me.Text, Me.Font)
            Me.Size = New Size(sizeStringSize.Width + sizeStringSize.Height, sizeStringSize.Height)
        End Using
    End Sub

#End Region

#Region "Eventhandler"

    Private Sub SDSignalControl_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles Me.MouseDoubleClick
        If m_RectangleCircle = Rectangle.Empty Then
            Return
        End If

        Dim MousePoint = e.Location

        If MousePoint.X < m_RectangleCircle.X OrElse MousePoint.X > m_RectangleCircle.Width + m_RectangleCircle.X Then
            Return
        End If

        If MousePoint.Y < m_RectangleCircle.Y OrElse MousePoint.Y > m_RectangleCircle.Height + m_RectangleCircle.Y Then
            Return
        End If

        RaiseEvent CircleDoubleClick(Me, e)
    End Sub

    Private Sub SDSignalControl_MouseClick(sender As Object, e As MouseEventArgs) Handles Me.MouseClick
        If m_RectangleCircle = Rectangle.Empty Then
            Return
        End If

        Dim MousePoint = e.Location

        If MousePoint.X < m_RectangleCircle.X OrElse MousePoint.X > m_RectangleCircle.Width + m_RectangleCircle.X Then
            Return
        End If

        If MousePoint.Y < m_RectangleCircle.Y OrElse MousePoint.Y > m_RectangleCircle.Height + m_RectangleCircle.Y Then
            Return
        End If

        RaiseEvent CircleClick(Me, e)
    End Sub

    Private Sub SDSignalControl_SizeChanged(sender As Object, e As EventArgs) Handles Me.SizeChanged

        Me.SetAutoSize()

        If Me.CircleBorderWidth = SDSignalControl.BorderWidthDefault Then
            Return
        End If

        If Me.CircleBorderWidth >= Me.Size.Height / 4 AndAlso Me.Size <> Size.Empty Then
            Throw New Exception("CircleBorderWidth muss vier mal kleiner als der Wert in Size.Height sein")
        End If
    End Sub

#End Region

#Region "Overrides"

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)

        e.Graphics.SmoothingMode = Drawing2D.SmoothingMode.HighQuality

        Dim sizeStringSize = e.Graphics.MeasureString(Me.Text, Me.Font)
        Dim TextPosition = Me.GetTextPosition(Me.TextAlign, Me.Size, sizeStringSize.ToSize())

        Using Brush As New SolidBrush(Me.ForeColor)
            e.Graphics.DrawString(Me.Text, Me.Font, Brush, TextPosition)
        End Using

        Using Brush As New SolidBrush(Me.BackColor)

            Dim sizRechtangleSize As Size = Size.Empty

            If Me.AutoSize OrElse Me.CircleRadius < 1 Then
                Dim intValue = Convert.ToInt32(IIf(Size.Width - sizeStringSize.Width < Size.Height, Size.Width - sizeStringSize.Width, Size.Height))
                sizRechtangleSize = New Size(intValue - Me.CircleBorderWidth, intValue - Me.CircleBorderWidth)
            End If

            If sizRechtangleSize = Size.Empty Then
                sizRechtangleSize = New Size((Me.CircleRadius * 2) - Me.CircleBorderWidth, (Me.CircleRadius * 2) - Me.CircleBorderWidth)
            End If

            Dim CirclePosition = Me.GetCirclePosition(Me.TextAlign, Me.Size, sizeStringSize.ToSize())

            Dim intY = CirclePosition.Y

            If sizeStringSize.Height >= sizRechtangleSize.Height Then
                intY = TextPosition.Y
            End If

            If intY + sizRechtangleSize.Height > Me.Height Then
                intY = 0
            End If

            Dim intX = CirclePosition.X

            If Me.CircleAnchorOnEdge AndAlso CirclePosition.X > 0 Then
                intX = Me.Size.Width - sizRechtangleSize.Width - Me.CircleBorderWidth
            End If

            Dim RealPosition = New Point(intX, intY + Math.Floor(Me.CircleBorderWidth / 2))

            m_RectangleCircle = New Rectangle(RealPosition, sizRechtangleSize)
            e.Graphics.FillEllipse(Brush, m_RectangleCircle)

            If Me.CircleBorderWidth < 1 Then
                Return
            End If

            Using BorderPen = New Pen(Me.CircleBorderColor, Me.CircleBorderWidth)
                e.Graphics.DrawEllipse(BorderPen, m_RectangleCircle)
            End Using

        End Using
    End Sub


#End Region

End Class