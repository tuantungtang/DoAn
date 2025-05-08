using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Editors;
using DoAn.Module.BusinessObjects.Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.XtraEditors;
using DevExpress.XtraPrinting.Native.RichText;

namespace DoAn.Module.Controllers
{
    public class ShowFormInApprove : ViewController
    {
        protected override void OnActivated()
        {
            base.OnActivated();
            TargetViewId = "ProposalApproval_DetailView";
            //if (View is DetailView detailView)
            //{
            //    // Find the PropertyEditor for the RTF content property
            //    var rtfEditor = detailView.FindItem("RtfContent") as PropertyEditor;

            //    if (rtfEditor != null)
            //    {
            //        // Get the RichTextEdit control from the PropertyEditor
            //        var richTextControl = rtfEditor.Control as RichTextEdit;

            //        if (richTextControl != null)
            //        {
            //            // Fetch the RTF content from the business object
            //            var businessObject = (ProposalForm)detailView.CurrentObject;
            //            string rtfText = businessObject?.RtfContent;

            //            // Set the RTF content in the RichTextEdit control
            //            if (!string.IsNullOrEmpty(rtfText))
            //            {
            //                richTextControl.RtfText = rtfText;
            //            }

            //            // Optionally, set the RichTextEdit to read-only
            //            richTextControl.Properties.ReadOnly = true;
            //        }
            //    }
            //}
        }
    }
}
