Imports System
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Windows.Forms

Namespace MyXtraGrid

    Public Partial Class Form1
        Inherits Form

        Public Sub New()
            InitializeComponent()
        End Sub

        Private Sub Form1_Load(ByVal sender As Object, ByVal e As EventArgs)
            Dim dt As DataTable = New DataTable()
            dt.Columns.Add("ID", GetType(Integer))
            dt.Columns.Add("Name")
            dt.Columns.Add("Date", GetType(Date))
            For i As Integer = 0 To 15 - 1
                dt.Rows.Add(New Object() {i, String.Format("Name {0}", i), Date.Today.AddDays(i)})
            Next

            myGridControl1.DataSource = dt
        End Sub
    End Class
End Namespace
