Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.Linq
Imports DevExpress.XtraEditors
Imports DevExpress.XtraEditors.Controls

Namespace CustomEditor
	Partial Public Class Form1
		Inherits XtraForm
		Public Sub New()
			InitializeComponent()
			CreateDataSource()
			ConfigureRepositoryItemButtonEdit()

		End Sub

		Private Sub CreateDataSource()
			Dim dataTable As New DataTable()
			dataTable.Columns.Add("Name", GetType(String))
			dataTable.Columns.Add("File with information", GetType(String))
			dataTable.Rows.Add(New Object() { "John Smith", "" })
			dataTable.Rows.Add(New Object() { "Ivanov", "" })
			dataTable.Rows.Add(New Object() { "Petrov","" })
			dataTable.Rows.Add(New Object() { "John Smith", "" })
			dataTable.Rows.Add(New Object() { "Ivanov", ""})
			dataTable.Rows.Add(New Object() { "Petrov", "" })
			dataTable.Rows.Add(New Object() { "John Smith", "" })
			dataTable.Rows.Add(New Object() { "Ivanov", "" })
			dataTable.Rows.Add(New Object() { "Petrov","" })
			dataTable.Rows.Add(New Object() { "John Smith", ""})
			gridControl1.DataSource = dataTable
		End Sub


		Private Sub ConfigureRepositoryItemButtonEdit()
			Dim item As RepositoryItemFileEdit = SpecifyRepositoryItemFileEdit()
			gridControl1.RepositoryItems.Add(item)
			gridView1.Columns("File with information").ColumnEdit = item
			gridView1.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowAlways
		End Sub

		Private Function SpecifyRepositoryItemFileEdit() As RepositoryItemFileEdit
			Dim item As New RepositoryItemFileEdit()
			item.Buttons(0).Kind = ButtonPredefines.DropDown
			item.Buttons.Add(New EditorButton(ButtonPredefines.Glyph))
			item.Buttons(1).Image = My.Resources.open
			item.Buttons(1).Tag = "OpenFileDialog"
			Return item
		End Function



	End Class
End Namespace
