///if form have been approve or decline, make only
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.XtraRichEdit.Model.History;
using DoAn.Module.BusinessObjects.Authentication;
using DoAn.Module.BusinessObjects.Class;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoAn.Module.Controllers.ProposalForm_DetailView
{
    public class DeclineCondition : ViewController
    {
        public DeclineCondition()
        {
            TargetViewId = "ProposalForm_DetailView";
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            lockForm();
        }
        private void lockForm()
        {
            if (View is DetailView detailView)
            {
                var obj = detailView.CurrentObject as ProposalForm;
                if (obj.State.Equals(Define.EStatusVB.dahuy) || obj.State.Equals(Define.EStatusVB.daduyet))
                {
                    View.AllowEdit["ReadOnly"] = false;
                }

            }
        }
    }
}
