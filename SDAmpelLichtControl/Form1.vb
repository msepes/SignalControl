Imports System.Timers
Imports System
Public Class Form1

    Private st As Integer = 0
    Private Ampel As SDSignalControl
    Private tim As Timer

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Dim SignalControl = New SDSignalControl()

        SignalControl.Location = New Point(1, 1)

        SignalControl.AutoSize = True
        SignalControl.MinimumFontSize = 20
        SignalControl.Size = New Size(165, 25)
        SignalControl.CircleBorderWidth = 4
        SignalControl.CircleBorderColor = Color.Black
        SignalControl.CircleRadius = 12
        SignalControl.BackColor = Color.Red
        SignalControl.ForeColor = Color.Black
        SignalControl.Text = " Job Tabelle ist befüllt"
        SignalControl.TextAlign = SDSignalControl.TextPosition.Middle_Left
        SignalControl.CircleAnchorOnEdge = True

        Dim tt = New Timer()
        tim = tt
        tt.Interval = 2000
        AddHandler tt.Elapsed, AddressOf Me.Timer_Tick
        tt.Start()
        AddHandler SignalControl.CircleDoubleClick, AddressOf Me.test
        AddHandler SignalControl.CircleClick, AddressOf Me.test1
        Me.Controls.Add(SignalControl)
        Ampel = SignalControl
    End Sub

    Private Sub test(sender As Object, e As MouseEventArgs)
        MsgBox("CircleDoubleClick")
    End Sub

    Private Sub Timer_Tick(sender As Object, e As EventArgs)
        Dim sysaction = New Action(AddressOf setDisplay)

        Me.Invoke(sysaction)
    End Sub

    Private Sub setDisplay()
        If Me.st = 0 Then
            Ampel.Text = "Job Tabelle leer"
            Ampel.BackColor = Color.Green
            Me.st = Me.st + 1
        ElseIf Me.st = 1 Then
            Ampel.Text = "Blockade in Job Tabelle"
            Ampel.BackColor = Color.Red
            Me.st = Me.st + 1
        ElseIf Me.st = 2 Then
            Ampel.Text = "Jobs werden Bearbeitet"
            Ampel.BackColor = Color.Yellow
            Me.st = 0
        End If
        Ampel.Refresh()

    End Sub
    Private Sub test1(sender As Object, e As MouseEventArgs)
        '  MsgBox("CircleClick")
    End Sub

    Private Sub Form1_Closing(sender As Object, e As EventArgs)
        RemoveHandler tim.Elapsed, AddressOf Me.Timer_Tick
    End Sub
End Class
