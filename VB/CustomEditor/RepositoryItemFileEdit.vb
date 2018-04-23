Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports DevExpress.XtraEditors
Imports DevExpress.XtraEditors.Repository
Imports DevExpress.XtraEditors.Registrator
Imports DevExpress.XtraEditors.ViewInfo
Imports DevExpress.XtraEditors.Drawing
Imports System.IO
Namespace CustomEditor
	<UserRepositoryItem("Register")> _
	Public Class RepositoryItemFileEdit
		Inherits RepositoryItemPopupContainerEdit
		Shared Sub New()
			Register()
		End Sub

		Public Sub New()
		End Sub


		Public Overrides Property PopupControl() As PopupContainerControl
			Get
				If customPopupControl Is Nothing Then
					customPopupControl = New PopupContainerControl()
				End If
				Return customPopupControl
			End Get
			Set(ByVal value As PopupContainerControl)

				customPopupControl = value
			End Set
		End Property


		Private customPopupControl As PopupContainerControl


		Public Const FileEditName As String = "FileEdit"

		Public Overrides ReadOnly Property EditorTypeName() As String
			Get
				Return FileEditName
			End Get
		End Property


		Public Overrides Function GetDisplayText(ByVal format As DevExpress.Utils.FormatInfo, ByVal editValue As Object) As String
			Return If(editValue IsNot Nothing, Path.GetFileName(editValue.ToString()), "")
		End Function


		Public Shared Sub Register()
            EditorRegistrationInfo.Default.Editors.Add(New EditorClassInfo(FileEditName, GetType(FileEdit), GetType(RepositoryItemFileEdit), GetType(PopupContainerEditViewInfo), New ButtonEditPainter(), True, 0, GetType(DevExpress.Accessibility.PopupEditAccessible)))
		End Sub
	End Class
End Namespace
