Imports System
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraEditors.Repository
Imports DevExpress.XtraGrid.Columns
Imports DevExpress.XtraEditors.Helpers
Imports System.Windows.Forms
Imports System.Drawing
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

        Protected Overrides ReadOnly Property ViewName As String
            Get
                Return "MyGridView"
            End Get
        End Property

        Protected Overrides Function CreateDateFilterPopup(ByVal column As GridColumn, ByVal ownerControl As Control, ByVal creator As Object) As DateFilterPopup
            Return New MyDateFilterPopup(Me, column, ownerControl, creator)
        End Function
    End Class

    Public Class MyDateFilterPopup
        Inherits DateFilterPopup

        Public Sub New(ByVal view As ColumnView, ByVal column As GridColumn, ByVal ownerControl As Control, ByVal creator As Object)
            MyBase.New(view, column, ownerControl, creator)
        End Sub

        Private dateCalendarField As DateControlEx = Nothing

        Private ReadOnly Property DateCalendar As DateControlEx
            Get
                If dateCalendarField Is Nothing Then SetDateCalendar(DateFilterControl)
                Return dateCalendarField
            End Get
        End Property

        Private dateFilterControlField As PopupOutlookDateFilterControl = Nothing

        Private ReadOnly Property DateFilterControl As PopupOutlookDateFilterControl
            Get
                If dateFilterControlField Is Nothing Then SetDateFilterControl(item)
                Return dateFilterControlField
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
                    dateFilterControlField = TryCast(ctrl, PopupOutlookDateFilterControl)
                    Exit For
                End If
            Next
        End Sub

        Private Sub SetDateCalendar(ByVal dateFilterControl As PopupOutlookDateFilterControl)
            For Each c As Control In dateFilterControl.Controls
                If TypeOf c Is DateControlEx Then
                    dateCalendarField = TryCast(c, DateControlEx)
                    Exit For
                End If
            Next
        End Sub

        Protected Overloads ReadOnly Property View As MyGridView
            Get
                Return TryCast(MyBase.View, MyGridView)
            End Get
        End Property

        Private uniqueFilterPopupValuesField As Object() = Nothing

        Private ReadOnly Property UniqueFilterPopupValues As Object()
            Get
                If uniqueFilterPopupValuesField Is Nothing Then uniqueFilterPopupValuesField = GetUniqueFilterPopupValues()
                Return uniqueFilterPopupValuesField
            End Get
        End Property

        Private Function GetUniqueFilterPopupValues() As Object()
            Dim dates As List(Of Object) = New List(Of Object)()
            For i As Integer = 0 To View.DataRowCount - 1
                Dim val As Object = View.GetRowCellValue(i, Column)
                If Not dates.Contains(val) Then dates.Add(val)
            Next

            Return dates.ToArray()
        End Function

        Private Sub OnCustomDrawDayNumberCell(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Calendar.CustomDrawDayNumberCellEventArgs)
            If IsExistingDate(e.Date) Then e.Style.ForeColor = Color.BlueViolet
        End Sub

        Private Function IsExistingDate(ByVal dt As Date) As Boolean
            Dim index As Integer = Array.IndexOf(UniqueFilterPopupValues, dt)
            Return index <> -1
        End Function

        Public Overrides Sub Dispose()
            If DateCalendar IsNot Nothing Then RemoveHandler DateCalendar.CustomDrawDayNumberCell, AddressOf OnCustomDrawDayNumberCell
            MyBase.Dispose()
        End Sub
    End Class
End Namespace
