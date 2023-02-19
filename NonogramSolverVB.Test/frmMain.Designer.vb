<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMain
    Inherits System.Windows.Forms.Form

    'Форма переопределяет dispose для очистки списка компонентов.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Является обязательной для конструктора форм Windows Forms
    Private components As System.ComponentModel.IContainer

    'Примечание: следующая процедура является обязательной для конструктора форм Windows Forms
    'Для ее изменения используйте конструктор форм Windows Form.  
    'Не изменяйте ее в редакторе исходного кода.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.cmdTest = New System.Windows.Forms.Button()
        Me.lblStatus = New System.Windows.Forms.Label()
        Me.lblSummary = New System.Windows.Forms.Label()
        Me.failuresList = New System.Windows.Forms.ListBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.rtbDebug = New System.Windows.Forms.RichTextBox()
        Me.SuspendLayout()
        '
        'cmdTest
        '
        Me.cmdTest.Location = New System.Drawing.Point(12, 12)
        Me.cmdTest.Name = "cmdTest"
        Me.cmdTest.Size = New System.Drawing.Size(75, 23)
        Me.cmdTest.TabIndex = 0
        Me.cmdTest.Text = "Do Test"
        Me.cmdTest.UseVisualStyleBackColor = True
        '
        'lblStatus
        '
        Me.lblStatus.AutoSize = True
        Me.lblStatus.Location = New System.Drawing.Point(12, 54)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(37, 13)
        Me.lblStatus.TabIndex = 1
        Me.lblStatus.Text = "Status"
        '
        'lblSummary
        '
        Me.lblSummary.AutoSize = True
        Me.lblSummary.Location = New System.Drawing.Point(12, 86)
        Me.lblSummary.Name = "lblSummary"
        Me.lblSummary.Size = New System.Drawing.Size(50, 13)
        Me.lblSummary.TabIndex = 2
        Me.lblSummary.Text = "Summary"
        '
        'failuresList
        '
        Me.failuresList.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.failuresList.FormattingEnabled = True
        Me.failuresList.Location = New System.Drawing.Point(12, 330)
        Me.failuresList.Name = "failuresList"
        Me.failuresList.Size = New System.Drawing.Size(776, 108)
        Me.failuresList.TabIndex = 3
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 314)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(37, 13)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "Errors:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(12, 111)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(42, 13)
        Me.Label2.TabIndex = 5
        Me.Label2.Text = "Debug:"
        '
        'rtbDebug
        '
        Me.rtbDebug.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.rtbDebug.Location = New System.Drawing.Point(12, 127)
        Me.rtbDebug.Name = "rtbDebug"
        Me.rtbDebug.Size = New System.Drawing.Size(776, 184)
        Me.rtbDebug.TabIndex = 6
        Me.rtbDebug.Text = ""
        '
        'frmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(800, 450)
        Me.Controls.Add(Me.rtbDebug)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.failuresList)
        Me.Controls.Add(Me.lblSummary)
        Me.Controls.Add(Me.lblStatus)
        Me.Controls.Add(Me.cmdTest)
        Me.Name = "frmMain"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Nonogram Solver Test"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents cmdTest As Button
    Friend WithEvents lblStatus As Label
    Friend WithEvents lblSummary As Label
    Friend WithEvents failuresList As ListBox
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents rtbDebug As RichTextBox
End Class
