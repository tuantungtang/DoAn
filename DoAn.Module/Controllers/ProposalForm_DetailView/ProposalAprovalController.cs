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
            //DuyetDX = new(this, "DuyetDX", "View")
            //{
            //    Caption = "Duyệt",
            //    ImageName = "duyet",
            //    ToolTip = "Duyệt đề xuất",
            //    TargetViewId = "ProposalForm_DetailView",
            //};
            //DuyetDX.Execute += DuyetDX_Execute;
            //DuyetDX.Active["IsVisible"] = false;
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
                notifications.NotificationString = "There is a new form need your approval";
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

        //private void UpdateDuyetDXVisibility()
        //{ 
        //  DuyetDX.Active["IsVisible"] = curNV.chucdanh != null ;
        //}
        //private void DuyetDX_Execute(object sender, SimpleActionExecuteEventArgs e)
        //{
        //    try
        //    {
        //        if (curNV != null && CurrentBuocDuyet != null)
        //        {
        //            IObjectSpace objectSpace = Application.CreateObjectSpace(typeof(ProposalApproval));
        //            DetailView dView = Application.CreateDetailView(objectSpace, "ProposalApproval_DetailView", true, objectSpace.GetObject(CurrentBuocDuyet));
        //            e.ShowViewParameters.CreatedView = dView;
        //            e.ShowViewParameters.Context = TemplateContext.View;
        //            e.ShowViewParameters.TargetWindow = TargetWindow.Default;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Define.CustomError(ex.Message);
        //    }
        //}

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

                        //Duyet
                        //Session session = ((XPObjectSpace)ObjectSpace).Session;
                        //string sql = @"SELECT TOP (100) PERCENT State, user, OID
                        //FROM     dbo.ProposalApproval WHERE   (proposalform = " + CurPhieu.Oid + ") ORDER BY Step";
                        //SelectedData results2 = session.ExecuteQuery(sql);
                        //foreach (SelectStatementResultRow row in results2.ResultSet[0].Rows)
                        //{
                        //    int trangthai = CommonLib.CInt(row.Values[0]);
                        //    string nvId = CommonLib.CString(row.Values[1]);
                        //    int duyetId = CommonLib.CInt(row.Values[2]);
                        //    if (nvId == "CommonLib.CString(curNV.Oid)")
                        //    {
                        //        CurrentBuocDuyet = ObjectSpace.FindObject<ProposalApproval>(CriteriaOperator.Parse("Oid=?", duyetId));
                        //    if (CurrentBuocDuyet != null)
                        //    {
                        //        DcDuyet = true;
                        //        int sott = CurrentBuocDuyet.Step + 1;
                        //        ProposalApproval duyetsau = ObjectSpace.FindObject<ProposalApproval>(CriteriaOperator.Parse("Step=? && proposalform=?", sott, CurPhieu));
                        //        if (duyetsau != null)//neu duyet sau <> cho thi khong dc duyet nua
                        //        {
                        //            if (duyetsau.State != Define.EStatusDuyet.choduyet)
                        //            {
                        //                DcDuyet = false;
                        //            }
                        //        }
                        //        break;
                        //    }
                        //    }
                        //}
                        //DuyetDX.Active.SetItemValue("an", DcDuyet);
                    }
                }
            }

        }
    }
}
