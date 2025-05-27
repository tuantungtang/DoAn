using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Notifications;
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
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Notifications;
using DevExpress.Persistent.Base.General;

using static DoAn.Module.BusinessObjects.Class.Define;
using DoAn.Module.Controllers.Notification;
using System.Security.Cryptography;
namespace DoAn.Module.Controllers.ProposalApproval_DetailView
{
    public class StatusController : ViewController
    {
        public StatusController()
        {
            TargetViewId = "ProposalApproval_DetailView";
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            ModificationsController controller = Frame.GetController<ModificationsController>();
            if (controller != null)
            {
                controller.SaveAction.ExecuteCompleted += SaveAction_ExecuteCompleted;
            }
            if (View is DetailView detailView)
            {
                var obj = View.CurrentObject as ProposalApproval;
                ApplicationUser user = GetCurrentNhanvien();
                int curStep = obj.Step;
                int curStatus = obj.proposalform.Status;
                var state = obj.proposalform.Daxong;
                if (curStep-curStatus!=1 || state==true)
                {
                    View.AllowEdit["ReadOnly"] = false;
                }
                else
                {
                    View.AllowEdit["ReadOnly"] = true;
                }
                

                //View.CurrentObjectChanged += SaveAction_ExecuteCompleted; 
            }
        }

    

        private void SaveAction_ExecuteCompleted(object sender, DevExpress.ExpressApp.Actions.ActionBaseEventArgs e)
        {
            
            if (View.CurrentObject is ProposalApproval currentApproval)
            {
                if (currentApproval.State == EStatusDuyet.daduyet)
                {
                    UpdateProposalFormStatus(currentApproval);
                }
                if (currentApproval.State == EStatusDuyet.huy)
                {
                    DeleteProposalForStatus(currentApproval);
                }
            }
        }

        private void DeleteProposalForStatus(ProposalApproval currentApproval)
        {
            IObjectSpace objectSpace = Application.CreateObjectSpace(typeof(ProposalForm));
            ProposalForm proposalForm = objectSpace.GetObjectByKey<ProposalForm>(currentApproval.proposalform.Oid);
            proposalForm.Status =- 1;
            proposalForm.State = EStatusVB.dahuy;
            foreach(ProposalApproval proposalApproval in proposalForm.ProposalApprovals)
            {

                if (proposalApproval.State == EStatusDuyet.choduyet)
                {
                    proposalApproval.State = EStatusDuyet.huy;
                }
            }
            
            Session session = proposalForm.Session;
            CreateNotification(objectSpace, proposalForm, session);
            objectSpace.CommitChanges();
        }

        //add notifications for cancel/approved forms
        private void CreateNotification(IObjectSpace objectSpace,ProposalForm proposalForm,Session session)
        {
            Notifications notification = ObjectSpace.CreateObject<Notifications>();
            foreach (Sharing share in proposalForm.Sharings)
            {
                if (share.user != null)
                    notification.user = ObjectSpace.GetObjectByKey<ApplicationUser>(share.user.Oid);
            }
            if (proposalForm.user != null)
            {
                notification.user = ObjectSpace.GetObjectByKey<ApplicationUser>(proposalForm.user.Oid);
            }
                
            notification.proposalForms.Add(ObjectSpace.GetObjectByKey<ProposalForm>(proposalForm.Oid));
            string vbState;
            string name = proposalForm.Name;
            if (proposalForm.State == Define.EStatusVB.daduyet)
            {
                vbState = "approved";
                ExportToPDF.Export(proposalForm);
            }
            else
            {
                vbState = "denied";
            }
            proposalForm.Daxong = true;
                notification.NotificationString = name+" have been " + vbState;
            ObjectSpace.CommitChanges();
        }
        //notificate for user a form that on their approve
        private void NotificationUser(IObjectSpace objectSpace,ProposalForm proposalForm,ProposalApproval proposalApproval,Session session)
        {
            int step_now = proposalApproval.Step;
            
            ApplicationUser user_add = proposalForm.ProposalApprovals[step_now-1].user;
            
            foreach (ProposalApproval proposalApproval1 in proposalForm.ProposalApprovals)
            {
                if (proposalApproval1.Step == step_now+1)
                {
                    user_add = proposalApproval1.user;

                }
            }
            Execute execute_role = user_add.chucdanh;
            string name = proposalForm.Name;
            string text = name+" need your approval";
            foreach (ApplicationUser applicationUser in execute_role.Users)
            {
                Notifications notification2 = ObjectSpace.CreateObject<Notifications>();
                notification2.NotificationString = text;
                notification2.proposalForms.Add(proposalForm);
                notification2.notiTime = DateTime.Now;
                notification2.user = applicationUser;
                ObjectSpace.CommitChanges();
            }
        }
        private void UpdateProposalFormStatus(ProposalApproval currentApproval)
        {
            IObjectSpace objectSpace = Application.CreateObjectSpace(typeof(ProposalForm));
            ProposalForm proposalForm = ObjectSpace.GetObjectByKey<ProposalForm>(currentApproval.proposalform.Oid);
            foreach(ProposalApproval proposalApproval in proposalForm.ProposalApprovals)
            {
                ApplicationUser nv = Define.GetCurrentNhanvien();
                if(proposalApproval.user == nv)
                {
                    proposalApproval.ApprovalDate = DateTime.Now;

                }
            }
            Session session = proposalForm.Session;
            //ApplicationUser temp= Define.GetCurrentNhanvien();

            //proposalForm.approved.Add(temp.Oid.ToString());
            //this.ObjectSpace.CommitChanges();

                NotificationUser(objectSpace, proposalForm, currentApproval, session);


            
            proposalForm.Status += 1;
            
            if (proposalForm.Status == proposalForm.ProposalApprovals.Count)
            {
                proposalForm.State = EStatusVB.daduyet;
                proposalForm.Ngayduyet = System.DateTime.Now;
            }
            

            if (proposalForm.Status == proposalForm.ProposalApprovals.Count || proposalForm.State == EStatusVB.dahuy)
            {
                CreateNotification(objectSpace, proposalForm, session);
            }

            ObjectSpace.CommitChanges();
            
            Application.ShowViewStrategy.ShowMessage("Update success ", InformationType.Info);
        }
        
    }
}
