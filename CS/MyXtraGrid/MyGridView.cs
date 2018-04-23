
using System;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraEditors.Helpers;
using System.Windows.Forms;
using System.Drawing;
using DevExpress.Utils;
using DevExpress.Data;
using System.Collections.Generic;

namespace MyXtraGrid {
	public class MyGridView : DevExpress.XtraGrid.Views.Grid.GridView {
		public MyGridView() : this(null) {}
		public MyGridView(DevExpress.XtraGrid.GridControl grid) : base(grid) {
		}
		protected override string ViewName { get { return "MyGridView"; } }

        protected override DateFilterPopup CreateDateFilterPopup(DevExpress.XtraGrid.Columns.GridColumn column, System.Windows.Forms.Control ownerControl, object creator) {
            return new MyDateFilterPopup(this, column, ownerControl, creator);
        }
	}

    public class MyDateFilterPopup: DateFilterPopup {

        public MyDateFilterPopup(ColumnView view, GridColumn column, System.Windows.Forms.Control ownerControl, object creator) : base(view, column, ownerControl, creator) { }
        
        DateControlEx dateCalendar = null;
        DateControlEx DateCalendar {
            get {
                if(dateCalendar == null) SetDateCalendar(DateFilterControl);
                return dateCalendar;
            }
        }
        PopupOutlookDateFilterControl dateFilterControl = null;
        PopupOutlookDateFilterControl DateFilterControl {
            get {
                if(dateFilterControl == null) SetDateFilterControl(item);
                return dateFilterControl;
            }
        }
        RepositoryItemPopupContainerEdit item = null;

        protected override RepositoryItemPopupBase CreateRepositoryItem() {
            item = base.CreateRepositoryItem() as RepositoryItemPopupContainerEdit;
            DateCalendar.CustomDrawDayNumberCell += OnCustomDrawDayNumberCell;
            return item;
        }

        private void SetDateFilterControl(RepositoryItemPopupContainerEdit item) {
            foreach(Control ctrl in item.PopupControl.Controls)
                if(ctrl is PopupOutlookDateFilterControl) {
                    dateFilterControl = ctrl as PopupOutlookDateFilterControl;
                    break;
                }
        }

        private void SetDateCalendar(PopupOutlookDateFilterControl dateFilterControl) {
            foreach(Control c in dateFilterControl.Controls)
                if(c is DateControlEx) {
                    dateCalendar = c as DateControlEx;
                    break;
                }
        }

        protected new MyGridView View {
            get { return base.View as MyGridView; }
        }

        object[] uniqueFilterPopupValues = null;
        object[] UniqueFilterPopupValues {
            get {
                if(uniqueFilterPopupValues == null)
                    uniqueFilterPopupValues = GetUniqueFilterPopupValues();
                return uniqueFilterPopupValues; }
        }

        object[] GetUniqueFilterPopupValues() {
            List<object> dates = new List<object>();
            for(int i = 0;i < View.DataRowCount;i++) {
                object val = View.GetRowCellValue(i, Column);
                if(!dates.Contains(val))
                    dates.Add(val);
            }
            return dates.ToArray();
        }

        void OnCustomDrawDayNumberCell(object sender, DevExpress.XtraEditors.Calendar.CustomDrawDayNumberCellEventArgs e) {
            if(IsExistingDate(e.Date))
                e.Style.ForeColor = Color.BlueViolet;
        }

        bool IsExistingDate(DateTime dt) {
            int index = Array.IndexOf(UniqueFilterPopupValues, dt);
            return index != -1;
        }

        public override void Dispose() {
            if(DateCalendar != null)
                DateCalendar.CustomDrawDayNumberCell -= OnCustomDrawDayNumberCell;
            base.Dispose();
        }
    }
}
