//if form is expired, lock it
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Utils;
using DevExpress.Xpo;
using DevExpress.XtraRichEdit.Model.History;
using DoAn.Module.BusinessObjects.Authentication;
using DoAn.Module.BusinessObjects.Class;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoAn.Module.Controllers.Global
{
    public class LockExpiredForms: ViewController
    {
        public LockExpiredForms()
        {
            TargetViewId = "ProposalForm_DetailView";
            TargetViewId = "PropsalApproval_DetailView";
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            if (View.Id == "ProposalForm_DetailView")
            {
                ProposalForm proposalForm = View.CurrentObject as ProposalForm;
                LockForm(proposalForm);
            }
            else
            {
                ProposalApproval proposalApproval = View.CurrentObject as ProposalApproval;
                ProposalForm proposalForm = proposalApproval.proposalform;
                LockForm(proposalForm);
            }
        }

        private void LockForm(ProposalForm proposalForm)
        {
            
            DateTime maxdate;
            List<DateTime> temps = new List<DateTime>();
            foreach(ProposalApproval proposalApproval in proposalForm.ProposalApprovals)
            {
                temps.Add(proposalApproval.DueDate);
            }
            maxdate = temps.Max();
            if (maxdate > DateTime.Now)
            {
                proposalForm.State = Define.EStatusVB.dahuy;
            }
        }
    }
}
