Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports DevExpress.XtraEditors
Imports System.Windows.Forms
Imports DevExpress.XtraEditors.Controls
Imports System.IO
Imports System.Drawing
Imports DevExpress.XtraEditors.Popup

Namespace CustomEditor

	Friend Class CustomPopupForm
		Inherits PopupContainerForm
		Private ownerEdit_Renamed As PopupContainerEdit
		Public Sub New(ByVal ownerEdit As PopupContainerEdit)
			MyBase.New(ownerEdit)
			Me.ownerEdit_Renamed = ownerEdit
			Initialize()
		End Sub


		Public Shadows ReadOnly Property OwnerEdit() As FileEdit
			Get
				Return TryCast(ownerEdit_Renamed, FileEdit)
			End Get
		End Property

		Private labelModifideDate As LabelControl
		Private labelCreationDate As LabelControl
		Private labelFileSize As LabelControl
		Private labelLastAccessDate As LabelControl
		Private labelLocation As LabelControl
		Private Sub Initialize()
			labelCreationDate = New LabelControl()
			labelModifideDate = New LabelControl()
			labelFileSize = New LabelControl()
			labelLocation = New LabelControl()
			labelLastAccessDate = New LabelControl()

			InitializeLabels()
			PopupControl.Controls.AddRange(New Control() { labelCreationDate, labelModifideDate, labelFileSize, labelLastAccessDate, labelLocation })

			Me.Width = MyBase.MinFormSize.Width
			IsImageLoaded = False
			InitializeFileInfo()

		End Sub

		Public Sub InitializeFileInfo()
			labelCreationDate.Text = "File created: "
			labelModifideDate.Text = "File modified: "
			labelFileSize.Text = "File size: "
			labelLastAccessDate.Text = "File accessed: "
			labelLocation.Text = "File location: "
		End Sub

		Private pictureEdit As PictureEdit
		Private privateIsImageLoaded As Boolean
		Public Property IsImageLoaded() As Boolean
			Get
				Return privateIsImageLoaded
			End Get
			Set(ByVal value As Boolean)
				privateIsImageLoaded = value
			End Set
		End Property
		Public Sub DisplayOpenFilePreview()
			Try
				If pictureEdit Is Nothing Then
					pictureEdit = New PictureEdit()
					pictureEdit.Dock = DockStyle.Bottom
					pictureEdit.Properties.SizeMode = PictureSizeMode.StretchVertical
					PopupControl.Controls.Add(pictureEdit)
				End If
				pictureEdit.Image = Image.FromFile(Me.OwnerEdit.GetFileName())
				pictureEdit.Show()
				IsImageLoaded = True
			Catch e1 As Exception
				If pictureEdit IsNot Nothing Then
					pictureEdit.Hide()
				End If
				IsImageLoaded = False
			End Try


		End Sub

		Public Sub ClearPopupContainerContent()
			InitializeFileInfo()
			If pictureEdit IsNot Nothing Then
				pictureEdit.Hide()
			End If
			IsImageLoaded = False
		End Sub


		Private Function CalcLabelsHeight() As Integer
			Return labelLocation.Height + labelCreationDate.Height + labelFileSize.Height + labelLastAccessDate.Height + labelModifideDate.Height
		End Function

		Public Function CalcPictureEditHeight() As Integer
			Return pictureEdit.Height
		End Function

		Public Sub FillLabelContent(ByVal file As FileInfo)
			labelFileSize.Text += file.Length & " byte"
			labelCreationDate.Text += file.CreationTime
			labelModifideDate.Text += file.LastWriteTime
			labelLastAccessDate.Text += file.LastAccessTime
			labelLocation.Text += file.DirectoryName
		End Sub

		Private Sub InitializeLabels()
			SetLabelProperties(labelFileSize)
			SetLabelProperties(labelCreationDate)
			SetLabelProperties(labelModifideDate)
			SetLabelProperties(labelLastAccessDate)
			SetLabelProperties(labelLocation)
		End Sub

		Private Sub SetLabelProperties(ByVal label As LabelControl)
			label.AutoSizeMode = LabelAutoSizeMode.Vertical
			label.Width = Width
			label.Dock = DockStyle.Top
		End Sub


		Public Function CalcBlobPopupFormSize() As Size
			Dim offset As Integer = 2
			Dim controlHeight As Integer = MyBase.DefaultMinFormSize.Height

			Dim resultSize As New Size()
			resultSize.Height = CalcLabelsHeight() + controlHeight + offset * 2
			resultSize.Width = Width

			If IsImageLoaded Then
				resultSize.Height += CalcPictureEditHeight() + offset * 2
			End If

			Me.ClientSize = resultSize
			Return resultSize
		End Function

	End Class
End Namespace
