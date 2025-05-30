//send button
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Xpo.DB;
using DevExpress.Xpo;
using DoAn.Module.BusinessObjects.Authentication;
using DoAn.Module.BusinessObjects.Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoAn.Module.Controllers.Notification;
using DevExpress.CodeParser;
using System.Security.AccessControl;

namespace DoAn.Module.Controllers.ProposalForm_DetailView
{
    public class ProposalAprovalController : ViewController
    {
        private readonly SimpleAction GuiDX;
        private readonly SimpleAction DuyetDX;
        private ProposalApproval CurrentBuocDuyet = null;
        private ProposalForm CurPhieu;
        private ApplicationUser curNV;

        public ProposalAprovalController()
        {
            TargetViewId = "ProposalForm_DetailView";
            GuiDX = new(this, "GuiDX", "View")
            {
                Caption = "Send",
                ImageName = "gui",
                TargetViewId = "ProposalForm_DetailView",
                ToolTip = "Gửi hoặc hủy Đề Xuất"
            };
            GuiDX.Execute += GuiDX_Execute;

        }

        private void GuiDX_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            if (CurPhieu != null)
            {
                ProposalForm p = ObjectSpace.GetObject(CurPhieu);
                if (p != null)
                {
                    if (p.State == Define.EStatusVB.choduyet)
                    {
                        string msg = p.Dagui ? "Are you sure you want to cancel this offer?" : "Are you sure you want to submit this proposal?";
                        NonPersistentObjectSpace objectSpace =
                            (NonPersistentObjectSpace)Application.CreateObjectSpace(typeof(ConfirmationWindowParameters));
                        ConfirmationWindowParameters parameters = new()
                        {
                            ConfirmationMessage = msg
                        };
                        DetailView confirmationDetailView = Application.CreateDetailView(objectSpace, parameters);
                        confirmationDetailView.Caption = "Confirm";

                        Application.ShowViewStrategy.ShowViewInPopupWindow(confirmationDetailView, OkDelegate);

                    }
                    else
                        Define.CustomError("Proposal is being processed");
                }
            }
        }

        private void sendNoti()
        {
            //IObjectSpace objectSpace = Application.CreateObjectSpace(typeof(Notifications));
            Notifications notifications = ObjectSpace.CreateObject<Notifications>();
            ProposalForm proposal = ObjectSpace.GetObject(CurPhieu);
            Session session = proposal.Session;
            ApplicationUser applicationUser = ObjectSpace.GetObjectByKey<ApplicationUser>(proposal.ProposalApprovals[0].user.Oid);
            foreach(ProposalApproval proposalApproval in proposal.ProposalApprovals)
            {
                if (proposalApproval.Step == 1)
                {
                    applicationUser = proposalApproval.user;
                }
            }
            Execute execute = applicationUser.chucdanh;
            foreach(ApplicationUser user in execute.Users)
            {
                notifications.user = user;
                notifications.proposalForms.Add(proposal);
                string form_name = proposal.Name;
                notifications.NotificationString = form_name+" need your approval";
                ObjectSpace.CommitChanges();

            }
            
        }

        private void OkDelegate()
        {
            CurPhieu.Dagui = !CurPhieu.Dagui;
            ObjectSpace.CommitChanges();
            string msg = CurPhieu.Dagui ? "You send the Proposal" : "You have canceled the proposal";
            Application.ShowViewStrategy.ShowMessage(msg);
            Define.SendEmailDexuat(CurPhieu);
            if (CurPhieu.Dagui)
            {
                GuiDX.Caption = "Cancel";
                sendNoti();
            }
            else
                GuiDX.Caption = "Send";
        }



        protected override void OnActivated()
        {
            base.OnActivated();
            bool DcDuyet = false;
            curNV = Define.GetCurrentNhanvien();
            //UpdateDuyetDXVisibility();
            if (curNV != null)
            {
                CurPhieu = ObjectSpace.FindObject<ProposalForm>(CriteriaOperator.Parse("Oid=?", View.CurrentObject));
                if (CurPhieu != null && curNV != null)
                {
                    bool CoChucvu = false;
                    if (curNV.chucdanh != null) CoChucvu = true;

                    if (CurPhieu.State == Define.EStatusVB.dahuy)
                    {
                        GuiDX.Active.SetItemValue("an", false);
                        //DuyetDX.Active.SetItemValue("an", false);
                    }
                    else if (CurPhieu.State == Define.EStatusVB.daduyet)
                    {
                        GuiDX.Active.SetItemValue("an", false);
                        //DuyetDX.Active.SetItemValue("an", false);
                        //xu ly
                        bool bXuly = false;
                        if (CurPhieu.State == Define.EStatusVB.daduyet && CurPhieu.Daxong == false)
                        {
                            if (CurPhieu.NguoiXuly != null)
                            {
                                if (CurPhieu.NguoiXuly.Oid == curNV.Oid)
                                {
                                    bXuly = true;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (CurPhieu.State == Define.EStatusVB.choduyet && CurPhieu.user.Oid == curNV.Oid)
                        {
                            if (CurPhieu.Dagui) { 
                                GuiDX.Caption = "Cancel";
                                sendNoti();
                                }
                            else
                                GuiDX.Caption = "Send";
                            GuiDX.Active.SetItemValue("an", true);
                        }
                        else
                            GuiDX.Active.SetItemValue("an", false);


                    }
                }
            }

        }
    }
}
