using System;
using System.Collections.Generic;
using System.Linq;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors.Registrator;
using DevExpress.XtraEditors.ViewInfo;
using DevExpress.XtraEditors.Drawing;
using System.IO;
namespace CustomEditor {
    [UserRepositoryItem("Register")]
    public class RepositoryItemFileEdit : RepositoryItemPopupContainerEdit {
        static RepositoryItemFileEdit() {
            Register();
        }

        public RepositoryItemFileEdit() {
        }


        public override PopupContainerControl PopupControl {
            get {
                if (customPopupControl == null)
                    customPopupControl = new PopupContainerControl();
                return customPopupControl;
            }
            set {

                customPopupControl = value;
            }
        }


        private PopupContainerControl customPopupControl;
        

        public const string FileEditName = "FileEdit";

        public override string EditorTypeName { get { return FileEditName; } }

 
        public override string GetDisplayText(DevExpress.Utils.FormatInfo format, object editValue) {
            return editValue!= null ? Path.GetFileName(editValue.ToString()) : "";
        }

  
        public static void Register() {
            EditorRegistrationInfo.Default.Editors.Add(new EditorClassInfo(FileEditName, typeof(FileEdit),
                typeof(RepositoryItemFileEdit), typeof(PopupContainerEditViewInfo),
                new ButtonEditPainter(), true, null, typeof(DevExpress.Accessibility.PopupEditAccessible)));
        }
    }
}
