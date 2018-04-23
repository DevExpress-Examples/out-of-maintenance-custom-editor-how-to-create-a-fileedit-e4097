using System;
using System.Collections.Generic;
using System.Linq;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Drawing;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;
using System.Drawing;
using DevExpress.Utils.Menu;
using DevExpress.LookAndFeel;
using DevExpress.XtraEditors.Popup;
using DevExpress.Utils;

namespace CustomEditor {
    class FileEdit:PopupContainerEdit {
        static FileEdit() {
            RepositoryItemFileEdit.Register();
        }

        public FileEdit() {
            CreateContextMenu();

            ToolTipController.DefaultController.GetActiveObjectInfo += new ToolTipControllerGetActiveObjectInfoEventHandler(tooltip_GetActiveObjectInfo);
        }

       
        void tooltip_GetActiveObjectInfo(object sender, ToolTipControllerGetActiveObjectInfoEventArgs e) {
            if (e.SelectedControl != this) return;

            ToolTipControlInfo info = null;
            
             string text = "";
             if (EditValue != null) {
                 text = EditValue.ToString();
                 info = new ToolTipControlInfo(text, text);
             }
               if (info != null)
                   e.Info = info;

        }

        string defaultDirectory = "c:\\";
        private void InitializeOpenDialog() {
            openFileDialog.InitialDirectory = defaultDirectory;
            openFileDialog.Filter = "Text Files (*.txt)|*.txt|BMP Files (*.bmp)|*.bmp|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif|All files (*.*)|*.*";
        }

        protected override void Dispose(bool disposing) {
            ToolTipController.DefaultController.GetActiveObjectInfo -= new ToolTipControllerGetActiveObjectInfoEventHandler(tooltip_GetActiveObjectInfo);
            base.Dispose(disposing);
        }
        
         DXPopupMenu menu;
        private void CreateContextMenu() {
            menu= new DXPopupMenu();
            menu.MenuViewType = MenuViewType.Menu;
            menu.Items.Add(new DXMenuItem("Open file", OpenFile));
            menu.Items.Add(new DXMenuItem("Rename/Move to ... file", RenameFile));
            menu.Items.Add(new DXMenuItem("Copy file", CopyFile));
            menu.Items.Add(new DXMenuItem("Delete file", DeleteFile));
            SetMenuItemEnabledMode(false);
        }

        private void SetMenuItemEnabledMode(bool key) {
            foreach (DXMenuItem m in menu.Items)
                m.Enabled = key;
        }

        SkinMenuManager skinMenuManager;
        protected override void OnMouseUp(MouseEventArgs e) {
            base.OnMouseUp(e);
            if(e.Button == MouseButtons.Right) {
                UserLookAndFeel lf = UserLookAndFeel.Default;
                 
                if (skinMenuManager == null)
                    skinMenuManager = new SkinMenuManager(lf);

                ((IDXDropDownControl)menu).Show(skinMenuManager, this, e.Location);
            }
        }

        
        private void RetrieveInformation(FileInfo info) {
            blobForm.InitializeFileInfo();
            blobForm.FillLabelContent(info);
            blobForm.DisplayOpenFilePreview();
        }
        protected override void OnEditValueChanged() {
            base.OnEditValueChanged();
            if (EditValue != null) {
                if (EditValue.ToString() != "") {
                    SetMenuItemEnabledMode(true);
                    FileInfo info = GetFileInfo();
                    if (blobForm != null) {
                        RetrieveInformation(info);
                    }
                }

            }

        }

        private void OpenFile(object sender, EventArgs e) {
            try {
                System.Diagnostics.Process.Start(GetFileName());
            }
            catch (Exception ex) {
                MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
            }

        }

        SaveFileDialog fileDialog;
        private void RenameFile(object sender, EventArgs e) {
            string oldFileName = GetFileName();
            if (fileDialog == null)
                fileDialog = new SaveFileDialog();
            fileDialog.Title = "Rename/Move to ... ";
            if ( GetOpenSaveDialogResult())
                File.Move(oldFileName, fileDialog.FileName);
        }

        private void SetFileEditValue(string newFileName) {
            this.EditValue = newFileName;
           
           
        }
        private void CopyFile(object sender, EventArgs e) {
            string oldFileName = GetFileName();
            if (fileDialog == null)
                fileDialog = new SaveFileDialog();
            fileDialog.Title = "Copy";
            if ( GetOpenSaveDialogResult())
                File.Copy(oldFileName, fileDialog.FileName, true);

        }


        private bool GetOpenSaveDialogResult() {
            string oldFileName = GetFileName();
            fileDialog.FileName = oldFileName;
            fileDialog.DefaultExt = Path.GetExtension(oldFileName);
            if (fileDialog.ShowDialog() == DialogResult.OK) {
                string newFileName = fileDialog.FileName;
                fileDialog.AddExtension = true;
                SetFileEditValue(newFileName);
                return true;
            }
            return false;
        }

 

        private void DeleteFile(object sender, EventArgs e) {
            try {
                DialogResult result = MessageBox.Show("Are you sure you want to delete this file?", "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (result == DialogResult.Yes){
                    File.Delete(GetFileName());
                    MessageBox.Show("File " + GetFileName() + " was successfully deleted!", "Information");
                    this.EditValue = null;
                    SetMenuItemEnabledMode(false);
                }
            }
            catch (Exception ex) {
                MessageBox.Show("Error: Could not delete file from disk. Original error: " + ex.Message);
            }

        }


        public override string EditorTypeName {
            get {
                return RepositoryItemFileEdit.FileEditName;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public new RepositoryItemFileEdit Properties {
            get { return base.Properties as RepositoryItemFileEdit; }
        }


        protected override void OnClickButton(EditorButtonObjectInfoArgs buttonInfo) {
            base.OnClickButton(buttonInfo);
            if (buttonInfo.Button.Tag != null) {
                if (buttonInfo.Button.Tag.ToString() == "OpenFileDialog")
                    OpenFileDialog();
            }
           
        }

        protected internal string GetFileName() {
            return (EditValue == null) ? defaultDirectory : this.EditValue.ToString();
        }

        OpenFileDialog openFileDialog;
        private void OpenFileDialog() {
            if (openFileDialog == null) {
                openFileDialog = new OpenFileDialog();
                InitializeOpenDialog();
            }

            openFileDialog.InitialDirectory = GetFileName();
            if (openFileDialog.ShowDialog() == DialogResult.OK) {
                string fileName = openFileDialog.FileName;
                SetFileEditValue(fileName);
            }
        }

  
        private FileInfo GetFileInfo() {
            return  new FileInfo(GetFileName());
        }


        protected override Size CalcPopupFormSize() {
            if (EditValue != null)
                if (EditValue.ToString() == "")
                    blobForm.ClearPopupContainerContent();

            return blobForm.CalcBlobPopupFormSize();
        }
        CustomPopupForm blobForm;
        protected override PopupBaseForm GetPopupForm() {
            if (blobForm == null) {
                blobForm = new CustomPopupForm(this);
                if (EditValue != null) {
                    if (EditValue.ToString() != "" )
                        RetrieveInformation(GetFileInfo());
                    else
                        blobForm.ClearPopupContainerContent();
                }
            }
            return blobForm;

        }

        protected override PopupBaseForm PopupForm {
            get {
                return blobForm;
            }
        }

    }
  
}
