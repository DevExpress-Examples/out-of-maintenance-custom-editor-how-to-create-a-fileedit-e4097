using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;

namespace CustomEditor {
    public partial class Form1 : XtraForm {
        public Form1() {
            InitializeComponent();
            CreateDataSource();
            ConfigureRepositoryItemButtonEdit();
            
        }

        private void CreateDataSource() {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Name", typeof(string));
            dataTable.Columns.Add("File with information", typeof(string));
            dataTable.Rows.Add(new object[] { "John Smith", "" });
            dataTable.Rows.Add(new object[] { "Ivanov", "" });
            dataTable.Rows.Add(new object[] { "Petrov","" });
            dataTable.Rows.Add(new object[] { "John Smith", "" });
            dataTable.Rows.Add(new object[] { "Ivanov", ""});
            dataTable.Rows.Add(new object[] { "Petrov", "" });
            dataTable.Rows.Add(new object[] { "John Smith", "" });
            dataTable.Rows.Add(new object[] { "Ivanov", "" });
            dataTable.Rows.Add(new object[] { "Petrov","" });
            dataTable.Rows.Add(new object[] { "John Smith", ""});
            gridControl1.DataSource = dataTable;
        }

       
        private void ConfigureRepositoryItemButtonEdit() {
            RepositoryItemFileEdit item  = SpecifyRepositoryItemFileEdit();
            gridControl1.RepositoryItems.Add(item);
            gridView1.Columns["File with information"].ColumnEdit = item;
            gridView1.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowAlways;
        }

        private RepositoryItemFileEdit SpecifyRepositoryItemFileEdit() {
            RepositoryItemFileEdit item = new RepositoryItemFileEdit();
            item.Buttons[0].Kind = ButtonPredefines.DropDown;
            item.Buttons.Add(new EditorButton(ButtonPredefines.Glyph));
            item.Buttons[1].Image = CustomEditor.Properties.Resources.open;
            item.Buttons[1].Tag = "OpenFileDialog";
            return item;
        }



    }
}
