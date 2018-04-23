Imports Microsoft.VisualBasic
Imports System
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraEditors.Repository
Imports DevExpress.XtraGrid.Columns
Imports DevExpress.XtraEditors.Helpers
Imports System.Windows.Forms
Imports System.Drawing
Imports DevExpress.Utils
Imports DevExpress.Data
Imports System.Collections.Generic

Namespace MyXtraGrid
	Public Class MyGridView
		Inherits DevExpress.XtraGrid.Views.Grid.GridView
		Public Sub New()
			Me.New(Nothing)
		End Sub
		Public Sub New(ByVal grid As DevExpress.XtraGrid.GridControl)
			MyBase.New(grid)
		End Sub
		Protected Overrides ReadOnly Property ViewName() As String
			Get
				Return "MyGridView"
			End Get
		End Property

		Protected Overrides Function CreateDateFilterPopup(ByVal column As DevExpress.XtraGrid.Columns.GridColumn, ByVal ownerControl As System.Windows.Forms.Control, ByVal creator As Object) As DateFilterPopup
			Return New MyDateFilterPopup(Me, column, ownerControl, creator)
		End Function
	End Class

	Public Class MyDateFilterPopup
		Inherits DateFilterPopup

		Public Sub New(ByVal view As ColumnView, ByVal column As GridColumn, ByVal ownerControl As System.Windows.Forms.Control, ByVal creator As Object)
			MyBase.New(view, column, ownerControl, creator)
		End Sub

		Private dateCalendar_Renamed As DateControlEx = Nothing
		Private ReadOnly Property DateCalendar() As DateControlEx
			Get
				If dateCalendar_Renamed Is Nothing Then
					SetDateCalendar(DateFilterControl)
				End If
				Return dateCalendar_Renamed
			End Get
		End Property
		Private dateFilterControl_Renamed As PopupOutlookDateFilterControl = Nothing
		Private ReadOnly Property DateFilterControl() As PopupOutlookDateFilterControl
			Get
				If dateFilterControl_Renamed Is Nothing Then
					SetDateFilterControl(item)
				End If
				Return dateFilterControl_Renamed
			End Get
		End Property
		Private item As RepositoryItemPopupContainerEdit = Nothing

		Protected Overrides Function CreateRepositoryItem() As RepositoryItemPopupBase
			item = TryCast(MyBase.CreateRepositoryItem(), RepositoryItemPopupContainerEdit)
			AddHandler DateCalendar.CustomDrawDayNumberCell, AddressOf OnCustomDrawDayNumberCell
			Return item
		End Function

		Private Sub SetDateFilterControl(ByVal item As RepositoryItemPopupContainerEdit)
			For Each ctrl As Control In item.PopupControl.Controls
				If TypeOf ctrl Is PopupOutlookDateFilterControl Then
					dateFilterControl_Renamed = TryCast(ctrl, PopupOutlookDateFilterControl)
					Exit For
				End If
			Next ctrl
		End Sub

		Private Sub SetDateCalendar(ByVal dateFilterControl As PopupOutlookDateFilterControl)
			For Each c As Control In dateFilterControl.Controls
				If TypeOf c Is DateControlEx Then
					dateCalendar_Renamed = TryCast(c, DateControlEx)
					Exit For
				End If
			Next c
		End Sub

		Protected Shadows ReadOnly Property View() As MyGridView
			Get
				Return TryCast(MyBase.View, MyGridView)
			End Get
		End Property

		Private uniqueFilterPopupValues_Renamed() As Object = Nothing
		Private ReadOnly Property UniqueFilterPopupValues() As Object()
			Get
				If uniqueFilterPopupValues_Renamed Is Nothing Then
					uniqueFilterPopupValues_Renamed = GetUniqueFilterPopupValues()
				End If
				Return uniqueFilterPopupValues_Renamed
			End Get
		End Property

		Private Function GetUniqueFilterPopupValues() As Object()
			Dim dates As New List(Of Object)()
			For i As Integer = 0 To View.DataRowCount - 1
				Dim val As Object = View.GetRowCellValue(i, Column)
				If (Not dates.Contains(val)) Then
					dates.Add(val)
				End If
			Next i
			Return dates.ToArray()
		End Function

		Private Sub OnCustomDrawDayNumberCell(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Calendar.CustomDrawDayNumberCellEventArgs)
			If IsExistingDate(e.Date) Then
				e.Style.ForeColor = Color.BlueViolet
			End If
		End Sub

		Private Function IsExistingDate(ByVal dt As DateTime) As Boolean
			Dim index As Integer = Array.IndexOf(UniqueFilterPopupValues, dt)
			Return index <> -1
		End Function

		Public Overrides Sub Dispose()
			If DateCalendar IsNot Nothing Then
				RemoveHandler DateCalendar.CustomDrawDayNumberCell, AddressOf OnCustomDrawDayNumberCell
			End If
			MyBase.Dispose()
		End Sub
	End Class
End Namespace
