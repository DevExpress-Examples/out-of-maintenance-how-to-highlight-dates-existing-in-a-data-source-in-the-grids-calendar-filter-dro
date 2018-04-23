using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MyXtraGrid {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e) {
            DataTable dt = new DataTable();
            dt.Columns.Add("ID", typeof(int));
            dt.Columns.Add("Name");
            dt.Columns.Add("Date", typeof(DateTime));
            for(int i = 0;i < 15;i++) {
                dt.Rows.Add(new object[] { i, string.Format("Name {0}", i),DateTime.Today.AddDays(i) });
            }
            myGridControl1.DataSource = dt;
        }
    }
}