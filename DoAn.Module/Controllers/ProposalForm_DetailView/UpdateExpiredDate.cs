using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.XtraRichEdit.Fields;
using DoAn.Module.BusinessObjects.Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoAn.Module.Controllers.ProposalForm_DetailView { 
	
	public class UpdateExpiredDate : ViewController
    {
        public UpdateExpiredDate()
        {
            TargetViewId = "ProposalForm_DetailView";
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            
            SaveAction_ExecuteCompleted();
            
            //UpdateDate();
        }

        private void SaveAction_ExecuteCompleted()
        {
            ProposalForm proposalForm = View.CurrentObject as ProposalForm;
            DateTime createdDate = proposalForm.Date;
            DateTime tempDate = proposalForm.Date;
            List<DateTime> tempList = new List<DateTime>();
            foreach(var process in proposalForm.templateform.ApprovalProcesses)
            {
                var temp=tempDate.AddDays(process.DayLeft);
                tempList.Add(temp);
                tempDate = temp;
            }
            int i = 0;
            foreach(ProposalApproval proposalApproval in proposalForm.ProposalApprovals)
            {
                proposalApproval.DueDate = tempList[proposalApproval.Step - 1];
            }
        }


    }

}