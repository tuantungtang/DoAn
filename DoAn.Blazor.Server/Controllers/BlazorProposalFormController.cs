using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.XtraRichEdit;
using DoAn.Module.BusinessObjects.Class;

namespace DoAn.Blazor.Server.Controllers
{
    public class BlazorProposalFormController : ObjectViewController<ListView, ProposalForm>
    {
        private ListViewProcessCurrentObjectController openPhieu;
        //public BlazorProposalFormController()
        //{
        //    TargetViewId = "ProposalForm_ListView";
        //    SimpleAction newPhieu = new(this, "newPhieu", "View")
        //    {
        //        Caption = "Create New Form",
        //        ImageName = "dxmoi",
        //        TargetViewId = "ProposalForm_ListView",
        //        ConfirmationMessage = "Bạn có muốn tạo phiếu đề xuất ?",
        //    };
        //    newPhieu.Execute += NewPhieu_Execute;
        //}

        //private void NewPhieu_Execute(object sender, SimpleActionExecuteEventArgs e)
        //{
        //    IObjectSpace objectSpace = Application.CreateObjectSpace(typeof(ProposalForm));
        //    ProposalForm pmoi = objectSpace.CreateObject<ProposalForm>();
        //    DetailView dView = Application.CreateDetailView(objectSpace, "Phieumoi", true, pmoi);
        //    Application.ShowViewStrategy.ShowViewInPopupWindow(dView, () => Lapphieu(pmoi, objectSpace));
        //}

        //protected override void OnActivated()
        //{
        //    base.OnActivated();
        //    openPhieu = Frame.GetController<ListViewProcessCurrentObjectController>();
        //    if (openPhieu != null)
        //    {
        //        openPhieu.CustomProcessSelectedItem += OpenChungtu_CustomProcessSelectedItem;
        //    }
        //}
        //void OpenChungtu_CustomProcessSelectedItem(object sender, CustomProcessListViewSelectedItemEventArgs e)
        //{
        //    e.Handled = true;
        //    if (View.CurrentObject is ProposalForm p)
        //    {
        //        IObjectSpace objectSpace = Application.CreateObjectSpace(typeof(ProposalForm));
        //        ProposalForm phieu = objectSpace.FindObject<ProposalForm>(CriteriaOperator.Parse("Oid=?", p.Oid));
        //        if (phieu != null)
        //        {
        //            Define.AddUserValue("viewdx", phieu.Oid.ToString());
        //            DashboardView dashboardView = Application.CreateDashboardView(objectSpace, "viewDX", true);
        //            //DetailView dView = Application.CreateDetailView(objectSpace, "Phieudexuat_DetailView", true, phieu);
        //            e.InnerArgs.ShowViewParameters.CreatedView = dashboardView;
        //            e.InnerArgs.ShowViewParameters.Context = TemplateContext.View;
        //            e.InnerArgs.ShowViewParameters.TargetWindow = TargetWindow.Default;
        //        }
        //    }
        //}
        //protected override void OnDeactivated()
        //{
        //    if (openPhieu != null)
        //    {
        //        openPhieu.CustomProcessSelectedItem -= OpenChungtu_CustomProcessSelectedItem;
        //    }
        //    base.OnDeactivated();
        //}
        //void Lapphieu(ProposalForm phieu, IObjectSpace objectSpace)
        //{
        //    if (Define.LapphieuDX(phieu, objectSpace))
        //    {
        //        IObjectSpace objectSpace1 = Application.CreateObjectSpace(typeof(ProposalForm));
        //        ProposalForm p = objectSpace1.GetObject(phieu);
        //        Define.AddUserValue("viewdx", p.Oid.ToString());
        //        DashboardView dashboardView = Application.CreateDashboardView(objectSpace1, "viewDX", true);
        //        ShowViewParameters svp = new()
        //        {
        //            CreatedView = dashboardView,
        //            TargetWindow = TargetWindow.Default,
        //            Context = TemplateContext.View
        //        };
        //        Application.ShowViewStrategy.ShowView(svp, new ShowViewSource(null, null));
        //    }
        //}

    }
}
