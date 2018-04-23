Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports DevExpress.XtraEditors
Imports DevExpress.XtraEditors.Drawing
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.IO
Imports System.Drawing
Imports DevExpress.Utils.Menu
Imports DevExpress.LookAndFeel
Imports DevExpress.XtraEditors.Popup
Imports DevExpress.Utils

Namespace CustomEditor
	Friend Class FileEdit
		Inherits PopupContainerEdit
		Shared Sub New()
			RepositoryItemFileEdit.Register()
		End Sub

		Public Sub New()
			CreateContextMenu()

			AddHandler ToolTipController.DefaultController.GetActiveObjectInfo, AddressOf tooltip_GetActiveObjectInfo
		End Sub


		Private Sub tooltip_GetActiveObjectInfo(ByVal sender As Object, ByVal e As ToolTipControllerGetActiveObjectInfoEventArgs)
			If e.SelectedControl IsNot Me Then
				Return
			End If

			Dim info As ToolTipControlInfo = Nothing

			 Dim text As String = ""
			 If EditValue IsNot Nothing Then
				 text = EditValue.ToString()
				 info = New ToolTipControlInfo(text, text)
			 End If
			   If info IsNot Nothing Then
				   e.Info = info
			   End If

		End Sub

		Private defaultDirectory As String = "c:\"
		Private Sub InitializeOpenDialog()
			openFileDialog_Renamed.InitialDirectory = defaultDirectory
			openFileDialog_Renamed.Filter = "Text Files (*.txt)|*.txt|BMP Files (*.bmp)|*.bmp|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif|All files (*.*)|*.*"
		End Sub

		Protected Overrides Overloads Sub Dispose(ByVal disposing As Boolean)
			RemoveHandler ToolTipController.DefaultController.GetActiveObjectInfo, AddressOf tooltip_GetActiveObjectInfo
			MyBase.Dispose(disposing)
		End Sub

		 Private menu As DXPopupMenu
		Private Sub CreateContextMenu()
			menu= New DXPopupMenu()
			menu.MenuViewType = MenuViewType.Menu
			menu.Items.Add(New DXMenuItem("Open file", AddressOf OpenFile))
			menu.Items.Add(New DXMenuItem("Rename/Move to ... file", AddressOf RenameFile))
			menu.Items.Add(New DXMenuItem("Copy file", AddressOf CopyFile))
			menu.Items.Add(New DXMenuItem("Delete file", AddressOf DeleteFile))
			SetMenuItemEnabledMode(False)
		End Sub

		Private Sub SetMenuItemEnabledMode(ByVal key As Boolean)
			For Each m As DXMenuItem In menu.Items
				m.Enabled = key
			Next m
		End Sub

		Private skinMenuManager As SkinMenuManager
		Protected Overrides Sub OnMouseUp(ByVal e As MouseEventArgs)
			MyBase.OnMouseUp(e)
			If e.Button = MouseButtons.Right Then
				Dim lf As UserLookAndFeel = UserLookAndFeel.Default

				If skinMenuManager Is Nothing Then
					skinMenuManager = New SkinMenuManager(lf)
				End If

				CType(menu, IDXDropDownControl).Show(skinMenuManager, Me, e.Location)
			End If
		End Sub


		Private Sub RetrieveInformation(ByVal info As FileInfo)
			blobForm.InitializeFileInfo()
			blobForm.FillLabelContent(info)
			blobForm.DisplayOpenFilePreview()
		End Sub
		Protected Overrides Sub OnEditValueChanged()
			MyBase.OnEditValueChanged()
			If EditValue IsNot Nothing Then
				If EditValue.ToString() <> "" Then
					SetMenuItemEnabledMode(True)
					Dim info As FileInfo = GetFileInfo()
					If blobForm IsNot Nothing Then
						RetrieveInformation(info)
					End If
				End If

			End If

		End Sub

		Private Sub OpenFile(ByVal sender As Object, ByVal e As EventArgs)
			Try
				System.Diagnostics.Process.Start(GetFileName())
			Catch ex As Exception
				MessageBox.Show("Error: Could not read file from disk. Original error: " & ex.Message)
			End Try

		End Sub

		Private fileDialog As SaveFileDialog
		Private Sub RenameFile(ByVal sender As Object, ByVal e As EventArgs)
			Dim oldFileName As String = GetFileName()
			If fileDialog Is Nothing Then
				fileDialog = New SaveFileDialog()
			End If
			fileDialog.Title = "Rename/Move to ... "
			If GetOpenSaveDialogResult() Then
				File.Move(oldFileName, fileDialog.FileName)
			End If
		End Sub

		Private Sub SetFileEditValue(ByVal newFileName As String)
			Me.EditValue = newFileName


		End Sub
		Private Sub CopyFile(ByVal sender As Object, ByVal e As EventArgs)
			Dim oldFileName As String = GetFileName()
			If fileDialog Is Nothing Then
				fileDialog = New SaveFileDialog()
			End If
			fileDialog.Title = "Copy"
			If GetOpenSaveDialogResult() Then
				File.Copy(oldFileName, fileDialog.FileName, True)
			End If

		End Sub


		Private Function GetOpenSaveDialogResult() As Boolean
			Dim oldFileName As String = GetFileName()
			fileDialog.FileName = oldFileName
			fileDialog.DefaultExt = Path.GetExtension(oldFileName)
			If fileDialog.ShowDialog() = DialogResult.OK Then
				Dim newFileName As String = fileDialog.FileName
				fileDialog.AddExtension = True
				SetFileEditValue(newFileName)
				Return True
			End If
			Return False
		End Function



		Private Sub DeleteFile(ByVal sender As Object, ByVal e As EventArgs)
			Try
				Dim result As DialogResult = MessageBox.Show("Are you sure you want to delete this file?", "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Information)
				If result = DialogResult.Yes Then
					File.Delete(GetFileName())
					MessageBox.Show("File " & GetFileName() & " was successfully deleted!", "Information")
					Me.EditValue = Nothing
					SetMenuItemEnabledMode(False)
				End If
			Catch ex As Exception
				MessageBox.Show("Error: Could not delete file from disk. Original error: " & ex.Message)
			End Try

		End Sub


		Public Overrides ReadOnly Property EditorTypeName() As String
			Get
				Return RepositoryItemFileEdit.FileEditName
			End Get
		End Property

		<DesignerSerializationVisibility(DesignerSerializationVisibility.Content)> _
		Public Shadows ReadOnly Property Properties() As RepositoryItemFileEdit
			Get
				Return TryCast(MyBase.Properties, RepositoryItemFileEdit)
			End Get
		End Property


		Protected Overrides Sub OnClickButton(ByVal buttonInfo As EditorButtonObjectInfoArgs)
			MyBase.OnClickButton(buttonInfo)
			If buttonInfo.Button.Tag IsNot Nothing Then
				If buttonInfo.Button.Tag.ToString() = "OpenFileDialog" Then
					OpenFileDialog()
				End If
			End If

		End Sub

		Protected Friend Function GetFileName() As String
			Return If((EditValue Is Nothing), defaultDirectory, Me.EditValue.ToString())
		End Function

		Private openFileDialog_Renamed As OpenFileDialog
		Private Sub OpenFileDialog()
			If openFileDialog_Renamed Is Nothing Then
				openFileDialog_Renamed = New OpenFileDialog()
				InitializeOpenDialog()
			End If

			openFileDialog_Renamed.InitialDirectory = GetFileName()
			If openFileDialog_Renamed.ShowDialog() = DialogResult.OK Then
				Dim fileName As String = openFileDialog_Renamed.FileName
				SetFileEditValue(fileName)
			End If
		End Sub


		Private Function GetFileInfo() As FileInfo
			Return New FileInfo(GetFileName())
		End Function


		Protected Overrides Function CalcPopupFormSize() As Size
			If EditValue IsNot Nothing Then
				If EditValue.ToString() = "" Then
					blobForm.ClearPopupContainerContent()
				End If
			End If

			Return blobForm.CalcBlobPopupFormSize()
		End Function
		Private blobForm As CustomPopupForm
		Protected Overrides Function GetPopupForm() As PopupBaseForm
			If blobForm Is Nothing Then
				blobForm = New CustomPopupForm(Me)
				If EditValue IsNot Nothing Then
					If EditValue.ToString() <> "" Then
						RetrieveInformation(GetFileInfo())
					Else
						blobForm.ClearPopupContainerContent()
					End If
				End If
			End If
			Return blobForm

		End Function

		Protected Overrides ReadOnly Property PopupForm() As PopupBaseForm
			Get
				Return blobForm
			End Get
		End Property

	End Class

End Namespace
