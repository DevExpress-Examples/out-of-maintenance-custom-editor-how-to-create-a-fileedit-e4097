using System;
using System.Collections.Generic;
using System.Linq;
using DevExpress.XtraEditors;
using System.Windows.Forms;
using DevExpress.XtraEditors.Controls;
using System.IO;
using System.Drawing;
using DevExpress.XtraEditors.Popup;

namespace CustomEditor {

    class CustomPopupForm : PopupContainerForm {
        PopupContainerEdit ownerEdit;
        public CustomPopupForm(PopupContainerEdit ownerEdit)
            : base(ownerEdit) {
            this.ownerEdit = ownerEdit;
            Initialize();
        }


        public new FileEdit OwnerEdit { get { return ownerEdit as FileEdit; } }

        private LabelControl labelModifideDate;
        private LabelControl labelCreationDate;
        private LabelControl labelFileSize;
        private LabelControl labelLastAccessDate;
        private LabelControl labelLocation;
        private void Initialize() {
            labelCreationDate = new LabelControl();
            labelModifideDate = new LabelControl();
            labelFileSize = new LabelControl();
            labelLocation = new LabelControl();
            labelLastAccessDate = new LabelControl();

            InitializeLabels();
            PopupControl.Controls.AddRange(new Control[] { labelCreationDate, labelModifideDate, labelFileSize, labelLastAccessDate, labelLocation });

            this.Width = base.MinFormSize.Width;
            IsImageLoaded = false;
            InitializeFileInfo();

        }

        public void InitializeFileInfo() {
            labelCreationDate.Text = "File created: ";
            labelModifideDate.Text = "File modified: ";
            labelFileSize.Text = "File size: ";
            labelLastAccessDate.Text = "File accessed: ";
            labelLocation.Text = "File location: ";
        }

        PictureEdit pictureEdit;
        public bool IsImageLoaded { get; set; }
        public void DisplayOpenFilePreview() {
            try {
                if (pictureEdit == null) {
                    pictureEdit = new PictureEdit();
                    pictureEdit.Dock = DockStyle.Bottom;
                    pictureEdit.Properties.SizeMode = PictureSizeMode.StretchVertical;
                    PopupControl.Controls.Add(pictureEdit);
                }
                pictureEdit.Image = Image.FromFile(this.OwnerEdit.GetFileName());
                pictureEdit.Show();
                IsImageLoaded = true;
            }
            catch (Exception) {
                if (pictureEdit != null)
                    pictureEdit.Hide();
                IsImageLoaded = false;
            }


        }

        public void ClearPopupContainerContent() {
            InitializeFileInfo();
            if (pictureEdit != null)
                pictureEdit.Hide();
            IsImageLoaded = false;
        }


        private int CalcLabelsHeight() {
            return labelLocation.Height + labelCreationDate.Height + labelFileSize.Height + labelLastAccessDate.Height + labelModifideDate.Height;
        }

        public int CalcPictureEditHeight() {
            return pictureEdit.Height;
        }

        public void FillLabelContent(FileInfo file) {
            labelFileSize.Text += file.Length + " byte";
            labelCreationDate.Text += file.CreationTime;
            labelModifideDate.Text += file.LastWriteTime;
            labelLastAccessDate.Text += file.LastAccessTime;
            labelLocation.Text += file.DirectoryName;
        }

        private void InitializeLabels() {
            SetLabelProperties(labelFileSize);
            SetLabelProperties(labelCreationDate);
            SetLabelProperties(labelModifideDate);
            SetLabelProperties(labelLastAccessDate);
            SetLabelProperties(labelLocation);
        }

        private void SetLabelProperties(LabelControl label) {
            label.AutoSizeMode = LabelAutoSizeMode.Vertical;
            label.Width = Width;
            label.Dock = DockStyle.Top;
        }


        public Size CalcBlobPopupFormSize() {
            int offset = 2;
            int controlHeight = base.DefaultMinFormSize.Height;

            Size resultSize = new Size();
            resultSize.Height = CalcLabelsHeight() + controlHeight + offset * 2;
            resultSize.Width = Width;

            if (IsImageLoaded) {
                resultSize.Height += CalcPictureEditHeight() + offset * 2;
            }

            this.ClientSize = resultSize;
            return resultSize;
        }

    }
}
