Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Text
Imports System.Windows.Forms

Namespace MyXtraGrid
	Partial Public Class Form1
		Inherits Form
		Public Sub New()
			InitializeComponent()
		End Sub

		Private Sub Form1_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
			Dim dt As New DataTable()
			dt.Columns.Add("ID", GetType(Integer))
			dt.Columns.Add("Name")
			dt.Columns.Add("Date", GetType(DateTime))
			For i As Integer = 0 To 14
				dt.Rows.Add(New Object() { i, String.Format("Name {0}", i),DateTime.Today.AddDays(i) })
			Next i
			myGridControl1.DataSource = dt
		End Sub
	End Class
End Namespace